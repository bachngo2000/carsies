using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService;

// produce an event when the auction finishes
// we need to check the AuctionEnd date. And when that end date has been reached, then we need to send the AuctionFinished event (in Contracts) to our service bus.
// In the AuctionFinished event, we have a flag (ItemSold) to say the item has sold. If the auction has bids and the bid is greater than the reserve price, then we're going to consider that item sold and flag this property as true. 
// And we'll also have the AuctionID, the Winner, if there is one, and the Seller and the Amount in the AuctionFinished event. So our challenge here really 
// is what to do about this. And there are different options When we receive this AuctionCreated event (Contracts), we're going to know the AuctionEnd date.
// Our approach is we could have a background task that runs on our service and checks to see if there are any auctions that have finished, as in the dates has gone past their AuctionEnd date and have not been marked as finished. 
// And for those ones, if we check on a periodic basis that say every five or 10s, then we can check to see if there's any auctions that have ended that have not yet been marked as finished. And then for each of those, we can send an event to the service bus. We're going to have a background service running that's simply going to poll our database and see if there's any auctions that have met the auction end dates but have not been marked as finished. And for each of those, we're going to send an event on the service bus.

// IHostedService - this is running as a singleton, meaning it's gonna run when our application starts up and it's only gonna stop when our application shuts down
public class CheckAuctionFinished : BackgroundService
{   
    private readonly ILogger<CheckAuctionFinished> _logger;

    private readonly IServiceProvider _services;

    public CheckAuctionFinished(ILogger<CheckAuctionFinished> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    // the CancellationToken is Triggered when IHostedService.StopAsync(CancellationToken) is called and we can make use of the stoppingToken to stop execution of any running requests
    // when this is provided, such as our application shuts down, then we want to stop any database activity
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting check for finished auctions");

        stoppingToken.Register(() => _logger.LogInformation("==> Auction check is stopping"));

        // as long as this cancellation has not been requested
        while (!stoppingToken.IsCancellationRequested) {
            await CheckAuctions(stoppingToken);

            // run every 5 seconds
            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task CheckAuctions(CancellationToken stoppingToken)
    {   
        // querying the MongoDB databasea and get a list of all the auctions that have ended but have not yet been marked as finished
        var finishedAuctions = await DB.Find<Auction>()
            .Match(x => x.AuctionEnd <= DateTime.UtcNow)
            .Match(x => !x.Finished)
            // if our service does stop, then we're not gonna continue running the query inside MongoDB
            .ExecuteAsync(stoppingToken);

        // if there's not any matching auction, then we just return
        if (finishedAuctions.Count == 0) {
            return;
        }

        // otherwise, print how many matching auctions we have
        _logger.LogInformation("==> Found {count} auctions that have completed", finishedAuctions.Count);

        // create a scope so that we can access the IPublishEndpoint because this background service is going to run as a singleton. The mass transit service (IPublishEndpoint) 
        // lifetime is scoped to the scope of the request. So we can't inject something that's got a different lifetime into our background service that we're creating here. So we have to create a scope inside this so that we can get access to IPublishEndpoint and we can actually publish the event for the finished auction.
        using var scope = _services.CreateScope();

        // create our endpoint and get hold of the IPublishEndpoint
        var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        // loop over our finished auctions
        foreach (var auction in finishedAuctions)
        {   
            // update the Finished flag
            auction.Finished = true;
            await auction.SaveAsync(null, stoppingToken);

            // get our winning bid
            var winningBid = await DB.Find<Bid>()
                .Match(a => a.AuctionId == auction.ID)
                .Match(b => b.BidStatus == BidStatus.Accepted)
                .Sort(x => x.Descending(s => s.Amount))
                .ExecuteFirstAsync(stoppingToken);

            // using our endpoint to publish a new AuctionFinished event for each finished auction to the service bus
            await endpoint.Publish(new AuctionFinished
            {
                ItemSold = winningBid != null,
                AuctionId = auction.ID,
                Winner = winningBid?.Bidder,
                Amount = winningBid?.Amount,
                Seller = auction.Seller
            }, stoppingToken);
        }
    }
}
