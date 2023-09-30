// {/* inform Next.js that this is a client side component, not server side*/}
// 'use client'
import React from 'react'
import Search from './Search';
import Logo from './Logo';
import LoginButton from './LoginButton';

export default function Navbar() {
  return (
    // sticky, top-0, etc. are valid Tailwind CSS classes
    // we want our header to stick to the top of the screen, above all other contents, and our divs to be displayed in line and to spread across the top of our browser window
    // white background, padding all around, center things horizontally, and text formatting
    <header className='sticky top-0 z-50 flex justify-between bg-white p-5 items-center text-gray-800 shadow-md'>
        {/* we want to be able to click on the name Carsies Auctions and reset our page. Now, in order to use the reset function, that means using our params store because that is a react hook that needs to be done inside a client component. I don't want to change the nav bar from a server component to a client component, so I'm going to create a new component, called Logo.tsx, just for this which will allow us to reset our state./*}
        {/*<div className='flex items-center gap-2 text-3xl font-semibold text-red-500'>
          <AiOutlineCar size={34}/>
          Carsies Auctions
        </div>*/}
      <Logo/>
      <Search/>
      <LoginButton/>
    </header>
  )
}
