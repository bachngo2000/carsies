'use client'
import { Button } from 'flowbite-react'
import { signIn } from 'next-auth/react'
import React from 'react'

export default function LoginButton() {
  return (
    // use the signIn function from next-auth b/c this gives us our sign in functionality
    <Button outline onClick={() => signIn('id-server', {callbackUrl: '/'}, {prompt: 'login'})}>
        Login
    </Button>
  )
}
