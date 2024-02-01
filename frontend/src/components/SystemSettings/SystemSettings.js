import { ButtonGroup, Button, Stack, Box } from '@mui/material';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';

export default function SystemSettings() {
    const navigate = useNavigate();
    const location = useLocation();

    const isUserManagementActive = location?.pathname.includes('/system-settings/user-management');
    const isDistrictManagementActive = location?.pathname.includes('/system-settings/district-management');
    const isSubscriptionManagementActive = location?.pathname.includes('/system-settings/subscription-management');

    return (
        <Stack spacing={3}>
            <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}><Box sx={{ color: '#0180ff', fontWeight: '600' }}>System Settings</Box></Box>
            <ButtonGroup variant="contained" aria-label="outlined primary button group" sx={{ boxShadow: 'none', borderRadius: 'none', justifyContent: 'center' }}>
                <Button 
                    variant={isUserManagementActive ? "contained" : "outlined"} 
                    sx={{ borderColor: '#0180ff !important' }}
                    color="secondary" 
                    onClick={() => navigate('/system-settings/user-management')}
                >
                    User Management
                </Button>
            </ButtonGroup>
            <Outlet />
        </Stack>
    )
}