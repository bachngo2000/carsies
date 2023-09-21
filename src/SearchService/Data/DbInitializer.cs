using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;
public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        // initialize our MongoDB database
        await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        // What we're also going to do, because this is going to act as a search server, is we're going to create an index for our item for the 
        // certain fields that we want to be able to search on.
        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync(); 

        // count the number of auctions in our database
        var count = await DB.CountAsync<Item>();

        // if (count == 0)
        // {
        //     Console.WriteLine("No data - will attempt to seed");

        //     var itemData = await File.ReadAllTextAsync("Data/auctions.json");

        //     var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        //     // So this effectively is going to take that Json formatted document and effectively convert this into a list of items in .net format.
        //     var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

        //     await DB.SaveAsync(items);
        // }

        // using HTTP to get the data

        // since we need access to our service, the AuctionSvcHttpClient service, and when we run our DBInitalizer, we're not able to inject anything into this, so we need to use using
        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine(items.Count + " returned from the auction service");

        // check how many items we have
        // we only make a call to the database if we actually have auctions to save
        if (items.Count > 0) await DB.SaveAsync(items);


    }

}
