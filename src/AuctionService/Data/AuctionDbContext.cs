using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

// derived from the entity framework class DB Context
public class AuctionDbContext : DbContext
{   
    // simple constructor with options we need for us to provide the options to the DBContext base class
    // options that we need: provide it with the database provider that we're using and the connection string
    public AuctionDbContext(DbContextOptions options) : base(options) {

    }

    // tell the DbContext class about the entities(tables) we have in our project
    // we use DbSet as that is what we use to tell entity framework
    // table Auctions in our database
    // Note: we do not need to specify the Item inside here b/c our item is related to the Auctions and entity framework is going to create
    // the table for Item as well as Auctions since they're related
    public DbSet<Auction> Auctions {get; set;}

    
}