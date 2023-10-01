using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService;

// We need to create a consumer so that we can consume the auction created event. And that means that our bid service is going to need access to our contracts project.
public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        // create an auction
        var auction = new Auction
        {
            ID = context.Message.Id.ToString(),
            Seller = context.Message.Seller,
            AuctionEnd = context.Message.AuctionEnd,
            ReservePrice = context.Message.ReservePrice
        };

        await auction.SaveAsync();
    }
}

