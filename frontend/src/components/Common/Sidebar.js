import { useState } from 'react';
import { styled } from '@mui/material/styles';
import MuiDrawer from '@mui/material/Drawer';
import { List, Typography, IconButton, ListItem, ListItemButton, ListItemText } from '@mui/material';
import { Menu, Settings, Dashboard } from '../Icons';
import { useNavigate } from 'react-router-dom';

const drawerWidth = 275;
const appBarHeight = 65;

const menuOptions = [
    { name: 'Home', roles: [1, 2], icon: <Dashboard/>, path: '/home' },
    { name: 'System Settings', roles: [1, 2], icon: <Settings />, path: '/system-settings/user-management' }
];

const openedMixin = () => ({
    width: drawerWidth,
});

const closedMixin = (theme) => ({
    width: `calc(${theme.spacing(7)} + 1px)`,
    [theme.breakpoints.up('sm')]: {
        width: `calc(${theme.spacing(8)} + 1px)`,
    },
});

const DrawerHeader = styled('div')(() => ({
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#000000',
    color: '#ffffff',
    height: appBarHeight,
    padding: '0 20px',
}));

const Drawer = styled(MuiDrawer, {
    shouldForwardProp: (prop) => prop !== 'open',
})(({ theme, open }) => ({
    display: 'flex',
    flexDirection: 'colum',
    width: drawerWidth,
    flexShrink: 0,
    whiteSpace: 'nowrap',
    boxSizing: 'border-box',
    marginBottom: theme.spacing(5),
    backgroundColor: '#e6e6e6',
    ...(open && {
        ...openedMixin(theme),
        '& .MuiDrawer-paper': openedMixin(theme),
    }),
    ...(!open && {
        ...closedMixin(theme),
        '& .MuiDrawer-paper': closedMixin(theme),
    }),
}));


export default function Sidebar({ open, toggleOpen, userRoles }) {
    const navigate = useNavigate();
    const [selected, setSelected] = useState(0);

    return (
        <Drawer
            variant="permanent"
            open={open}
        >
            <DrawerHeader>
                {open &&
                    <Typography
                        variant='h6'
                        component='div'
                        sx={{
                            mr: 'auto'
                        }}>
                        Sample App
                    </Typography>
                }
                <IconButton
                    onClick={() => toggleOpen()}
                    sx={{
                        color: '#ffffff',
                        p: 0
                    }}
                >
                    <Menu />
                </IconButton>
            </DrawerHeader>
            <List
                sx={{
                    py: 0
                }}
            >
                {menuOptions.map((obj, index) => {
                    if (!obj.roles.some(x => userRoles?.some(y => y.roleId === x))) return null;
                    let isSelected = selected === index;

                    return (
                        <ListItem
                            key={index}
                            disablePadding
                            sx={{
                                display: 'block',
                            }}
                        >
                            <ListItemButton
                                selected={isSelected}
                                sx={{
                                    minHeight: 48,
                                    justifyContent: open ? 'initial' : 'center',
                                    px: 2.5,
                                    '&.Mui-selected': {
                                        backgroundColor: '#0180FF',
                                        color: '#ffffff',
                                        fontColor: '#ffffff',
                                        ':hover': {
                                            backgroundColor: '#0180FF',
                                            color: '#ffffff',
                                            fontColor: '#ffffff'
                                        }
                                    },
                                    '&.Mui-focusVisible': {
                                        backgroundColor: '#0180FF',
                                        color: '#ffffff',
                                        fontColor: '#ffffff'
                                    },
                                    ':hover': {
                                        backgroundColor: '#0180FF',
                                        color: '#ffffff',
                                        fontColor: '#ffffff'
                                    }
                                }}
                                onClick={() => {
                                    setSelected(index);
                                    navigate(obj.path);
                                }}
                                title={obj.name}
                            >
                                {open && <ListItemText primary={obj.name} />}
                                {obj.icon}
                            </ListItemButton>
                        </ListItem>);
                })}
            </List>
        </Drawer>
    );
}