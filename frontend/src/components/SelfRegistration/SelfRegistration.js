import { useState } from 'react';
import { Box, Card, CardContent, TextField, Stack, Button, InputAdornment, IconButton, Link, Divider, Select, MenuItem, FormControl, FormHelperText } from '@mui/material';
import { Visibility, VisibilityOff } from '../Icons';
import styled from 'styled-components';
import { useSelfRegisterMutation } from './selfRegistrationSlice';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

const CreateAccountButton = styled(Button)`
&:hover {
    background-color: #026AD3 !important;
}`;

const accountTypes = [
    { value: 1, label: 'District Admin' },
    { value: 2, label: 'School Admin' },
    { value: 3, label: 'Faculty/Staff' }
];

export default function SelfRegistration() {
    const [showConfirmationEmail, setShowConfirmationEmail] = useState(true);
    const [form, setForm] = useState({
        email: '',
        emailError: false,
        confirmationEmail: '',
        confirmationEmailError: false,
        accountType: '',
        accountTypeError: false
    });
    const [selfRegister] = useSelfRegisterMutation();
    const navigate = useNavigate();

    function validateAndCreateAccount() {
        let emailError, confirmationEmailError, accountTypeError = false;

        if (!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(form.email))) emailError = true;
        if (form.email.toLowerCase() !== form.confirmationEmail.toLowerCase() || form.confirmationEmail === '') confirmationEmailError = true;
        if (form.accountType === '') accountTypeError = true;

        if (emailError || confirmationEmailError || accountTypeError) {
            setForm({ ...form, emailError, confirmationEmailError, accountTypeError });
            return;
        }

        selfRegister({ roleId: form.accountType, emailAddress: form.email })
            .then((response) => {
                if (response.error) toast.error('Error occurred while self registering. Please contact an administrator.');
                else navigate('/self-registration-confirmation');
            })
            .catch((e) => console.log(e));
    }

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
                        <TextField
                            placeholder='Email Address'
                            variant='outlined'
                            size='small'
                            type='email'
                            value={form.email}
                            onChange={(e) => setForm({ ...form, email: e.currentTarget.value, emailError: false })}
                            error={form.emailError}
                            helperText={form.emailError ? 'Please enter an valid email address' : null}
                        />
                        <TextField
                            placeholder='Confirm Email Address'
                            variant='outlined'
                            size='small'
                            value={form.confirmationEmail}
                            onChange={(e) => setForm({ ...form, confirmationEmail: e.currentTarget.value, confirmationEmailError: false })}
                            error={form.confirmationEmailError}
                            helperText={form.confirmationEmailError ? 'Please make sure this email address matches the one above' : null}
                            type={showConfirmationEmail ? 'email' : 'password'}
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position='end'>
                                        <IconButton onClick={() => setShowConfirmationEmail(!showConfirmationEmail)} edge='end' title={showConfirmationEmail ? 'Hide Confirmation Email' : 'Show Confirmation Email'}>
                                            {showConfirmationEmail ? <VisibilityOff /> : <Visibility />}
                                        </IconButton>
                                    </InputAdornment>
                                )
                            }}
                        />
                        <FormControl error={form.accountTypeError}>
                            <Select
                                sx={{
                                    color: form.accountType === '' ? '#707070' : 'inherit',
                                    opacity: '0.8',
                                    textAlign: 'left'
                                }}
                                size='small'
                                value={form.accountType}
                                onChange={(e) => setForm({ ...form, accountType: e.target.value, accountTypeError: false })}
                                displayEmpty
                                renderValue={(value) => value !== '' ? accountTypes.find(x => x.value === value).label : 'Select Account Type'}
                                error={form.accountTypeError}
                            >
                                {accountTypes.map((x, index) => (
                                    <MenuItem key={index} value={x.value}>{x.label}</MenuItem>
                                ))}
                            </Select>
                            {form.accountTypeError ? <FormHelperText>Please select an account type</FormHelperText> : null}
                        </FormControl>
                        <CreateAccountButton
                            variant='contained'
                            onClick={() => validateAndCreateAccount()}
                        >
                            Create Account
                        </CreateAccountButton>
                        <Link
                            href='/login'
                            color='secondary'
                        >
                            Return to Login
                        </Link>
                    </Stack>
                </CardContent>
            </Card>
        </Box>
    );
}