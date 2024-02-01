import { Typography, Dialog, DialogTitle, DialogContent } from '@mui/material';

export default function UpdatePasswordModal({ open, onClose }) {
    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>Update Password</DialogTitle>
            <DialogContent>
                <Typography paragraph>Future Update Password Modal Content</Typography>
            </DialogContent>
        </Dialog>
    )
}