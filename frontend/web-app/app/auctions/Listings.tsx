'use client'
import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import { Auction } from '@/types';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';

// fetch our data using server side fetching, going from our NodeJS server to our API, come back to our NodeJS server. Our next JS server is going to get the data and then it's going to return that data to our React component
// as HTML to the client. So the client is going to be completely unaware of where this data is coming from. As far as our client is concerned, this is coming from our client server, the next JS server.
// added 'Promise<PagedResult<Auction>>' so getData() knows it returns a PagedResult of type auctions so we can use types with our data
// comment out the function to make a new server side component for it so we can turn Listings into a client side component, so we can use React states/hooks
// async function getData(): Promise<PagedResult<Auction>> {
//     fetch caches the data that's coming back from our API
//     hardcoded the http we're gonna use, this needs to match our gateway service and search is the service that we're retrieving the data from
//     add the query string of "pageSize=4" to our search to retrieve 4 of the cars we have in our search database in each page
//     const res = await fetch("http://localhost:6001/search?pageSize=4");

//     if (!res.ok) {
//         throw new Error('Failed to fetch data');
//     }
//      use .json function to get the body of the response out of the response message
//     return res.json();

    
// }

// to use the data returned from getData(), since this is no longer a server side function, we will remove the async keyword from it bc we're gonna use a useEffect inside here and normal promises rather than await
export default function Listings() {

  // store our auctions in auctions and we can specify the type of state that we're going to get here, and it's going to be an Auction array and we're going to start off with an empty array
  const [auctions, setAuctions] = useState<Auction[]>([]);

  const [pageCount, setPageCount] = useState(0);

  const [pageNumber, setPageNumber] = useState(1);

  const [pageSize, setPageSize] = useState(4);

  // we need to store some states in our Listings component here for our pagination functionality to work b/c the getData() function above only gets called once, and so it only returns 4 auctions, but we want all 10 auctions
  // But React states require client side components, but Listings is currently a server side component.  Therefore, we will turn Listings into a client side component, but the getData() function above is a server-side function, so to continue using it, we will comment out the getData function above and make a new server side component for it, called auctionActions          

  // this allows us to do a side effect when the listings component first loads
  // and then depending on what happens, on what we're using inside this use effect, it may cause the component to rerender based on the code inside here
  // get the data out of what the getData() method returns
  useEffect(() => {
    getData(pageNumber, pageSize).then(data => {
      setAuctions(data.results);
      setPageCount(data.pageCount);
    })
    // our effect needs to know what our dependecies are, so if we don't have any dependencies then we would use an empty array to say that this use effect is going to run once and only once ever. 
    // But if we do want this use effect to be called again, when something it depends on such as the page number changes, then we put the pageNumber in as a dependency as we are doing here.
    // And whenever the page number changes, use effect gets called again and our component gets re rendered with the updated results
    // the same applies to pageSize
  }, [pageNumber, pageSize])

  if (auctions.length == 0) {
    return <h3>Loading...</h3>
  }

  useEffect
  return (
    // use React fragment
    <>
    <Filters pageSize={pageSize} setPageSize={setPageSize}/>
      {/*use a grid to lay out the cards (4 per page and a gap of 6 between them) on our Listings page*/}
      <div className='grid grid-cols-4 gap-6'>
        {/* Then, we map/loop through each auction inside the "actions" object and return an Auction Card for each of these auctions that we have inside of auctions and give each card a key to uniquely identify them   */}
        {auctions.map((auction) => (
          <AuctionCard auction={auction} key={auction.id} />
        ))}
      </div>

      <div className='flex justify-center mt-4'>
        <AppPagination pageChanged={setPageNumber} currentPage={pageNumber} pageCount={pageCount}/>
      </div>
    </>

  )
}
