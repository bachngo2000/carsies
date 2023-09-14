namespace AuctionService.Entities;

public class Aunction {

    // create entity properties/columns of the 

    // primary key for the table
    public Guid Id {get; set;}

    public int ReservePrice{get; set;} = 0;

    public string Seller{get; set;}

    public string Wiiner{get; set;}

    //optional
    public int? SoldAmount{get; set;}

    //optional
    public int? CurrentHighBid{get; set;}

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;

    public DateTime AuctionEnd {get; set;} = DateTime.UtcNow;

    public Status Status {get; set;}

    public Item Item {get; set;}


}