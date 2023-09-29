import { Button } from 'flowbite-react';
import ButtonGroup from 'flowbite-react/lib/esm/components/Button/ButtonGroup';
import React from 'react'

type Props = {
    pageSize: number;
    setPageSize: (size: number) => void;
}

// options for number of auctions display on a page
const pageSizeButtons = [4, 8, 12];

export default function Filters({pageSize, setPageSize}: Props) {
  return (
    <div className='flex justify-between items-center mb-4'>
        <div>
            <span className='uppercase text-sm text-gray-500 mr-2'>Page size</span>
            <ButtonGroup>
                {/* loop over our pageSizeButtons array, for each value, get hold of its index */}
                {pageSizeButtons.map((value, i) => (
                    // add a conditional logic, if the pageSize == value, the button color is red, or gray otherwise
                    <Button key={i} onClick={() => setPageSize(value)} color={`${pageSize == value ? 'red' : 'gray'}`} className='focus:ring-0'>
                        {value}
                    </Button>
                ))}
            </ButtonGroup>
        </div>
        
    </div>
  )
}
