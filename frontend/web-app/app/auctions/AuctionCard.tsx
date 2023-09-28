import React from 'react'

// specify what we're passing to the AuctionCard component
// specify "any" as a temporary type for an auction
type Props = {
    auction: any
}

// represents each auction we have
// receive the auction inside this componet that we're getting from the Listings component
// So for each one of these auction cards, then we're going to pass down an auction to the card so that we can use its properties to display the information inside this auction card.
// Now, in order to pass down properties from our listings to our cards, then we need to use Props.
// And then we take our Props and we can pass them down to our auction card and we can do it like this and say that the props we're receiving is a type of props.
export default function AuctionCard({auction}: Props) {
  return (
    //  we know inside our props objects we have an auction and we know that our auctions have a make.
    <div>{auction.make}</div>
  )
}
