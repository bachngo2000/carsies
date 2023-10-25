import React from 'react'

type Props = {
    // optional because there might be no bids
    amount?: number
    reservePrice: number
}

// add props as our params
export default function CurrentBid({ amount, reservePrice }: Props) {
    // variables to store what we want to display in the auction cards for bids information
    const text = amount ? '$' + amount : 'No bids';
    const color = amount ? amount > reservePrice ? 'bg-blue-600' : 'bg-amber-600' : 'bg-red-600'

    return (
        <div className={`
            border-2 border-white text-white py-1 px-2 rounded-lg flex
            justify-center ${color}
        `}>
            {text}
        </div>
    )
}