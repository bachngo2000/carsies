import React from 'react'
import CountdownTimer from './CountdownTimer'
import CarImage from './CarImage'
import { Auction } from '@/types'

// specify what we're passing to the AuctionCard component
// specify auction as type Auction
type Props = {
    auction: Auction
}

// represents each auction we have
// receive the auction inside this componet that we're getting from the Listings component
// So for each one of these auction cards, then we're going to pass down an auction to the card so that we can use its properties to display the information inside this auction card.
// Now, in order to pass down properties from our listings to our cards, then we need to use Props.
// And then we take our Props and we can pass them down to our auction card and we can do it like this and say that the props we're receiving is a type of props.
export default function AuctionCard({auction}: Props) {
  return (
    //  we know inside our props objects we have an auction
    //  make each card clickable.
    // add className to our <a></a> tag as "group". And we can apply styling for this group inside another component so that when we hover over anything (images) inside this tag, that styling for this group will apply and we're going to use it inside our car image.
    <a href='#' className='group'>
        <div className='w-full bg-gray-200 aspect-w-16 aspect-h-10 rounded-lg overflow-hidden'>
            <div>
                {/* we want to add react state b/c we need to know if the image has been loaded or not and do something when the image's been loaded. Now, when it comes to using React State or any react hooks that we use, these are client side functionality. So we can't use a use state hook inside a server component. And currently our auction card that's deriving from our listings or is a child component of our listings. These are all server side components right now. So to keep these server components, we can add smaller client components that are added to a server component and we'll do that for our image here.*/}
                {/* Therefore, we cut the Image here and create a separate client-side CarImage functional component that represents it so we can use React state.  */}
                {/* <Image
                    src={auction.imageUrl}
                    alt='image'
                    fill
                    priority
                    className='object-cover'
                    sizes='(max-width:768px) 100vw, (max-width: 1200px) 50vw, 25vw' />*/}
                <CarImage imageUrl={auction.imageUrl}/>
                <div className='absolute bottom-2 left-2'>
                    <CountdownTimer auctionEnd={auction.auctionEnd}/>
                </div>
                
            </div>
        </div>
        <div className='flex justify-between items-center mt-4'>
            <h3 className='text-gray-700'>
                {auction.make} {auction.model}
            </h3>
            <p className='font-semibold text-sm'>{auction.year}</p>
        </div>
    </a>
  )
}
