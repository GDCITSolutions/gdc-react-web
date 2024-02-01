import { useState } from 'react';
import { styled } from '@mui/material/styles';
import MuiAppBar from '@mui/material/AppBar';
import { Box, Toolbar, Typography, IconButton } from '@mui/material';
import { Notifications, Launch, AccountCircle } from '../Icons';
import { useLogoutMutation } from '../Login/authenticationSlice';
import { useNavigate } from 'react-router-dom';
import NotificationsModal from './NotificationsModal';
import ManageProfileDialog from '../ManageProfile';

const drawerWidth = 275;
const appBarHeight = 65;

const AppBar = styled(MuiAppBar, {
    shouldForwardProp: (prop) => prop !== 'open',
})(({ theme, open }) => ({
    backgroundColor: '#444444',
    height: appBarHeight,
    ...(!open && {
        paddingLeft: theme.spacing(8)
    }),
    ...(open && {
        marginLeft: drawerWidth,
        width: `calc(100% - ${drawerWidth}px)`,
    }),
}));

export default function Topbar({ open, user, role }) {
    const [isNotificationsOpen, setIsNotificationsOpen] = useState(false);
    const [isManageProfileOpen, setIsManageProfileOpen] = useState(false);
    const [logout] = useLogoutMutation();
    const navigate = useNavigate();

    return (
        <>
            <AppBar position="fixed" open={open}>
                <Toolbar>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column'
                        }}
                    >
                        <Typography
                            variant="h6"
                            component="div"
                        >
                            {`Welcome ${user.firstName} ${user.lastName}`}
                        </Typography>
                        <Typography
                            variant="string"
                            component="div"
                        >
                            {`Role(s): ${role}`}
                        </Typography>
                    </Box>
                    <Box sx={{
                        ml: 'auto',
                        display: 'flex'
                    }}>
                        <IconButton
                            sx={{
                                color: '#ffffff',
                                p: 0,
                                mr: 2
                            }}
                            onClick={() => setIsNotificationsOpen(true)}
                        >
                            <Notifications />
                        </IconButton>
                        <IconButton
                            sx={{
                                color: '#ffffff',
                                p: 0,
                                mr: 2
                            }}
                            onClick={() => setIsManageProfileOpen(true)}
                        >
                            <AccountCircle />
                        </IconButton>
                        <IconButton
                            sx={{
                                color: '#ffffff',
                                p: 0
                            }}
                            onClick={() => {
                                logout();
                                navigate('/login');
                            }}
                        >
                            <Launch />
                        </IconButton>
                    </Box>
                </Toolbar>
            </AppBar>
            <NotificationsModal open={isNotificationsOpen} onClose={() => setIsNotificationsOpen(false)} />
            <ManageProfileDialog open={isManageProfileOpen} onClose={() => setIsManageProfileOpen(false)} user={user} />
        </>
    );
}