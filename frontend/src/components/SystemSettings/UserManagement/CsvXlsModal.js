import React from 'react';
import { Dialog, DialogActions, Button } from '@mui/material';
import { API_BASE_URL } from '../../../utility/url';


function CsvXlsModal({ isOpen, close }) {

    return (
        <>
        <Dialog open={isOpen} onClose={close}>
            <DialogActions sx={{ justifyContent: 'space-between', padding: '1.5em' }}>
                <Button variant="contained" color="secondary" size="large" href={`${API_BASE_URL}/templates/user_data.csv`} download>Download CSV File</Button>
                <Button variant="contained" color="primary" size="large" href={`${API_BASE_URL}/templates/user_data.xlsx`} download>Download Excel File</Button>
            </DialogActions>
        </Dialog>
        </>
    )
}

export default CsvXlsModal;
