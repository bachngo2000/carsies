import { Auction, PagedResult } from "@/types"
import { create } from "zustand"

// Since we're storing our actions in local state, that's not helping us when it comes to updating these from another part of our application
// when it comes to the SignalR functionality we're gonna add, so we're creating a store (useAuctionStore.ts) to store our auctions

// state to store our auctions
type State = {
    auctions: Auction[]
    totalCount: number
    pageCount: number
}

type Actions = {
    // takes PagedResult<Auction>) as a parameter and returns void
    setData: (data: PagedResult<Auction>) => void

    // update the price for any auctions inside here when we receive an accepted bid back from the signalR service
    setCurrentPrice: (auctionId: string, amount: number) => void
}

// create an initial state of type of State with default values
const initialState: State = {
    auctions:[],
    pageCount: 0,
    totalCount: 0
}

export const useAuctionStore = create<State & Actions>((set) => ({
    // pass the initial state
    ...initialState,

    setData: (data: PagedResult<Auction>) => {
        set(() => ({
            auctions: data.results,
            totalCount: data.totalCount,
            pageCount: data.pageCount
        }))
    },

    setCurrentPrice: (auctionId: string, amount: number) => {
        // we need access to our current state, so we pass in a state as our parameter
        set((state) => ({
            // update the auction inside our state whose auctionId and amount has been passed to setCurrentPrice as params by setting the currentHighBid to amount
            auctions: state.auctions.map((auction) => auction.id === auctionId 
                ? {...auction, currentHighBid: amount} : auction)
        }))
    }
}))