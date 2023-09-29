'use server'
import { Auction, PagedResult } from "@/types";

//fetch our data using server side fetching, going from our NodeJS server to our API, come back to our NodeJS server. Our next JS server is going to get the data and then it's going to return that data to our React component
// as HTML to the client. So the client is going to be completely unaware of where this data is coming from. As far as our client is concerned, this is coming from our client server, the next JS server.
// added 'Promise<PagedResult<Auction>>' so getData() knows it returns a PagedResult of type auctions so we can use types with our data
// we moved this function from Listings.tsx to here
// specify a param and give it an init value of 1
export async function getData(pageNumber: number=1): Promise<PagedResult<Auction>> {
    // fetch caches the data that's coming back from our API
    // hardcoded the http we're gonna use, this needs to match our gateway service and search is the service that we're retrieving the data from
    // add the query string of "pageSize=4" to our search to retrieve 4 of the cars we have in our search database in each page
    const res = await fetch(`http://localhost:6001/search?pageSize=4&pageNumber=${pageNumber}`);

    if (!res.ok) {
        throw new Error('Failed to fetch data');
    }
    // use .json function to get the body of the response out of the response message
    return res.json();

    
}