import { useParamsStore } from '@/hooks/useParamsStore';
import { Button } from 'flowbite-react';
import React from 'react'
import { AiOutlineClockCircle, AiOutlineSortAscending } from 'react-icons/ai';
import { BsFillStopCircleFill, BsStopwatchFill} from 'react-icons/bs';
import { GiFinishLine, GiFlame } from 'react-icons/gi';

// we don't need to use props anymore since we're getting them directly from our useParamsStore
// type Props = {
//     pageSize: number;
//     setPageSize: (size: number) => void;
// }

// options for number of auctions display on a page
const pageSizeButtons = [4, 8, 12];

const orderButtons = [
    {
        label: 'Alphabetical',
        icon: AiOutlineSortAscending,
        value: 'make'
    },

    {
        label: 'End date',
        icon: AiOutlineClockCircle,
        value: 'endingSoon'
    },

    {
        label: 'Recently added',
        icon: BsFillStopCircleFill,
        value: 'new'
    }
]

const filterButtons = [
    {
        label: 'Live Auctions',
        icon: GiFlame,
        value: 'live'
    },

    {
        label: 'Ending < 6 hours',
        icon: GiFinishLine,
        value: 'endingSoon'
    },

    {
        label: 'Completed',
        icon: BsStopwatchFill,
        value: 'finished'
    }
]

export default function Filters(/*{pageSize, setPageSize}: Props*/) {

  const pageSize = useParamsStore(state => state.pageSize);
  const setParams = useParamsStore(state => state.setParams);
  const orderBy = useParamsStore(state => state.orderBy);
  const filterBy = useParamsStore(state => state.filterBy);

  return (
    <div className='flex justify-between items-center mb-4'>
    
        <div>
            <span className='uppercase text-sm text-gray-500 mr-2'>Filter by</span>
            <Button.Group>
                {filterButtons.map(({label, icon: Icon, value}) => (
                    <Button key={value} onClick={() => setParams({filterBy: value})} color={`${filterBy == value ? 'red' : 'gray'}`}>
                        <Icon className='mr-3 h-4 w-4'/>
                        {label}
                    </Button>
                ))}
            </Button.Group>
        </div>

        <div>
            <span className='uppercase text-sm text-gray-500 mr-2'>Order by</span>
            <Button.Group>
                {orderButtons.map(({label, icon: Icon, value}) => (
                    <Button key={value} onClick={() => setParams({orderBy: value})} color={`${orderBy == value ? 'red' : 'gray'}`}>
                        <Icon className='mr-3 h-4 w-4'/>
                        {label}
                    </Button>
                ))}
            </Button.Group>
        </div>

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
