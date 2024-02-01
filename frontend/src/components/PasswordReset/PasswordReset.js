import React, { useState, useEffect } from 'react';
import { Box, Card, CardContent, Divider, Typography, TextField, InputAdornment, IconButton, Button } from '@mui/material';
import { useValidateTokenMutation, useResetPasswordMutation } from './PasswordResetSlice';
import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { toast } from 'react-toastify';
import { Visibility, VisibilityOff } from '../Icons';

const PasswordResetCard = styled(Card)`
    display: flex; 
    justify-content: center; 
    align-items: center; 
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translateX(-50%) translateY(-50%);
    width: 510px; 
    border: 1px solid #CCCCCC;
    border-radius: 15px !important;

    & .css-46bh2p-MuiCardContent-root {
        width: 80%;
    }
`;

const PasswordResetCardEmail = styled(Typography)`
    text-align: center;
    font-family: 'Roboto' !important;
    font-size: 18px !important;
    font-weight: bold !important;
`;

const PasswordInput = styled(TextField)`
    width: 100%;
    margin-top: 5% !important;
    font-family: 'Roboto' !important;
    font-size: 14px !important;
    .Mui-error {
        color: #B00020 !important;
    }
`;

const ConfirmPasswordInput = styled(TextField)`
    width: 100%;
    margin-top: 5% !important;
    font-family: 'Roboto' !important;
    font-size: 14px !important;
    .Mui-error {
        color: #B00020 !important;
    }
`;

const VisibilityPasswordIconButton = styled(IconButton)`
    opacity: .5;
    .MuiInputBase-root:focus-within & {
        opacity: 1;
    }
    
`;

const VisibilityConfirmPasswordIconButton = styled(IconButton)`
    opacity: .5;
    .MuiInputBase-root:focus-within & {
        opacity: 1;
    }
    
`;

const SubmitButton = styled(Button)`
    font-size: 14px !important; 
    text-transform: none !important; 
    width: 100%; 
    background-color: #011E41 !important;
    color: white !important;
    margin-top: 3.5% !important;
    width: 55%;
    height: 50px;
    &:hover {
        background-color: #026AD3 !important;
    },
`;

