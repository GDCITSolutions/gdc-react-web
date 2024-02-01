import React, { useState } from 'react';
import { Outlet } from 'react-router-dom';
import { Box } from '@mui/material';
import { useGetSessionQuery } from '../../app/slices/sessionSlice';
import { Footer, Sidebar, Topbar } from '../Common';

export default function Home() {
    const [open, setOpen] = useState(false);
    const { data: session = {} } = useGetSessionQuery();

    return (
        <Box sx={{
            display: 'flex'
        }}>
            <Topbar open={open} user={session} role={session.roles?.map(x => x.role.value).join(', ') ?? ''} />
            <Sidebar open={open} toggleOpen={() => setOpen(!open)} userRoles={session.roles} />
            <Box
                sx={{
                    flexGrow: 1,
                    pt: 12,
                    px: 3,
                    pb: 9,
                    backgroundColor: '#ffffff',
                    width: open ? '70%' : '75%'
                }}
            >
                <Outlet />
            </Box>
            <Box
                component='div'
                sx={{
                    position: 'absolute',
                    left: '0',
                    bottom: '0',
                    width: '100%',
                    backgroundColor: '#000000',
                    color: '#ffffff',
                    zIndex: 1200
                }}
            >
                <Footer />
            </Box>
        </Box>
    );
}
