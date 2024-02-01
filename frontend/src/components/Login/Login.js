import React, { useEffect, useState } from 'react';
import { Box, Card, CardContent, TextField, Stack, Button, InputAdornment, IconButton, Checkbox, FormControlLabel, Link, Divider } from '@mui/material';
import { Visibility, VisibilityOff } from '../Icons';
import { useLoginMutation } from './authenticationSlice';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { setSession } from '../../utility';
import styled from 'styled-components';

const LoginButton = styled(Button)`
&:hover {
    background-color: #026AD3 !important;
},
`;

export default function Login() {
    const [form, setForm] = useState({
        schoolDistrict: '',
        schoolDistrictError: false,
        email: '',
        emailError: false,
        password: '',
        passwordError: false,
        rememberMe: false
    })
    const [showPassword, setShowPassword] = useState(false);
    const [login] = useLoginMutation();
    const navigate = useNavigate();

    useEffect(() => {
        var credsString = localStorage.getItem('sample_saved_credentials');

        if (credsString !== null) {
            var creds = JSON.parse(credsString);
            setForm({
                ...form,
                schoolDistrict: creds.schoolDistrict,
                rememberMe: creds.rememberMe
            });
        }
    }, []);

    function validateAndLogin() {
        let emailError, passwordError = false;

        if (form.email === '') emailError = true;
        if (form.password === '') passwordError = true;

        if (emailError || passwordError) {
            setForm({ ...form, emailError, passwordError });
            return;
        }

        if (form.rememberMe) {
            localStorage.setItem(
                'sample_saved_credentials',
                JSON.stringify({
                    schoolDistrict: form.schoolDistrict,
                    rememberMe: form.rememberMe
                }));
        } else {
            localStorage.removeItem('sample_saved_credentials');
        }

        login({ emailAddress: form.email, password: form.password })
            .then(response => {
                if (response.error && response.error.status === 401) {
                    toast.error('Invalid email/password. Please try again.');
                } else if (response.error && response.error.status === 500) {
                    toast.error('A problem occurred while logging in. Please contact a system administrator');
                } else {
                    navigate('/');
                    setSession();
                }
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
                    textAlign: 'center'
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
                            placeholder='Password'
                            variant='outlined'
                            size='small'
                            value={form.password}
                            onChange={(e) => setForm({ ...form, password: e.currentTarget.value, passwordError: false })}
                            error={form.passwordError}
                            helperText={form.passwordError ? 'Please enter a valid password' : null}
                            type={showPassword ? 'text' : 'password'}
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position='end'>
                                        <IconButton onClick={() => setShowPassword(!showPassword)} edge='end'>
                                            {showPassword ? <VisibilityOff /> : <Visibility />}
                                        </IconButton>
                                    </InputAdornment>
                                )
                            }}
                        />
                        <Box
                            component='div'
                            sx={{
                                display: 'flex',
                                justifyContent: 'space-between',
                                alignItems: 'center'
                            }}
                        >
                            <FormControlLabel
                                control={<Checkbox checked={form.rememberMe} onChange={e => setForm({ ...form, rememberMe: e.target.checked })} />}
                                label='Remember Me'
                            />
                            <Link
                                href='/forgot-password'
                                color='secondary'
                            >
                                Forgot Password?
                            </Link>
                        </Box>
                        <LoginButton
                            variant='contained'
                            onClick={() => validateAndLogin()}
                        >
                            Login
                        </LoginButton>
                        <Link
                            href='/self-registration'
                            color='secondary'
                        >
                            Sign Up for an Account
                        </Link>
                    </Stack>
                </CardContent>
            </Card>
        </Box>
    );
}