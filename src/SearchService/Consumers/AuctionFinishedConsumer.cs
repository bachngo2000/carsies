using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;
public class AuctionFinishedConsumer: IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        // get the specific auction
        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

        // check if the auction has been sold
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = (int)context.Message.Amount;
        }

        // update auction status
        auction.Status = "Finished";

        // save everything to the Mongo database
        await auction.SaveAsync();
    }


}
