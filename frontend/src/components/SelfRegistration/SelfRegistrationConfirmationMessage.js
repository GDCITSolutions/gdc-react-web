import { Box, Card, CardContent, Stack, Divider, Typography } from '@mui/material';

export default function SelfRegistrationConfirmationMessage() {
    return (
        <Box
            component='div'
            sx={{
                display: 'flex',
                height: '100vh',
                backgroundColor: '#e7e7e7'
            }}
        >
            <Card
                variant='outlined'
                sx={{
                    margin: 'auto',
                    padding: '1em',
                    borderRadius: '1em',
                    border: '1px solid #cccccc',
                    textAlign: 'center',
                    maxWidth: 455
                }}
            >
                <CardContent>
                    <Stack
                        spacing={2}
                    >
                        <span className='material-symbols-outlined' style={{ fontVariationSettings: '"FILL" 1' }}>dashboard</span>
                        <Divider
                            sx={{ margin: '1em 0 !important' }}
                        />
                        <Typography
                            paragraph
                        >
                            Thank you for self-registering to create an account. Please verify your account by clicking the link in the account setup email.
                        </Typography>
                    </Stack>
                </CardContent>
            </Card>
        </Box>
    )
}