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
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {   
        // first, we create the query, which is a page search
        var query = DB.PagedSearch<Item, Item>();

        // then, we check to see if we've got a search term in our query
        if (!string.IsNullOrEmpty(searchParams.SearchTerm)) {
            // if we do, we're gonna match it
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        // Then, we're going to go thru the rest of our query parameters
        // So even when they're searching, it's going to be ordered based on user's selection
        query = searchParams.OrderBy switch
        {   
            // ordered by make
            "make" => query.Sort(x => x.Ascending(a => a.Make)),

            // ordered by newest
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            // default sorting
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };

        // So even when they're searching,  it's going to be filtered based on the user's selection.
        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
                && x.AuctionEnd > DateTime.UtcNow),
            // default filtering
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        //  And if we've got something in the Seller, then of course, that's going to just display the auctions that that user is selling
        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        // And if we've got something in the Winner, then of course, that's going to just display the auctions that that user has won
        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        // page the results
        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }

}