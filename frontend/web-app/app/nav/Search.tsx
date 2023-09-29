'use client'
import { useParamsStore } from '@/hooks/useParamsStore'
import React, { useState } from 'react'
import {FaSearch} from 'react-icons/fa'

export default function Search() {

  // get setParams function from the store
  const setParams = useParamsStore(state => state.setParams);

  // using local state bc we need to track the state of what is being typed into the input field
  const [value, setValue] = useState('');

  // function to track when the input is being typed into
  function onChange(event: any) {
    // event.target.value is the characters typed into the input field
    setValue(event.target.value);
  }

  // function to submit the change
  function search() {
    // when this function is called, it updates our parameters, and inside our Listings.tsx components, then the URL that the useEffect() function depends on, our useEffect() 
    // is going to be called whenever this URL changes. And because we're getting our params inside here from the useParamsStore, this is going to react to the searchTerm changing, which means our URL is going to change, which means the useEffect() is going to be called with that updated URL, which will then in turn of course make the request to our search service and get the updated results back.
    setParams({searchTerm: value});
  }

  return (
    <div className='flex w-[50%] items-center border-2 rounded-full py-2 shadow-sm'>
        <input
            onKeyDown={(e: any) => {
              if (e.key == 'Enter') {
                search()
              }
            }}
            onChange={onChange}
            type="text'"
            placeholder='Search for cars by make, model, or color'
            className='
              flex-grow
              pl-5
              bg-transparent
              focus:outline-none
              border-transparent
              focus:border-transparent
              focus: ring-0
              text-sm
              text-gray-600
            '
        />
        <button onClick={search}>
            <FaSearch size={34} className='bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2'></FaSearch>
        </button>
        
    </div>
  )
}
