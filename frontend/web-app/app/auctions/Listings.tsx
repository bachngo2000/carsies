import React from 'react'

// fetch our data using server side fetching, going from our NodeJS server to our API, come back to our NodeJS server. Our next JS server is going to get the data and then it's going to return that data to our React component
// as HTML to the client. So the client is going to be completely unaware of where this data is coming from. As far as our client is concerned, this is coming from our client server, the next JS server.
async function getData() {
    // fetch caches the data that's coming back from our API
    // hardcoded the http we're gonna use, this needs to match our gateway service and search is the service that we're retrieving the data from
    const res = await fetch("http://localhost:6001/search");

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
    <div>
        {JSON.stringify(data, null, 2)}
    </div>
  )
}
