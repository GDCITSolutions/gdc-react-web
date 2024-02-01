import { useState } from 'react';
import { toast } from "react-toastify";
import { Button, Grid, TextField, Paper, Select, InputLabel,
     MenuItem, ListItemText, Checkbox, Box, FormControl, FormHelperText, Link } from '@mui/material';
import { useAddUserMutation } from './userManagementSlice';
import { useGetRolesQuery } from '../../../app/slices/rolesSlice';
import Dropdown from '../../Common/FormElements/Dropdown';
import { Plus } from '../../Icons';
import { REGEX } from '../../../utility/regex';
import BulkImportDialog from './BulkImportDialog';
import CsvXlsModal from './CsvXlsModal';

function AddUser() {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [emailAddress, setEmailAddress] = useState('');
    const [confirmEmailAddress, setConfirmEmailAddress] = useState('');
    const [userRoles, setUserRoles] = useState([]);
    const [formErrors, setFormErrors] = useState({});
    const [isRequesting, setIsRequesting] = useState(false);
    const [isBulkImportOpen, setIsBulkImportOpen] = useState(false);
    const [isDownloadingFile, setIsDownloadingFile] = useState(false);

    const [addUser] = useAddUserMutation();
    const { data: roles = [] } = useGetRolesQuery();

    function reset() {
        setFirstName('');
        setLastName('');
        setEmailAddress('');
        setConfirmEmailAddress('');
        setUserRoles([]);
        setFormErrors({});
    }

    function isFormValid() {
        const errors = {};

        errors['add-first-name'] = !firstName?.trim() ? 'Field is required' : null;
        errors['add-last-name'] = !lastName?.trim() ? 'Field is required' : null;
        errors['add-email-address'] = !emailAddress?.trim() ? 'Field is required' : null;
        errors['add-confirm-email-address'] = !confirmEmailAddress?.trim() ? 'Field is required' : null;
        errors.roles = !userRoles?.length ? 'Field is required' : null;

        if (!errors['add-email-address'])
            errors['add-email-address'] = !emailAddress.match(REGEX.EMAIL) ? 'Email must be properly formatted' : null;

        if (!errors['add-email-address'])
            errors['add-email-address'] = emailAddress !== confirmEmailAddress ? 'Emails must match' : null;

        if (!errors['add-confirm-email-address'])
            errors['add-confirm-email-address'] = !confirmEmailAddress.match(REGEX.EMAIL) ? 'Email must be properly formatted' : null;

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

    function validateSelect(e) {
        const { name, value } = e.target;

        setFormErrors(prev => ({
            ...prev,
            [name]: !value?.length > 0 ? 'Field is required' : null
        }));
    }

    function submit() {
        if (!isFormValid())
            return;

        const dto = {
            firstName,
            lastName,
            emailAddress,
            roleIds: roles.filter(x => userRoles.includes(x.value)).map(x => x.id)
        }

        setIsRequesting(true);
        addUser(dto)
            .then((response) => {
                setIsRequesting(false);

                if (response?.error)
                    throw new Error("An error occurred when adding user");

                if (response?.data?.message) {
                    setFormErrors({ ...formErrors, emailAddress: response.data.message })
                    return;
                }

                toast.success('Successfully added user');
                reset();
            })
            .catch(error => {
                setIsRequesting(false);
                toast.error(error);
            });
    }

    function onRoleChange(e) {
        const value = e.target.value;

        setUserRoles(typeof value === 'string' ? value.split(',') : value);
    }

    return (
        <Paper sx={{ padding: '15px', backgroundColor: (theme) => theme.palette.grey[100] }}>
            <Grid container spacing={2}>
                <Grid item md={2} lg={2}>
                    <TextField
                        id="add-first-name"
                        label="First Name"
                        variant="outlined"
                        size="small"
                        type="text"
                        sx={{
                            '& .MuiInputBase-root': {
                                backgroundColor: 'white'
                            }
                        }}
                        fullWidth
                        value={firstName}
                        onBlur={validate}
                        error={!!formErrors['add-first-name']}
                        helperText={formErrors['add-first-name']}
                        onChange={e => setFirstName(e.target.value)}
                    />
                </Grid>
                <Grid item md={2} lg={2}>
                    <TextField
                        id="add-last-name"
                        label="Last Name"
                        variant="outlined"
                        sx={{
                            '& .MuiInputBase-root': {
                                backgroundColor: 'white'
                            }
                        }}
                        size="small"
                        type="text"
                        fullWidth
                        value={lastName}
                        onBlur={validate}
                        error={!!formErrors['add-last-name']}
                        helperText={formErrors['add-last-name']}
                        onChange={e => setLastName(e.target.value)}
                    />
                </Grid>
                <Grid item md={2} lg={2}>
                    <TextField
                        id="add-email-address"
                        label="Email Address"
                        variant="outlined"
                        sx={{
                            '& .MuiInputBase-root': {
                                backgroundColor: 'white'
                            }
                        }}
                        size="small"
                        type="text"
                        fullWidth
                        value={emailAddress}
                        onBlur={validate}
                        error={!!formErrors['add-email-address']}
                        helperText={formErrors['add-email-address']}
                        onChange={e => setEmailAddress(e.target.value)}
                    />
                </Grid>
                <Grid item md={2} lg={2}>
                    <TextField
                        id="add-confirm-email-address"
                        label="Confirm Email Address"
                        sx={{
                            '& .MuiInputBase-root': {
                                backgroundColor: 'white'
                            }
                        }}
                        variant="outlined"
                        size="small"
                        type="text"
                        fullWidth
                        value={confirmEmailAddress}
                        onBlur={validate}
                        error={!!formErrors['add-confirm-email-address']}
                        helperText={formErrors['add-confirm-email-address']}
                        onChange={e => setConfirmEmailAddress(e.target.value)}
                    />
                </Grid>
                <Grid item md={2} lg={2}>
                    <FormControl error={!!formErrors['roles']} fullWidth>
                        <InputLabel id="roles-select-label" size="small">Role(s)</InputLabel>
                        <Select
                            labelId="roles-select-label"
                            label="Role(s)"
                            id="roles"
                            name="roles"
                            sx={{
                                '& .MuiSelect-select': {
                                    backgroundColor: 'white'
                                }
                            }}
                            size="small"
                            multiple
                            fullWidth
                            variant="outlined"
                            value={userRoles}
                            onBlur={validateSelect}
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
                </Grid>
                <Grid item md={12} lg={12}>
                    <Button 
                        sx={{ padding: 0 }} 
                        variant="standard" 
                        type="button" 
                        disabled={isRequesting} 
                        onClick={submit}
                    >
                        <Plus className="action-light-blue" /><span className="action-light-blue">Add User</span>
                    </Button>
                    <Link sx={{ marginLeft: '20px' }} component="button" onClick={() => setIsBulkImportOpen(true)} disabled={isRequesting}>
                        Import Users via CSV/XSLX
                    </Link>

                    <Link sx={{ marginLeft: '20px' }} component="button" onClick={() => setIsDownloadingFile(true)} disabled={isRequesting}>
                        CSV/XLS Template
                    </Link>
                </Grid>
            </Grid>
            <BulkImportDialog 
                isOpen={isBulkImportOpen}
                close={() => setIsBulkImportOpen(false)}
            />
            <CsvXlsModal
                isOpen={isDownloadingFile}
                close={() => setIsDownloadingFile(false)}
            />
        </Paper>
    );
};

export default AddUser;