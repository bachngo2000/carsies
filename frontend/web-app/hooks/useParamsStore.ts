// instead of using local states inside each component to store and set the state, then we can use Zustand to manage states
// where to store our state for our params that we need for our query strings

import { create } from "zustand"

// specify the type of states that we're using
type State = {
    pageNumber: number
    pageSize: number
    pageCount: number
    searchTerm: string
    searchValue: string
    orderBy: string
    filterBy: string
    seller?: string
    winner?: string
}

// specify the type of actions that we're gonna support inside this store
type Actions = {
    // method to set params, and this is just the type of thing we're creating. So this is going to be called params and we're going to use it to update the state that we're storing inside here. But we want the ability just to update one thing at a time. And the way that we can make all of these properties optional without actually making them optional is to say that this params is going to be of partial
    setParams: (params: Partial<State>) => void
    reset: () => void
    setSearchValue: (value: string) => void
}

// initial state for our store with initial values
const intialState: State = {
    pageNumber: 1,
    pageSize: 12,
    pageCount: 1,
    searchTerm: '',
    searchValue: '',
    orderBy: 'make',
    filterBy: 'live',
    seller: undefined,
    winner: undefined
}

// create our state store
// types we have in our store
export const useParamsStore = create<State & Actions>()((set) => ({
    // define our states and methods
    // define everything that's inside our state and actions
    // give it an initial state
    ...intialState,

    // define the methods that we're supporting inside here
    // the params here are what we're using to set our new state(s)
    setParams: (newParams: Partial<State>) => {
        // get access to our existing state inside our store
        set((state) => {
            // check to see if our new params is a page number
            // and if we are updating the page, we want to keep our existing states and purely update the page number because that means the user is just switching from one page to another page and we're going to return from this. The existing state. So we're going to use the curly brackets, the spread operator and say states, and then we're going to update the page number inside that state and we're going to set it to new params dot page number.
            if (newParams.pageNumber) {
                return {...state, pageNumber: newParams.pageNumber}
            } 
            // If we're not changing the page, that means we're changing something else. So we might be changing the page size.
            else {
                return {...state, ...newParams, pageNumber: 1}
            }
        })
    },

    // this method allows us to update our States (pageNumber, pageSize, ...) inside our store
    reset: () => set(intialState),

    setSearchValue: (value: string) => {
        set({searchValue: value})
    }
}))
