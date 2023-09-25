using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

// created a Web API controller so we can start returning some data from our database in Json format to the client 
// controller containing API endpoints
[ApiController]
// route so that the framework knows where to direct the HTTP requests when it comes into the server
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{   
    // And this is where we get to use dependency injection. Inside our program class, the services that we're creating, such as the Dbcontext 
    // and Automapper, in this case, when we want to make use of these services, we can inject them into classes in our application and 
    // we need to use our dbcontext so that we can access the data and we need to access to automapper so that we can shape the data and automatically map it from the auction entity that we get from the database to the auction DTO, which is what we want to return to the client.
    private readonly AuctionDbContext _context;

    private readonly IMapper _mapper;

    private readonly IPublishEndpoint _publishEndpoint;

    // The way that dependency injection works is that when our framework creates a new instance of the AuctionsController, which it will do when it receives a request into this particular route of "api/auctions", 
    // then it's going to take a look at the arguments inside the AuctionsController and it's going to say, Right, okay, I see you want a dbcontext and a mapper and it's going to instantiate these classes and 
    // make them available inside here
    // inject IPublishEndpoint from MassTransit into the AuctionsController class to allow us to publish the message to the service bus
    public AuctionsController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;  

        _mapper = mapper; 

        _publishEndpoint = publishEndpoint;
    }

    // Now that we have that, let's create a couple of endpoints so that we can test our progress and we'll create an [HttpGet] because this is 
    // going to be a get request so that we can go and get our data from the database. What we're returning from this is an ActionResult and 
    // an ActionResult lets us send back Http responses such as a 200 or a 404 not found. Inside the ActionResult, we're going to specify that 
    // we want to return a List of AuctionDto
    // Remember, we don't want to return an Auction object, but an AuctionDto object

    // New Update: 3.31
    // Rather than getting all the auctions every time the service starts up, it would be better to say, Hey, this is the latest auction I've got with the with an updated date of this. 
    // Give me all the auctions you have after this particular date. So we're going to take in a date property as a query string inside the GetAllAuctions method.
    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
    {   
        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        // check for the presence of the date
        if (!string.IsNullOrEmpty(date))
        {   
            // in order to compare DateTime, we need to parse date, which is a string
            // only return auctions whose updatedAt date are later than the date 
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        // we want to include or load our related properties here. And the related property is the item. So we're going to specify Include. 
        // And I'll specify that I want the X goes to X.item, so let's include it along with the auction. I'm going to specify some ordering just to give them some kind of order and I'm going to order by. The X goes to X.Item.Make. And I'll specify two list async.
        // var auctions = await _context.Auctions
        //     .Include(x => x.Item)
        //     .OrderBy(x => x.Item.Make)
        //     .ToListAsync();
        
        // // to return, we'll use Automapper and we're going to say we want to map to a list of AuctionDtos. And get that from the auctions.
        // return _mapper.Map<List<AuctionDto>>(auctions);

        // 
        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    // create another endpoint so we can get an individual auction
    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id) {
        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (auction == null)
            return NotFound();
        

        return _mapper.Map<AuctionDto>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto) {

        // first, we map the CreateAuctionDto auctionDto into an Auction entity & we can use Automapper for that
        // So we'll specify what we want to map into. So this is going to be Auction to represent the Auction entity. And we can pass it the auctionDto
        var auction = _mapper.Map<Auction>(auctionDto);

        //TODO: add current user as seller

        auction.Seller = "test";

        // we can then add the auction using entity framework.
        // And what's happening here is entity framework is effectively tracking this in memory. So nothing's been saved to the database at this point. This is simply being added to memory and entity framework is tracking this because it is an entity
        _context.Auctions.Add(auction);

        // We re-arranged and moved the two statements below above the SaveChangeAsync() statement
        // now we've added the outbox. This becomes part of our entity framework transaction so we can now move this up effectively and we're going to move the mapping functionality above the save changes. 
        // And what's going to happen now is these are going to be treated like a transaction and either they all work or none of them work. And that means that if we try and publish a message to our outbox, which is what's going to happen if the service bus is down, if that fails, then the whole transaction fails. If the service bus is up, then that's fine. We just publish our message and it carries on as it normally would and gets delivered to the bus and we save the changes. And that's the reason why if one fails, they all fail, is because now we've got this code before the save changes. And because this is using entity framework, then it's part of the same transaction that we're saving to the database here.
        // After saving changes to our database, we will wait until after we have the ID from the database, then we mapped the auction into an AuctionDto 
        var newAuction = _mapper.Map<AuctionDto>(auction);

        // publish to the service bus as an AuctionCreated object
        await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

        // now, we can actually save this to the database. We'll say greater than zero because this SaveChangesAsync method returns an integer for each change it was able to save in the database. If it returns zero, that means nothing was saved into our database and we know our result is going to be false. But if the changes were more than zero, then we can presume that was successful and this will evaluate to true
        var result = await _context.SaveChangesAsync() > 0;

        // So we'll check the results and we'll say if result is not greater than 0, then we'll simply return a bad request. And we'll say could not save changes to the DB
        if (!result) {
            return BadRequest("Could not save changes to the DB");
        }

        //And what we want to return from a post request is what we should return is an Http with a status code of 201 created to say that we've created a resource and we also need to tell the client where the resource was created at. 
        // So we have a method inside here that we can use to do such a thing. And what we'll return is a CreatedAtAction. And we can specify the name of the action where this resource can be found. 
        // So in this case, we've got a method here called GetAuctionById, and this is the location we would want to send back in the header to tell the client that, Hey, yes, we've created your resource and this is the location you can get your resource. 
        // So this particular endpoint is where we'd want them to know about if they wanted to get the resource that's been created. So we can specify a name of and get auction by ID. And this particular method, this takes an argument of the Guid of the auction so we can specify as a second parameter in the created at action, we can specify new and then we can simply specify the auction ID as the parameter that's needed for this particular action or endpoint. 
        // And then as a third parameter, we can return the AuctionDto. So in order to return an AuctionDto from this, we need to go from our Auction entity into an AuctionDto. So once again, we'll utilize mapper functionality for this and we'll map into an AuctionDto from the Auction
        return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, /*_mapper.Map<AuctionDto>(auction)*/ newAuction);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {   
        // first, we go and get the auction from the database
        // we need to include the items b/c we're updating the car properties in this
        var auction = await _context.Auctions.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        // check if the auction exists
        if (auction == null) return NotFound();

        // TODO: check seller == username

        // From here, all we want to do is update the current properties of the auctions to the updated properties in the updateAuctionDto or if that's not provided, we want to keep the original property of the entity.
        // So we're going to say the auction.Item.Make is equal to the updateAuctionDto.Make and if that's null or undefined, then we're going to use the null conditional Operator ?? and we're going to set the auction.Item.Make to what it was inside the entity.
        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        // publish to the service bus as an AuctionUpdated object
        // after updating the auction, we need to add the publish endpoint so that when we update the auction, we publish that endpoint. But I've got a mapping in here for the auction updated to go from the Auction entity to the AuctionUpdated
        await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));

        // now, we can actually save this to the database. We'll say greater than zero because this SaveChangesAsync method returns an integer for each change it was able to save in the database. If it returns zero, that means nothing was saved into our database and we know our result is going to be false. But if the changes were more than zero, then we can presume that was successful and this will evaluate to tru
        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Problem saving changes");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {   
        // get the auction from the database
        var auction = await _context.Auctions.FindAsync(id);

        if (auction == null) return NotFound();

        // TODO: check seller == username

        _context.Auctions.Remove(auction);

        await _publishEndpoint.Publish<AuctionDeleted>(new { Id = auction.Id.ToString() });

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }

}