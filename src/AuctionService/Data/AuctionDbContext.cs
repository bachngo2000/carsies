using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

// derived from the entity framework class DB Context
public class AuctionDbContext : DbContext
{   
    // simple constructor
    public AuctionDbContext(DbContextOptions options) : base(options) {

    }

    // tell the DbContext class about the entities we have in our project
    // we use DbSet as that is what we use to tell entity framework
    // table Auctions in our database
    public DbSet<Auction> Auctions {get; set;}

    
}