namespace Contracts;

// represents an event in which an auction has been created in the Auction service
// this AuctionCreated event is effectively the auction DTO that we're going to pass on to our service bus and then eventually be picked up by our search service. 
// Our search service doesn't even know what auction DTO is, but it is going to know what an auction created object looks like.
// And when we send this event onto the bus to say that, hey, an auction has been created, then once the service consumes this particular message, it will then be able to create the auction or the item, as we've called it, in the search service inside that database.
public class AuctionCreated
{
    public Guid Id {get; set;}

    public int ReservePrice{get; set;} = 0;

    public string Seller{get; set;}

    public string Winner{get; set;}

    public int SoldAmount{get; set;}

    public int CurrentHighBid{get; set;}

    public DateTime CreatedAt {get; set;} 

    public DateTime UpdatedAt {get; set;}

    public DateTime AuctionEnd {get; set;}

    public string Status {get; set;}

    public string Make {get; set;}

    public string Model {get; set;}

    public int Year {get; set;}

    public string Color {get; set;}

    public int Mileage {get; set;}

    public string ImageUrl {get; set;}

}
