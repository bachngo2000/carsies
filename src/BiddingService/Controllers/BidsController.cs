using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    // of type ActionResult and returns a Bid from this endpoint
    // we want this to be authenticated since we don't want anonymous users trying to create bids on auctions
    // to get to place a bid, then the request is eventually going to come through to API bids
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Bid>> PlaceBid(string auctionId, int amount) {

        // get hold of the auction from our database
        // But we don't have any auctions in our bidding service and we're not going to seed any auctions into our bid service either. Now what we will do is we'll have a consumer. So when a new auction is created, then we're going to consume that event.
        var auction = await DB.Find<Auction>().OneAsync(auctionId);

        // if we don't have an auction
        if (auction == null)
        {
            // TODO: check with auction service if that has auction
            // So in order to test our bidding functionality, we're going to actually need to at this stage because we don't have a way of going to our auction service yet and getting an auction if we don't have it in our database on this side in the bidding service. So we're going to need to create a consumer to get hold of and populate our database with a new auction when it is created.
            return NotFound();
        }

        // check if the bidder is the seller of the auction
        // we are making this an authenticated request. So to get into this controller method, then they are going to need to be authorized and we should have access to their user identity name.
        if (auction.Seller == User.Identity.Name)
        {
            return BadRequest("You cannot bid on your own auction");
        }

        // create a new bid
        var bid = new Bid
        {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = User.Identity.Name
        };

        // check the bid status to see if we can accept this bid or not
        if (auction.AuctionEnd < DateTime.UtcNow) {
            bid.BidStatus = BidStatus.Finished;
        }

        else {
            // get the current high bid for this auction
            var highBid = await DB.Find<Bid>()
                .Match(a => a.AuctionId == auctionId)
                .Sort(b => b.Descending(x => x.Amount))
                .ExecuteFirstAsync();

            if (highBid != null && amount > highBid.Amount || highBid == null)
            {
                bid.BidStatus = amount > auction.ReservePrice
                    ? BidStatus.Accepted
                    : BidStatus.AcceptedBelowReserve;
            }

            if (highBid != null && bid.Amount <= highBid.Amount)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        // save our bid to the database
        await DB.SaveAsync(bid);

        return Ok(bid);
    }

    // return a list of bids for a particular auction based on its auctionId
    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<Bid>>> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(a => a.BidTime))
            .ExecuteAsync();

        return bids;
    }

}
