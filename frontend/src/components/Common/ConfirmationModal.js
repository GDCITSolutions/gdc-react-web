import React from 'react';
import { Dialog, DialogContent, DialogActions, Stack, Divider, Typography, Button } from '@mui/material';
import styled from 'styled-components';

function ConfirmationModal({ isOpen, close, onConfirm, message, saveBtnText }) {

    return (
        <>
        <Dialog open={isOpen} onClose={close}>
            <DialogContent>
                <Stack spacing={2}>
                    <span className='material-symbols-outlined' style={{ fontVariationSettings: '"FILL" 1' }}>dashboard</span>
                    <Divider sx={{marginTop: '5%'}} color="#CCCCCC" light />
                    <Typography variant="body1" align='center'>
                        {message}
                    </Typography>
                </Stack>
            </DialogContent>
            <DialogActions sx={{ justifyContent: 'space-between', padding: '1.5em' }}>
                <Button variant="contained" color="secondary" size="large" onClick={onConfirm}>{saveBtnText}</Button>
                <Button variant="contained" color="primary" size="large" onClick={close}>Cancel</Button>
            </DialogActions>
        </Dialog>
        </>
    )
}

export default ConfirmationModal;