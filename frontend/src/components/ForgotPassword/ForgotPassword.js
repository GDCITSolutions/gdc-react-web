import React, { useState } from 'react';
import { Box, Card, CardContent, Typography, Divider, TextField, Grid, Button } from '@mui/material';
import { useForgotPasswordMutation } from './ForgotPasswordSlice';
import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import EmailFoundConfirmation from '../EmailFoundConfirmation/EmailFoundConfirmation';

const ForgotPasswordCard = styled(Card)`
    display: flex; 
    justify-content: center; 
    align-items: center; 
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translateX(-50%) translateY(-50%);
    width: 510px; 
    border: 1px solid #CCCCCC;
    border-radius: 1em !important;
`;

const EmailTextField = styled(TextField)`
    width: 100%; 
    margin-bottom: 5% !important;
    margin-top: 5% !important; 
    color: #707070; 
    font-family: "Open Sans"; 
    font-size: .8rem;
`;

const CancelButton = styled(Button)`
    font-size: 14px !important; 
    text-transform: none !important; 
    width: 100%; 
    background-color: #000000 !important;
`;

const SubmitButton = styled(Button)`
    font-size: 14px !important; 
    text-transform: none !important; 
    width: 100%; 
    background-color: #011E41 !important;
    &:hover {
        background-color: #026AD3 !important;
    },
`;

function ForgotPassword() {
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [showForm, setShowForm] = useState(true);
    const [form, setForm] = useState({
        email: '',
        emailError: false
    });

    const [forgotPassword] = useForgotPasswordMutation();
    const navigate = useNavigate();

    function SendResetEmail() {
        var emailError = false;
        if (form.email === '') emailError = true;
        if (emailError) {
            setForm({...form, emailError: true});
            return;
        }
        forgotPassword(form.email);
        setIsDialogOpen(true);
        setShowForm(false);
    }

    function CancelForm() {
        navigate('/login');
    }

    return (
        <>
        <Box sx={{backgroundColor: "#E7E7E7", height: "100vh"}}>
            <ForgotPasswordCard variant="outlined" sx={{ display: showForm ? 'flex' : 'none' }}>
                    <CardContent>

                        <span className='material-symbols-outlined' style={{ fontVariationSettings: '"FILL" 1' }}>dashboard</span>
                        <Divider sx={{marginTop:"5%", marginBottom:"5%"}} color="#CCCCCC" light />
                        <Typography variant="body1" align="center" fontFamily="Roboto" fontSize="1.2rem">
                            Please enter your email address to request a password reset for your account.
                        </Typography>
                        <EmailTextField variant="outlined" placeholder='Email Address' size="small" type="email" required 
                            value={form.email}
                            onChange={(e) => setForm({ ...form, email: e.currentTarget.value, emailError: false })}
                            error={form.emailError}
                            helperText={form.emailError ? 'Please enter a valid email address' : null}/>
                        <br />
                        <Grid container rowSpacing={1} columnSpacing={{ xs: 1 }}>
                            <Grid item xs={6}>
                                <CancelButton onClick={() => CancelForm()} variant="contained" size="large">Cancel</CancelButton>
                            </Grid>
                            <Grid item xs={6}>
                                <SubmitButton onClick={() => SendResetEmail()} variant="contained" size="large">Submit Request</SubmitButton> 
                            </Grid>
                        </Grid>
                    </CardContent>
            </ForgotPasswordCard>
        </Box>
        {isDialogOpen &&
                <EmailFoundConfirmation isOpen={isDialogOpen} close={() => { setIsDialogOpen(false); navigate('/login')}} />
        }
        </>
    )
}

export default ForgotPassword;