// specify type for each property of PagedResult<T> (results)
export type PagedResult<T> = {
    results: T[]
    pageCount: number
    totalCount: number
}

// specify type for each property of an auction
export type Auction = {
    reservePrice: number
    seller: string
    winner?: string
    soldAmount: number
    currentHighBid: number
    createdAt: string
    updatedAt: string
    auctionEnd: string
    status: string
    make: string
    model: string
    year: number
    color: string
    mileage: number
    imageUrl: string
    id: string
}