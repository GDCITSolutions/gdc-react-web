import { useState } from 'react';
import { Close } from "../../Icons";
import { toast } from "react-toastify";
import { Box, Stack } from '@mui/material';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, TextField } from "@mui/material";
import { useBulkImportUsersMutation } from './userManagementSlice';

function BulkImportDialog({ isOpen, close }) {
    const [file, setFile] = useState(null);
    const [apiError, setApiError] = useState('');
    const [apiErrors, setApiErrors] = useState([]);

    const [bulkImportUsers] = useBulkImportUsersMutation();

    function reset() {
        setFile(null);
        setApiError('');
        setApiErrors([]);
    }

    function submit() {
        const formData = new FormData();
        formData.append('file', file);

        bulkImportUsers(formData)
            .then(response => {
                if (response?.error)
                    throw new Error("An error occurred when bulk importing users");

                if (response?.data?.messages) {
                    setApiError('');
                    setApiErrors(response?.data?.messages);
                    return;
                }

                if (response?.data?.message) {
                    setApiErrors([]);
                    setApiError(response?.data?.message);
                    return;
                }

                toast.success('Successfully imported users');
                close();
                reset();
            })
            .catch(e => {
                setApiError(e);
            });
    }

    return (
        <Dialog open={isOpen} onClose={() => { close(); reset(); }} fullWidth maxWidth="md">
            <DialogTitle sx={{ fontWeight: 600 }}>
                <Box>
                    Bulk Import
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
            <DialogContent sx={{ paddingTop: '.5em !important', display: 'flex', justifyContent: 'center', flexDirection: 'column' }}>
                <Stack spacing={2}>
                    <TextField
                        id="file-upload"
                        variant="outlined"
                        inputProps={{ accept:".xlsx,.csv" }}
                        type="file"
                        onChange={(e) => setFile(e.target.files[0])}
                    />
                </Stack>
                <Box sx={{ color: 'red' }}>{apiError ? apiError : null}</Box>
                <Box sx={{ color: 'red' }}>{apiErrors.length ? apiErrors.map(x => <Box>{x}</Box>) : null}</Box>
            </DialogContent>
            <DialogActions sx={{ justifyContent: 'center', padding: '1.5em 0' }}>
                <Button onClick={() => { close(); reset(); }} variant="contained" sx={{ width: '150px' }}>Cancel</Button>
                <Button onClick={submit} variant="contained" sx={{ width: '150px' }} disabled={!file}>Submit</Button>
            </DialogActions>
        </Dialog>
    )
}

export default BulkImportDialog;
