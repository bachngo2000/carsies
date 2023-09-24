using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

// added an AuctionCreatedConsumer consumer to consume a message/an event from the service bus
// the IConsumer interface takes in the type of event that we're consuming - AuctionCreated
// Now, we also need a mapping profile so that we can go from AuctionCreated to the Item, which is what MongoDB needs to save
public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;
    // use dependency injection to inject AutoMapper into the AuctionCreatedConsumer class
    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    // To consume the AuctionCreated message when it arrives from the service bus
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {   
        // specify the id of the auction
        Console.WriteLine("--> Consuming auction created: " + context.Message.Id);

        // create an item from the message
        var item = _mapper.Map<Item>(context.Message);

        // save the item in the mongo database
        await item.SaveAsync();
    }
}
