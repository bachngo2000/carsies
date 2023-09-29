import React from 'react'
import Image from 'next/image'

// we need to get hold of our imageUrl and we're gonna pass it down as props
type Props = {
    imageUrl: string;
}
export default function CarImage({imageUrl} : Props) {
  return (
    <Image
        src={imageUrl}
        alt='image'
        fill
        priority
        className='object-cover'
        sizes='(max-width:768px) 100vw, (max-width: 1200px) 50vw, 25vw'
    />
  )
}
