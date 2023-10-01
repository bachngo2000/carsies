'use server'
import { Auction, PagedResult } from "@/types";
import { getTokenWorkaround } from "./authActions";
import { fetchWrapper } from "@/lib/fetchWrapper";
import { FieldValues } from "react-hook-form";
import { revalidatePath } from "next/cache";

//fetch our data using server side fetching, going from our NodeJS server to our API, come back to our NodeJS server. Our next JS server is going to get the data and then it's going to return that data to our React component
// as HTML to the client. So the client is going to be completely unaware of where this data is coming from. As far as our client is concerned, this is coming from our client server, the next JS server.
// added 'Promise<PagedResult<Auction>>' so getData() knows it returns a PagedResult of type auctions so we can use types with our data
// we moved this function from Listings.tsx to here
// specify params
export async function getData(query:string): Promise<PagedResult<Auction>> {
    // fetch caches the data that's coming back from our API
    // hardcoded the http we're gonna use, this needs to match our gateway service and search is the service that we're retrieving the data from
    // add the query string of "pageSize=${pageSize}" to allow users to specify how many auctions they want us to display in each page from our search database
    // const res = await fetch(`http://localhost:6001/search${query}`);

    // if (!res.ok) {
    //     throw new Error('Failed to fetch data');
    // }
    // use .json function to get the body of the response out of the response message
    // return res.json();

    return await fetchWrapper.get(`search/${query}`)

    
}

export async function updateAuctionTest() {
    const data = {
        mileage: Math.floor(Math.random() * 100000) + 1
    }

    // const token = await getTokenWorkaround();

    // const res = await fetch('http://localhost:6001/auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', {
    //     method: 'PUT',
    //     headers: {
    //         'Content-type': 'application/json',
    //         'Authorization': 'Bearer ' + token?.access_token
    //     },
    //     body: JSON.stringify(data)
    // })

    // if (!res.ok) return {status: res.status, message: res.statusText}

    // return res.statusText;
    return await fetchWrapper.put('auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', data);
}

export async function createAuction(data: FieldValues) {
    return await fetchWrapper.post('auctions', data);
}

export async function getDetailedViewData(id: string): Promise<Auction> {
    return await fetchWrapper.get(`auctions/${id}`);
}