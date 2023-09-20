using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
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

    // The way that dependency injection works is that when our framework creates a new instance of the AuctionsController, which it will do when it receives a request into this particular route of "api/auctions", 
    // then it's going to take a look at the arguments inside the AuctionsController and it's going to say, Right, okay, I see you want a dbcontext and a mapper and it's going to instantiate these classes and 
    // make them available inside here
    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;  

        _mapper = mapper; 
    }

    // Now that we have that, let's create a couple of endpoints so that we can test our progress and we'll create an [HttpGet] because this is 
    // going to be a get request so that we can go and get our data from the database. What we're returning from this is an ActionResult and 
    // an ActionResult lets us send back Http responses such as a 200 or a 404 not found. Inside the ActionResult, we're going to specify that 
    // we want to return a List of AuctionDto
    // Remember, we don't want to return an Auction object, but an AuctionDto object
    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {   
        // we want to include or load our related properties here. And the related property is the item. So we're going to specify Include. 
        // And I'll specify that I want the X goes to X.item, so let's include it along with the auction. I'm going to specify some ordering just to give them some kind of order and I'm going to order by. The X goes to X.Item.Make. And I'll specify two list async.
        var auctions = await _context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();
        
        // to return, we'll use Automapper and we're going to say we want to map to a list of AuctionDtos. And get that from the auctions.
        return _mapper.Map<List<AuctionDto>>(auctions);
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

}