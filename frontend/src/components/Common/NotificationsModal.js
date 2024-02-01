import { Typography, Dialog, DialogTitle, DialogContent } from '@mui/material';

export default function NotificationsModal({ open, onClose }) {
    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>Notifications</DialogTitle>
            <DialogContent>
                <Typography paragraph>Future Notifications Modal Content</Typography>
            </DialogContent>
        </Dialog>
    )
}