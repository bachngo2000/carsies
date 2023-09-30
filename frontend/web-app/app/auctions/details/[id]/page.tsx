import React from 'react'

// id of the specific car/auction
export default function Details({params}: {params: {id: string}}) {
  return (
    <div>Details for {params.id}</div>
  )
}
