/** @type {import('next').NextConfig} */
const nextConfig = {
    experimental:{
        logging:{
            level: 'verbose',
        },
        serverActions: true
    },

    images: {
        domains: [
            'cdn.pixabay.com'
        ]
    }
}

module.exports = nextConfig
