using MongoDB.Entities;

namespace SearchService.Models;
public class Item : Entity
{   
    // The Entity class provides an ID for our item and effectively in our MongoDB database, we're going to have collections of documents
    // and for this Item : Entit, because we've derived from Entity, when we do initialize our database, this is considered an entity class now inside MongoDB 
    // and we don't actually need to give it its own ID because this is coming from entity

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
