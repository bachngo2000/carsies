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
    public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm, int pageNumber = 1, int pageSize = 4)
    {   
        var query = DB.PagedSearch<Item>();

        // sort by the name of the Make
        query.Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrEmpty(searchTerm)) {
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }

        query.PageNumber(pageNumber);
        query.PageSize(pageSize);

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }

}