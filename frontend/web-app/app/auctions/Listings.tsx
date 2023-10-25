'use client'
import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import { Auction, PagedResult } from '@/types';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filters from './Filters';
import { shallow } from 'zustand/shallow';
import { useParamsStore } from '@/hooks/useParamsStore';
import queryString from 'query-string';
import EmptyFilter from '../components/EmptyFilter';
import { useAuctionStore } from '@/hooks/useAuctionStore';

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

  // add local state for loading
  const [loading, setLoading] = useState(true);

  // we need to store some states in our Listings component here for our pagination functionality to work b/c the getData() function above only gets called once, and so it only returns 4 auctions, but we want all 10 auctions
  // But React states require client side components, but Listings is currently a server side component.  Therefore, we will turn Listings into a client side component, but the getData() function above is a server-side function, so to continue using it, we will comment out the getData function above and make a new server side component for it, called auctionActions          

  // this allows us to do a side effect when the listings component first loads
  // and then depending on what happens, on what we're using inside this use effect, it may cause the component to rerender based on the code inside here
  // get the data out of what the getData() method returns
  // store our auctions in auctions and we can specify the type of state that we're going to get here, and it's going to be an Auction array and we're going to start off with an empty array
  // replace the local states with Zustand to manage states
  // const [auctions, setAuctions] = useState<Auction[]>([]);

  // const [pageCount, setPageCount] = useState(0);

  // const [pageNumber, setPageNumber] = useState(1);

  // const [pageSize, setPageSize] = useState(4);

  // use useParamsStore
  // Since we're storing our actions in local state, that's not helping us when it comes to updating these from another part of our application
  // when it comes to the SignalR functionality we're gonna add, so we're creating a store (useAuctionStore.ts) to store our auctions
  // update Listings to use our useAuctionStore store to store auctions instead of storing them in local state
  // const[data, setData] = useState<PagedResult<Auction>>();
  // get all of the params
  // the shallow method ensures that we don't get all the states back 
  const params = useParamsStore(state => ({
    pageNumber: state.pageNumber,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
    orderBy: state.orderBy,
    filterBy: state.filterBy,
    seller: state.seller,
    winner: state.winner

  }), shallow)

  // update Listings to use useAuctionStore to store auctions instead of storing them in local state using "const[data, setData] = useState<PagedResult<Auction>>()" like above
  const data = useAuctionStore(state => ({
    auctions: state.auctions,
    totalCount: state.totalCount,
    pageCount: state.pageCount
  }), shallow);

  // get the setData method from useAuctionStore
  const setData = useAuctionStore(state => state.setData);

  // get the method to set params so that we can use it to set our page number that we're gonna pass down for our AppPagination
  const setParams = useParamsStore(state => state.setParams);
  const url = queryString.stringifyUrl({url: '', query: params})

  //function to set the page number
  function setPageNumber(pageNumber:number) {
    // So this set params is going to go to our useParamsStore.ts and effectively the setParams function inside there is going to be executed because it is setting the page number, it's going to call the if statement rather than call the else statement
    setParams({pageNumber})
  }

  // so with this new change using Zustand, when the URL changes, this useeffect is called and we go and get our new data with our new query string. And then we set the data in our local state here
  useEffect(() => {
    getData(/*pageNumber, pageSize*/ url).then(data => {
      // setAuctions(data.results);
      // setPageCount(data.pageCount);
      setData(data);

      // turn off our loading flag
      setLoading(false);
    })
    // our effect needs to know what our dependecies are, so if we don't have any dependencies then we would use an empty array to say that this use effect is going to run once and only once ever. 
    // But if we do want this use effect to be called again, when something it depends on such as the page number changes, then we put the pageNumber in as a dependency as we are doing here.
    // And whenever the page number changes, use effect gets called again and our component gets re rendered with the updated results
    // the same applies to pageSize
    // replace pageNumber, pageSize with the url as our new depency
  }, [/*pageNumber, pageSize*/ url])

  // instead of checking if we have no data, we can check for loading
  if (/*!data*/ loading) {
    return <h3>Loading car auctions...</h3>
  }

  useEffect
  return (
    // use React fragment
    <>
      <Filters /*pageSize={pageSize} setPageSize={setPageSize}*/ />
      {data.totalCount == 0 ? (
        <EmptyFilter showReset/>
      ) : (
        <>
          <div className='grid grid-cols-4 gap-6'>
            {/* Then, we map/loop through each auction inside the "data.auctions" object and return an Auction Card for each of these auctions that we have and give each card a key to uniquely identify them   */}
            {data.auctions.map((auction) => (
              <AuctionCard auction={auction} key={auction.id} />
             ))}
          </div>

          {/*use a grid to lay out the cards (4 per page and a gap of 6 between them) on our Listings page*/}
          <div className='flex justify-center mt-4'>
            <AppPagination pageChanged={setPageNumber} currentPage={/*pageNumber*/ params.pageNumber} pageCount={/*pageCount*/ data.pageCount}/>
          </div>
        </>
      )}
      
    </>

  )
}
