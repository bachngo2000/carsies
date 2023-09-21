using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;
public class AuctionSvcHttpClient
{   
    // initialize and assign these fields so that we have access to them in this class.
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    // create a constructor as we need to inject a few things into this particular service.
    // the first thing we need is the Http client so that this search service can make an Http request to our Auction service
    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {   
        // get the date of the auction that's been last updated in our database
        // return a string
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString()) // project this so that we just get the string of the date
            .ExecuteFirstAsync();

        // use the GetFromJsonAsync so it automatically deserializes the Json that we get back from the Auction service into a List of Items
        return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] 
            + "/api/auctions?date=" + lastUpdated);
    }

}
