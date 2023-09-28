/** @type {import('next').NextConfig} */
const nextConfig = {
    experimental:{
        logging:{
            level: 'verbose',
        }
    },

    images: {
        domains: [
            'cdn.pixabay.com'
        ]
    }
}

module.exports = nextConfig
