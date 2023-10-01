using MongoDB.Entities;

namespace BiddingService;
public class Auction : Entity
{
    // create some properties that our Bidding service needs to know about for our auction
    public DateTime AuctionEnd {get; set;}

    public string Seller {get; set;}

    public int ReservePrice {get; set;}

    public bool Finished {get; set;}

}
