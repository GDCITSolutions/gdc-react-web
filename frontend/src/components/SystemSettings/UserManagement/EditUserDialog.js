import { useState, useEffect } from 'react';
import { Close, Plus } from "../../Icons";
import { toast } from "react-toastify";
import { Box, Stack } from '@mui/material';
import { Button, Dialog, DialogActions, DialogContent, MenuItem, ListItemText, 
     DialogTitle, IconButton, TextField, Autocomplete, Checkbox, InputLabel, FormControl, Select, FormHelperText } from "@mui/material";
import { useUpdateUserMutation } from './userManagementSlice';
import { useGetRolesQuery } from '../../../app/slices/rolesSlice';
import { REGEX } from '../../../utility/regex';
import { useGetSystemStatusesQuery } from "../../../app/slices/systemStatusSlice";

function EditUserDialog({ isOpen, user, close }) {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [emailAddress, setEmailAddress] = useState('');
    const [confirmEmailAddress, setConfirmEmailAddress] = useState('');
    const [userRoles, setUserRoles] = useState([]);
    const [systemStatus, setSystemStatus] = useState('');
    const [formErrors, setFormErrors] = useState({});
    const [isRequesting, setIsRequesting] = useState(false);

    const [updateUser] = useUpdateUserMutation();
    const { data: roles = [] } = useGetRolesQuery();
    const { data: systemStatuses = [] } = useGetSystemStatusesQuery();

    useEffect(() => {
        if (!user) return;

        setFirstName(user.firstName);
        setLastName(user.lastName);
        setSystemStatus(user.systemStatusName);
        setEmailAddress(user.emailAddress);
        setConfirmEmailAddress(user.emailAddress);
        setUserRoles(user.roles?.map(x => x.name));
    }, [user]);

    function reset() {
        setFirstName('');
        setLastName('');
        setEmailAddress('');
        setSystemStatus('');
        setConfirmEmailAddress('');
        setUserRoles([]);
        setFormErrors({});
    }

    function isFormValid() {
        const errors = {};

        errors['update-first-name'] = !firstName?.trim() ? 'Field is required' : null;
        errors['update-last-name'] = !lastName?.trim() ? 'Field is required' : null;
        errors['update-system-status'] = !systemStatus?.trim() ? 'Field is required' : null;
        errors['update-email-address'] = !emailAddress?.trim() ? 'Field is required' : null;
        errors['update-confirm-email-address'] = !confirmEmailAddress?.trim() ? 'Field is required' : null;
        errors.roles = !userRoles?.length ? 'Field is required' : null;

        if (!errors['update-email-address'])
            errors['update-email-address'] = !emailAddress.match(REGEX.EMAIL) ? 'Email must be properly formatted' : null;

        if (!errors['update-email-address'])
            errors['update-email-address'] = emailAddress !== confirmEmailAddress ? 'Emails must match' : null;

        if (!errors['update-confirm-email-address'])
            errors['update-confirm-email-address'] = !confirmEmailAddress.match(REGEX.EMAIL) ? 'Email must be properly formatted' : null;

        setFormErrors(errors);

        for (let prop in errors)
            if (errors.hasOwnProperty(prop) && errors[prop] != null)
                return false;

        return true;
    }

    function validate(e) {
        setFormErrors(prev => ({
            ...prev,
            [e.target.id]: !e.target.value.trim() ? 'Field is required' : null
        }));
    }

    function onRoleChange(e) {
        const value = e.target.value;

        setUserRoles(typeof value === 'string' ? value.split(',') : value);
    }

    function submit() {
        if (!isFormValid())
            return;

        const dto = {
            id: user.id,
            firstName,
            lastName,
            emailAddress,
            roleIds: roles.filter(x => userRoles.includes(x.value)).map(x => x.id),
            systemStatusId: systemStatuses.find(x => x.value === systemStatus).id
        }

        setIsRequesting(true);
        updateUser(dto)
            .then((response) => {
                setIsRequesting(false);

                if (response?.error)
                    throw new Error("An error occurred when updating user");

                if (response?.data?.message) {
                    setFormErrors({ ...formErrors, ['update-email-address']: response.data.message })
                    return;
                }

                toast.success('Successfully updated user');
                reset();
                close();
            })
            .catch(error => {
                setIsRequesting(false);
                console.log(error);
                toast.error(error);
            });
    }

    return (
        <Dialog open={isOpen} onClose={close} fullWidth maxWidth="md">
            <DialogTitle sx={{ fontWeight: 600 }}>
                <Box>
                    Editing User
                </Box>
                <IconButton
                    aria-label="close"
                    onClick={() => { close(); reset(); }}
                    sx={{
                        position: 'absolute',
                        right: 8,
                        top: 8,
                        color: (theme) => theme.palette.grey[500],
                    }}
                >
                    <Close />
                </IconButton>
            </DialogTitle>
            <DialogContent sx={{ paddingTop: '.5em !important' }}>
                <Stack spacing={3}>
                    <TextField
                        id="update-first-name"
                        label="First Name"
                        variant="outlined"
                        type="text"
                        value={firstName}
                        onBlur={validate}
                        error={!!formErrors['firstName']}
                        helperText={formErrors['firstName']}
                        onChange={e => setFirstName(e.target.value)}
                    />
                    <TextField
                        id="update-last-name"
                        label="Last Name"
                        variant="outlined"
                        type="text"
                        value={lastName}
                        onBlur={validate}
                        error={!!formErrors['lastName']}
                        helperText={formErrors['lastName']}
                        onChange={e => setLastName(e.target.value)}
                    />
                    <Autocomplete
                        id="update-system-status"
                        options={systemStatuses.map(x => x.value)}
                        value={systemStatus}
                        onChange={(_, v) => setSystemStatus(v)}
                        renderInput={(params) =>
                            <TextField
                                {...params}
                                label="Status"
                                required
                                onBlur={validate}
                                error={!!formErrors['update-system-status']}
                                helperText={formErrors['update-system-status']}
                            />}
                    />
                    <TextField
                        id="update-email-address"
                        label="Email Address"
                        variant="outlined"
                        type="text"
                        value={emailAddress}
                        onBlur={validate}
                        error={!!formErrors['update-email-address']}
                        helperText={formErrors['update-email-address']}
                        onChange={e => setEmailAddress(e.target.value)}
                    />
                    <TextField
                        id="update-confirm-email-address"
                        label="Confirm Email Address"
                        variant="outlined"
                        type="text"
                        value={confirmEmailAddress}
                        onBlur={validate}
                        error={!!formErrors['update-confirm-email-address']}
                        helperText={formErrors['update-confirm-email-address']}
                        onChange={e => setConfirmEmailAddress(e.target.value)}
                    />
                    <FormControl error={!!formErrors['roles']}>
                        <InputLabel id="update-roles-select-label">Role(s)</InputLabel>
                        <Select
                            id="update-roles-select"
                            labelId="update-roles-select-label"
                            label="Role(s)"
                            multiple
                            variant="outlined"
                            value={userRoles}
                            onChange={onRoleChange}
                            renderValue={(selected) => selected.join(', ')}
                        >
                            {roles.map((r) => (
                                <MenuItem key={`role-${r.id}`} value={r.value}>
                                    <Checkbox checked={userRoles.indexOf(r.value) > -1} />
                                    <ListItemText primary={r.value} />
                                </MenuItem>
                            ))}
                        </Select>
                        {formErrors['roles'] && <FormHelperText sx={{ color: 'red' }}>{formErrors['roles']}</FormHelperText>}
                    </FormControl>
                </Stack>
            </DialogContent>
            <DialogActions sx={{ justifyContent: 'center', padding: '1.5em 0' }}>
                <Button onClick={() => { close(); reset(); }} variant="contained" sx={{ width: '150px' }}>Cancel</Button>
                <Button
                    onClick={submit}
                    variant="contained"
                    disabled={isRequesting}
                    sx={{ width: '150px' }}
                >Update
                </Button>
            </DialogActions>
        </Dialog>
    )
}

export default EditUserDialog;
