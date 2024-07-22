import React from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faFacebook, faGit, faGithub, faGoogle } from '@fortawesome/free-brands-svg-icons'
import { faBarcode } from '@fortawesome/free-solid-svg-icons'
export default function ThirdAuth() {
    return (
        <div className='grid grid-cols-2 gap-6'>
            <button className='inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border border-input bg-background hover:bg-accent hover:text-accent-foreground h-10 px-4 py-2'>
                <FontAwesomeIcon className='mr-2 h-4 w-4' icon={faGoogle} />
                <span>Google</span>
            </button>
            <button className='inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border border-input bg-background hover:bg-accent hover:text-accent-foreground h-10 px-4 py-2'>
                <FontAwesomeIcon className='mr-2 h-4 w-4' icon={faGithub} />
                <span>Github</span>
            </button>
            <button className='inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border border-input bg-background hover:bg-accent hover:text-accent-foreground h-10 px-4 py-2'>
                <FontAwesomeIcon className='mr-2 h-4 w-4' icon={faFacebook} />
                <span>Facebook</span>
            </button>
            <button className='inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border border-input bg-background hover:bg-accent hover:text-accent-foreground h-10 px-4 py-2'>
                <FontAwesomeIcon className='mr-2 h-4 w-4' icon={faBarcode} />
                <span>Barcode</span>
            </button>
        </div>
    )
}
