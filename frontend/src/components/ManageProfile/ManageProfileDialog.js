import { useState, useEffect } from 'react';
import { toast } from "react-toastify";
import { Dialog, DialogTitle, DialogContent, TextField, Stack, Button, IconButton, InputAdornment } from '@mui/material';
import { Visibility, VisibilityOff } from '../Icons';
import styled from 'styled-components';
import { REGEX } from '../../utility/regex';
import { useUpdateProfileMutation } from './manageProfileSlice';

const CancelButton = styled(Button)`&:hover { background-color: #ff0000 !important; }`;
const SaveButton = styled(Button)`&:hover { background-color: #026AD3 !important; }`;

export default function ManageProfileDialog({ open, onClose, user }) {
    const [isLoading, setIsLoading] = useState(false);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [emailAddress, setEmailAddress] = useState('');
    const [password, setPassword] = useState('');
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const [newPassword, setNewPassword] = useState('');
    const [isNewPasswordVisible, setIsNewPasswordVisible] = useState(false);
    const [verifyPassword, setVerifyPassword] = useState('');
    const [isVerifyPasswordVisible, setIsVerifyPasswordVisible] = useState(false);
    const [formErrors, setFormErrors] = useState({});

    const [updateProfile] = useUpdateProfileMutation();

    useEffect(() => {
        setFirstName(user.firstName);
        setLastName(user.lastName);
        setEmailAddress(user.emailAddress);
    }, [user.firstName, user.lastName, user.emailAddress]);

    function validate(e) {
        setFormErrors(prev => ({
            ...prev,
            [e.target.id]: !e.target.value.trim() ? 'Field is required' : null
        }));
    }

    function isFormValid() {
        const errors = {};

        errors['first-name'] = !firstName?.trim() ? 'Field is required' : null;
        errors['last-name'] = !lastName?.trim() ? 'Field is required' : null;
        errors['email-address'] = !emailAddress?.trim() ? 'Field is required' : null;
        errors['password'] = !password ? 'Field is required' : null;

        if (!errors['email-address'])
            errors['email-address'] = !emailAddress.match(REGEX.EMAIL) ? 'Email must be properly formatted' : null;

        if (!errors['new-password'])
            errors['new-password'] = !!newPassword && !newPassword.match(REGEX.PASSWORD) ? 'Password must be atleast 8 characters long and contain 1 Capital, 1 Lowercase, 1 Number, 1 Special Character (@, %, ^, $, !, *)' : null;

        if (!errors['verify-password'])
            errors['verify-password'] = verifyPassword !== newPassword ? 'Verify password must match the new password' : null;

        setFormErrors(errors);

        for (let prop in errors)
            if (errors.hasOwnProperty(prop) && errors[prop] != null)
                return false;

        return true;
    }

    function submit() {
        if (!isFormValid())
            return;

        const dto = {
            id: user.id,
            firstName: firstName,
            lastName: lastName,
            emailAddress: emailAddress,
            password: password,
            newPassword: newPassword
        }

        setIsLoading(true);

        updateProfile(dto)
            .then(response => {
                setIsLoading(false);

                if (response.error)
                    toast.error(response.error);

                toast.success('Successfully update user profile.');

                onClose();
            })
            .catch(e => {
                setIsLoading(false);
                toast.error(e);
            });
    }

    return (
        <>
            <Dialog open={open} onClose={onClose}>
                <DialogTitle>
                    <Stack spacing={2} alignItems="center">
                        <span className='material-symbols-outlined' style={{ fontVariationSettings: '"FILL" 1' }}>dashboard</span>

                        <div>Profile Update</div>
                    </Stack>
                </DialogTitle>
                <DialogContent>
                    <Stack spacing={2} sx={{ width: '30em', paddingTop: '0.5em' }}>
                        <TextField
                            id="first-name"
                            label="First Name"
                            variant="outlined"
                            size="small"
                            type="text"
                            fullWidth
                            value={firstName}
                            onBlur={validate}
                            error={!!formErrors['first-name']}
                            helperText={formErrors['first-name']}
                            onChange={e => setFirstName(e.target.value)}
                        />
                        <TextField
                            id="last-name"
                            label="Last Name"
                            variant="outlined"
                            size="small"
                            type="text"
                            fullWidth
                            value={lastName}
                            onBlur={validate}
                            error={!!formErrors['last-name']}
                            helperText={formErrors['last-name']}
                            onChange={e => setLastName(e.target.value)}
                        />
                        <TextField
                            id="email-address"
                            label="Email Address"
                            variant="outlined"
                            size="small"
                            type="text"
                            fullWidth
                            value={emailAddress}
                            onBlur={validate}
                            error={!!formErrors['email-address']}
                            helperText={formErrors['email-address']}
                            onChange={e => setEmailAddress(e.target.value)}
                        />
                        <TextField
                            id="password"
                            label="Password"
                            variant='outlined'
                            size='small'
                            value={password}
                            onChange={(e => setPassword(e.target.value))}
                            error={!!formErrors['password']}
                            helperText={formErrors['password']}
                            type={isPasswordVisible ? 'text' : 'password'}
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position='end'>
                                        <IconButton
                                            onClick={() => setIsPasswordVisible(!isPasswordVisible)}
                                            edge='end'
                                            title={isPasswordVisible ? 'Hide Password' : 'Show Password'}
                                        >
                                            {isPasswordVisible ? <VisibilityOff /> : <Visibility />}
                                        </IconButton>
                                    </InputAdornment>
                                )
                            }}
                        />
                        <TextField
                            id="new-password"
                            label="New Password"
                            variant='outlined'
                            size='small'
                            value={newPassword}
                            onChange={(e => setNewPassword(e.target.value))}
                            error={!!formErrors['new-password']}
                            helperText={formErrors['new-password']}
                            type={isNewPasswordVisible ? 'text' : 'password'}
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position='end'>
                                        <IconButton
                                            onClick={() => setIsNewPasswordVisible(!isNewPasswordVisible)}
                                            edge='end'
                                            title={isNewPasswordVisible ? 'Hide Password' : 'Show Password'}
                                        >
                                            {isNewPasswordVisible ? <VisibilityOff /> : <Visibility />}
                                        </IconButton>
                                    </InputAdornment>
                                )
                            }}
                        />
                        <TextField
                            id="verify-password"
                            label="Verify Password"
                            variant='outlined'
                            size='small'
                            value={verifyPassword}
                            onChange={(e => setVerifyPassword(e.target.value))}
                            error={!!formErrors['verify-password']}
                            helperText={formErrors['verify-password']}
                            type={isVerifyPasswordVisible ? 'text' : 'password'}
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position='end'>
                                        <IconButton
                                            onClick={() => setIsVerifyPasswordVisible(!isVerifyPasswordVisible)}
                                            edge='end'
                                            title={isVerifyPasswordVisible ? 'Hide Password' : 'Show Password'}
                                        >
                                            {isVerifyPasswordVisible ? <VisibilityOff /> : <Visibility />}
                                        </IconButton>
                                    </InputAdornment>
                                )
                            }}
                        />
                        <TextField
                            id="school"
                            label="Sample Field"
                            variant="outlined"
                            size="small"
                            type="text"
                            fullWidth
                            value="Some Sample Value"
                            disabled
                        />
                        <Stack direction="row" spacing={10} justifyContent="center">
                            <CancelButton
                                variant="contained"
                                color="danger"
                                type="button"
                                onClick={() => onClose()}
                                disabled={isLoading}
                            >
                                Cancel
                            </CancelButton>
                            <SaveButton
                                variant="contained"
                                color="primary"
                                type="button"
                                onClick={submit}
                                disabled={isLoading}
                            >
                                Save
                            </SaveButton>
                        </Stack>
                    </Stack>
                </DialogContent>
            </Dialog >
        </>
    )
}