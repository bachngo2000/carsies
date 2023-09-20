using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{   
    // And inside this particular controller we don't need to provide a constructor, we don't need to inject the MongoDB entities that we're using. We can simply just use it because it's a static class.
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm)
    {   
        // use Find to return a full list of items w/o a search term
        var query = DB.Find<Item>();

        // sort by the name of the Make
        query.Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrEmpty(searchTerm)) {
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }

        var result = await query.ExecuteAsync();

        return result;
    }

}