import React from 'react';
import { Box, Dialog, DialogContent, DialogContentText, Divider } from '@mui/material';
import styled from 'styled-components';

const DialogCard = styled(Dialog)`
  & .css-1t1j96h-MuiPaper-root-MuiDialog-paper {
    border-radius: 15px;
    box-shadow: none;
  }

  & .css-yiavyu-MuiBackdrop-root-MuiDialog-backdrop {
    background: transparent;
  }
`;

const DialogCardText = styled(DialogContentText)`
  text-align: center;
  font-family: "Roboto" !important; 
  font-size: 1rem;
  color: black !important;
  font-weight: 500 !important;
`;

function EmailFoundConfirmation({isOpen, close}) {

  return (
    <>
      <Box sx={{backgroundColor: "#E7E7E7", height: "100vh"}}>
        <DialogCard
            open={isOpen}
            onClose={() => close()}
            aria-describedby="alert-dialog-description"
        >
            <span className='material-symbols-outlined' style={{ fontVariationSettings: '"FILL" 1' }}>dashboard</span>

            <DialogContent sx={{ paddingBottom:'10%' }}>
              <Divider sx={{ marginBottom:"5%", borderBottomWidth:"medium" }} light />
              <DialogCardText id="alert-dialog-description">
                Thank you. If a profile is found, a password reset link will
                <br></br>be sent to the email submitted.
              </DialogCardText>
            </DialogContent>
        </DialogCard>
      </Box>
    </>
  );
}

export default EmailFoundConfirmation;