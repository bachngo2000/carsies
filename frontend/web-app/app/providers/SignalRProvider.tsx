'use client'
import { useAuctionStore } from '@/hooks/useAuctionStore';
import { useBidStore } from '@/hooks/useBidStore';
import { Auction, Bid } from '@/types';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { User } from 'next-auth';
import { ReactNode, useEffect, useState } from 'react'
import AuctionCreatedToast from '../components/AuctionCreatedToast';
import toast from 'react-hot-toast';

type Props = {
    children: ReactNode
    user: User | null
}

export default function SignalRProvider({ children, user }: Props) {
    // store our connection inside local state
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const setCurrentPrice = useAuctionStore(state => state.setCurrentPrice);
    const addBid = useBidStore(state => state.addBid);

    // connect to our signalR Hub when the user loads our application
    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:6001/notifications')
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        // check if we have a connection
        if (connection) {
            connection.start()
                .then(() => {
                    console.log('Connected to notification hub');

                    connection.on('BidPlaced', (bid: Bid) => {
                        if (bid.bidStatus.includes('Accepted')) {
                            setCurrentPrice(bid.auctionId, bid.amount);
                        }
                        addBid(bid);
                    });

                    connection.on('AuctionCreated', (auction: Auction) => {
                        if (user?.username !== auction.seller) {
                            return toast(<AuctionCreatedToast auction={auction} />, 
                                {duration: 10000})
                        }
                    });

                }).catch(error => console.log(error));
        }

        return () => {
            connection?.stop();
        }
    }, [connection, setCurrentPrice])

    return (
        children
    )
}