'use client'
import { useParamsStore } from '@/hooks/useParamsStore';
import { usePathname, useRouter } from 'next/navigation';
import React from 'react'
import {AiOutlineCar} from 'react-icons/ai';

export default function Logo() {

  // get reset function from useParamsStore
  const reset = useParamsStore(state => state.reset);

  const router = useRouter();
  const pathname = usePathname();
  function doReset() {
    if (pathname !== '/') {
      router.push('/');
      reset();
    }
  }

  return (
    <div onClick={doReset} className='cursor-pointer flex items-center gap-2 text-3xl font-semibold text-red-500'>
        {/* added icon from the react icons library*/}
        <AiOutlineCar size={34}/>
          CarAuctions
    </div>
  )
}

