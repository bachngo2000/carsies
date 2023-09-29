import { useParamsStore } from '@/hooks/useParamsStore';
import { Button } from 'flowbite-react';
import React from 'react'

// we don't need to use props anymore since we're getting them directly from our useParamsStore
// type Props = {
//     pageSize: number;
//     setPageSize: (size: number) => void;
// }

// options for number of auctions display on a page
const pageSizeButtons = [4, 8, 12];

export default function Filters(/*{pageSize, setPageSize}: Props*/) {

  const pageSize = useParamsStore(state => state.pageSize);
  const setParams = useParamsStore(state => state.setParams);

  return (
    <div className='flex justify-between items-center mb-4'>
        <div>
            <span className='uppercase text-sm text-gray-500 mr-2'>Page size</span>
            <Button.Group>
                {/* loop over our pageSizeButtons array, for each value, get hold of its index */}
                {pageSizeButtons.map((value, i) => (
                    // add a conditional logic, if the pageSize == value, the button color is red, or gray otherwise
                    <Button key={i} onClick={() => setParams({pageSize: value})} color={`${pageSize == value ? 'red' : 'gray'}`} className='focus:ring-0'>
                        {value}
                    </Button>
                ))}
            </Button.Group>
        </div>
        
    </div>
  )
}
