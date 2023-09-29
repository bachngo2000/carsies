import React from 'react'
import AuctionCard from './AuctionCard';

// fetch our data using server side fetching, going from our NodeJS server to our API, come back to our NodeJS server. Our next JS server is going to get the data and then it's going to return that data to our React component
// as HTML to the client. So the client is going to be completely unaware of where this data is coming from. As far as our client is concerned, this is coming from our client server, the next JS server.
async function getData() {
    // fetch caches the data that's coming back from our API
    // hardcoded the http we're gonna use, this needs to match our gateway service and search is the service that we're retrieving the data from
    // add the query string of "pageSize=10" to our search to retrieve all the cars we have in our search database
    const res = await fetch("http://localhost:6001/search?pageSize=10");

    if (!res.ok) {
        throw new Error('Failed to fetch data');
    }
    // use .json function to get the body of the response out of the response message
    return res.json();

    
}

// to use the data returned from getData(), since this is still a server side function, we need to make it an async function
export default async function Listings() {

    // get the data returned from getData()
    const data = await getData();

  return (
    // use a grid to lay out the cards (4 per page and a gap of 6 between them) on our Listings page
    <div className='grid grid-cols-4 gap-6'>
        {/* make sure we have data coming back from our server.  Also, data.results is that's the object our auctions are coming back in.  */}
        {/* Then, we map/loop through each auction inside the "results" object and return an Auction Card for each of these auctions that we have inside of results and give each card a key to uniquely identify them   */}
        {data && data.results.map((auction: any) => (
          <AuctionCard auction={auction} key={auction.id} />
        ))}
    </div>
  )
}