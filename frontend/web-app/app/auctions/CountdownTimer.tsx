// we need to tell Nextjs that this is a client-side component. The issue this fixes here is that our React countdown is using some React client side code and this cannot be rendered on our server and then returned as HTML. This is client side functionality. Our browser needs to do something with this JavaScript that it's trying to work with, but effectively it can't do anything like this. What we need to do for this situation is we need to go to our countdown timer and we need to use the use client because that's where this functionality is going to need to take place on the client browser.
'use client'
import { useBidStore } from '@/hooks/useBidStore';
import { usePathname } from 'next/navigation';
import React from 'react'
import Countdown, { zeroPad } from 'react-countdown';

// specify auctionEnd as a string type b/c that's how it's returned from our backend API
type Props = {
    auctionEnd: string;
}

// Renderer callback with condition
// specify types for input data
const renderer = ({ days, hours, minutes, seconds, completed }:{days: number, hours: number, minutes: number, seconds: number, completed: boolean}) => {

    return (
        // these styles in clasname (border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center) apply to our div regardless of the conditional logics
        // use conditional logic inside our classname with back ticks
        // specify a conditional (completed) of how this is going to be displayed depending on whether or not this is complete
        // if it has completed, we give it a red background.  For the alternative condition, we do another check (days == 0 && hours < 10), and if that is the case
        // we give it a background color of amber.  If that's not the case, we give it a background color of green
        <div className={`
            border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center
            ${completed 
                ? 'bg-red-600' : (days == 0 && hours < 10) ? 'bg-amber-600' : 'bg-green-600'}
        `}>
            {/* check if the auction has been completed.  If it is, then we specify it as finished.  Otherwise, we will display its countdown/remaining days, hours, minutes, and seconds  */}
            {completed ? (
                <span> Auction finished </span>
            ) : (
                <span suppressHydrationWarning={true}>
                    {zeroPad(days)}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
                </span>
            )}

        </div>
    )
    
};

// add our props 
export default function CountdownTimer({auctionEnd}: Props) {

    const setOpen = useBidStore(state => state.setOpen);

    const pathname = usePathname();

    function auctionFinished() {
        if (pathname.startsWith('/auctions/details')) {
          setOpen(false)
        }
    }

  return (
    <div>
        {/* pass into the Countdown component from the react countdown library the date that it's counting down from and it's gonna be from our auction's EndDate  */}
        <Countdown date={auctionEnd} renderer={renderer} onComplete={auctionFinished}/>
    </div>
  )
}
