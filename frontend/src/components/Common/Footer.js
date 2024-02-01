import React from 'react';
import { Box, Typography, Link } from '@mui/material';

export default function Footer() {
    return (
        <Box
            component='div'
            sx={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                padding: '1em'
            }}
        >
            <Typography component='span'>
                <span className='material-symbols-outlined' style={{ fontVariationSettings: '"FILL" 1' }}>dashboard</span>
            </Typography>
            <Box>
                <Link href='/accessibility' underline='always' sx={{ color: '#ffffff', textDecorationColor: '#ffffff', mr: 2 }}>Accessibility</Link>
                <Link href='/privacy-policy' underline='always' sx={{ color: '#ffffff', textDecorationColor: '#ffffff', mr: 2 }}>Privacy Policy</Link>
                <Link href='/terms-of-service' underline='always' sx={{ color: '#ffffff', textDecorationColor: '#ffffff' }}>Terms of Service</Link>
            </Box>
        </Box>
    )
}
