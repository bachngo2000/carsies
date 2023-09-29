// {/* inform Next.js that this is a client side component, not server side*/}
// 'use client'
import {AiOutlineCar} from 'react-icons/ai';

import React from 'react'
import Search from './Search';

export default function Navbar() {
  return (
    // sticky, top-0, etc. are valid Tailwind CSS classes
    // we want our header to stick to the top of the screen, above all other contents, and our divs to be displayed in line and to spread across the top of our browser window
    // white background, padding all around, center things horizontally, and text formatting
    <header className='sticky top-0 z-50 flex justify-between bg-white p-5 items-center text-gray-800 shadow-md'>
      <div>
        <div className='flex items-center gap-2 text-3xl font-semibold text-red-500'>
          {/* added icon from the react icons library*/}
          <AiOutlineCar size={34}/>
          Carsies Auctions
        </div>
      </div>
      <Search/>
      <div> Login </div>
    </header>
  )
}
