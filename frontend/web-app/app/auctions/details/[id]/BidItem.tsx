import { numberWithCommas } from '@/app/lib/numberWithComma';
import { Bid } from '@/types'
import { format } from 'date-fns';
import React from 'react'

type Props = {
    bid: Bid
}

function capitalizeFirstLetter(input: string) {
    return input.charAt(0).toUpperCase() + input.slice(1);
}

export default function BidItem({ bid }: Props) {
    // display in different colors, etc, depending on the bid status that has come back
    function getBidInfo() {
        let bgColor = '';
        let text = '';
        switch (bid.bidStatus) {
            case 'Accepted':
                bgColor = 'bg-green-200'
                text = 'Bid Accepted'
                break;
            case 'AcceptedBelowReserve':
                bgColor = 'bg-amber-500'
                text = 'Reserve Not Met'
                break;
            case 'TooLow':
                bgColor = 'bg-red-200'
                text = 'Bid Was Too Low'
                break
            default:
                bgColor = 'bg-red-200'
                text = 'Bid Placed After Auction Finished'
                break;
        }
        return {bgColor, text}
    }

    return (
        <div className={`
            border-gray-300 border-2 px-3 py-2 rounded-lg
            flex justify-between items-center mb-2
            ${getBidInfo().bgColor}
        `}>
            <div className='flex flex-col'>
                <span>Bidder: {capitalizeFirstLetter(bid.bidder)}</span>
                <span className='text-gray-700 text-sm'>
                    Time: {format(new Date(bid.bidTime), 'dd MMM yyyy h:mm a')}
                </span>
            </div>
            <div className='flex flex-col text-right'>
                <div className='text-xl font-semibold'>${numberWithCommas(bid.amount)}</div>
                <div className='flex flex-row items-center'>
                    <span>{getBidInfo().text}</span>
                </div>
            </div>
        </div>
    )
}