function PasswordReset() {
    const [showPassword, setShowPassword] = useState(true);
    const [showConfirmPassword, setShowConfirmPassword] = useState(true);
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState('');
    const [isSubmitted, setIsSubmitted] = useState(false);
    const navigate = useNavigate();
    const [isTokenValidated, setIsTokenValidated] = useState(false);
    const [email, setEmail] = useState('');

    const [validateTokenMutation] = useValidateTokenMutation();
    const [resetPasswordMutation] = useResetPasswordMutation();

    useEffect(() => {
        const urlParams = new URLSearchParams(window.location.search);
        const tokenFromUrl = urlParams.get('token');

        if (!tokenFromUrl) {
            toast.error('Your link is invalid or has expired.');
            navigate('/forgot-password');
            return;
        }

        validateTokenMutation(tokenFromUrl)
            .then((response) => {
                if (response.data && response.data.success) {
                    setIsTokenValidated(true);
                    setEmail(response.data.email);
                } else {
                    setIsTokenValidated(false);
                    navigate('/forgot-password');
                }
            })
            .catch((error) => {
                console.log(error);
                setIsTokenValidated(false);
                navigate('/forgot-password');
            });
    }, [validateTokenMutation, navigate]);

    const handleClickShowPassword = () => setShowPassword((show) => !show);
    const handleClickShowConfirmPassword = () => setShowConfirmPassword((show) => !show);

    const handleMouseDownPassword = (event) => {
        event.preventDefault();
    };
    const handleMouseDownConfirmPassword = (event) => {
        event.preventDefault();
    };

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
        if (isSubmitted) {
            setError('');
        }
    };

    const handleConfirmPasswordChange = (event) => {
        setConfirmPassword(event.target.value);
        if (isSubmitted) {
            setError('');
        }
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsSubmitted(true);

        const hasNumber = /\d/.test(password);
        const hasLetter = /[a-zA-Z]/.test(password);
        const hasSpecialCharacter = /[@%^$!*]/.test(password);

        if (!hasNumber || !hasLetter || !hasSpecialCharacter) {
            setError('Password must have at least one number, one letter, and one special character.');
            return;
        }

        if (password !== confirmPassword) {
            setError('Passwords do not match.');
            return;
        }

        try {
            const urlParams = new URLSearchParams(window.location.search);
            const tokenFromUrl = urlParams.get('token');

            if (!tokenFromUrl) {
                toast.error('Your link is invalid or has expired.');
                navigate('/forgot-password');
                return;
            }

            const resetResponse = await resetPasswordMutation({
                newPassword: password,
                resetToken: tokenFromUrl,
            }).unwrap();

            if (resetResponse.success) {
                toast.success('Your password has been reset.');
                navigate('/login');
            } else {
                toast.error('Error resetting your password.');
                navigate(`/reset-password?token=${tokenFromUrl}`);
            }
        } catch (error) {
            console.error(error);
        }
    };


    const isPasswordValid = () => {
        const hasNumber = /\d/.test(password);
        const hasLetter = /[a-zA-Z]/.test(password);
        const hasSpecialCharacter = /[@%^$!*]/.test(password);
        const hasMinimumCharacterRequirements = password.length >= 8;
        return hasNumber && hasLetter && hasSpecialCharacter && hasMinimumCharacterRequirements;
    };

    return (
        <>
            {isTokenValidated &&
                (
                    <Box sx={{ backgroundColor: "#E7E7E7", height: "100vh" }}>
                        <PasswordResetCard variant="outlined">
                            <CardContent>
                                <span className='material-symbols-outlined' style={{ fontVariationSettings: '"FILL" 1' }}>dashboard</span>
                                <Divider sx={{ marginTop: "7%", marginBottom: "6%" }} color="#CCCCCC" light />
                                <PasswordResetCardEmail variant="body1">
                                    {email}
                                </PasswordResetCardEmail>
                                <PasswordInput
                                    id="outlined-adornment-password"
                                    type={showPassword ? 'text' : 'password'}
                                    variant="outlined"
                                    value={password}
                                    onChange={handlePasswordChange}
                                    error={!!error}
                                    helperText={isSubmitted ? error : ''}
                                    InputProps={{
                                        endAdornment: (
                                            <InputAdornment position="end">
                                                <VisibilityPasswordIconButton
                                                    aria-label="toggle password visibility"
                                                    onClick={handleClickShowPassword}
                                                    onMouseDown={handleMouseDownPassword}
                                                    edge="end"
                                                >
                                                    {showPassword ? <VisibilityOff /> : <Visibility />}
                                                </VisibilityPasswordIconButton>
                                            </InputAdornment>
                                        )
                                    }}
                                    label="New Password"
                                    placeholder='New Password'
                                    required
                                />
                                <ConfirmPasswordInput
                                    id="outlined-adornment-confirm-password"
                                    type={showConfirmPassword ? 'text' : 'password'}
                                    variant="outlined"
                                    value={confirmPassword}
                                    onChange={handleConfirmPasswordChange}
                                    error={!!error}
                                    helperText={isSubmitted ? error : ''}
                                    disabled={!isPasswordValid()}
                                    InputProps={{
                                        endAdornment: (
                                            <InputAdornment position="end">
                                                <VisibilityConfirmPasswordIconButton
                                                    aria-label="toggle password visibility"
                                                    onClick={handleClickShowConfirmPassword}
                                                    onMouseDown={handleMouseDownConfirmPassword}
                                                    edge="end"
                                                >
                                                    {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                                                </VisibilityConfirmPasswordIconButton>
                                            </InputAdornment>
                                        )
                                    }}
                                    label="Verify Password"
                                    placeholder='Verify Password'
                                    required
                                />
                                <div style={{ display: 'flex', justifyContent: 'center' }}>
                                    <SubmitButton type="submit" onClick={handleSubmit}>
                                        Save Changes
                                    </SubmitButton>
                                </div>

                            </CardContent>
                        </PasswordResetCard>
                    </Box>
                )}
        </>
    );
}

export default PasswordReset;