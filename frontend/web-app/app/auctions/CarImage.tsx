'use client'
import React, { useState } from 'react'
import Image from 'next/image'

// we need to get hold of our imageUrl and we're gonna pass it down as props
type Props = {
    imageUrl: string;
}
export default function CarImage({imageUrl} : Props) {
    // add react state to know when the image is loading
    const [isLoading, setLoading] = useState(true);
  return (
    <Image
        src={imageUrl}
        alt='image'
        fill
        priority
        // use conditional logic to add some styles for when the image is loading and when it's not loading respectively
        className={`object-cover group-hover:opacity-75 duration-700 ease-in-out
            ${isLoading ? 'grayscale blur-2xl scale-110' : 'grayscale-0 blur-0 scale-100'}
        `}
        sizes='(max-width:768px) 100vw, (max-width: 1200px) 50vw, 25vw'
        // set isLoading to false when loading is complete
        onLoadingComplete={() => setLoading(false)}
    />
  )
}
