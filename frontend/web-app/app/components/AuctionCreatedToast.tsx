import { Auction } from '@/types'
import Image from 'next/image'
import Link from 'next/link'
import React from 'react'

type Props = {
    auction: Auction
}

// when a new auction is created, we'll pop up a toast on the user's screen to notify them that there is a new auction they can take a look at
export default function AuctionCreatedToast({auction}: Props) {
  return (
    // clicking on the link allows the user to look at the new auction in details
    <Link href={`/auctions/details/${auction.id}`} className='flex flex-col items-center'>
        <div className='flex flex-row items-center gap-2'>
            <Image
                src={auction.imageUrl}
                alt='image'
                height={80}
                width={80}
                className='rounded-lg w-auto h-auto'
            />
            <span> New Auction! {auction.make} {auction.model} has been added</span>
        </div>
    </Link>
  )
}