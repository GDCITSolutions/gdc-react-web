import { Stack } from '@mui/material';
import { useState, useMemo } from 'react';
import { useGetAllUsersQuery, useLockUserMutation, useDeleteUserMutation } from './userManagementSlice';
import { toast } from "react-toastify";
import UserManagementDataGrid from "./UserManagementDataGrid";
import UserManagementFilters from "./UserManagementFilters";
import EditUserDialog from "./EditUserDialog";
import AddUser from "./AddUser";
import ConfirmationModal from '../../Common/ConfirmationModal';

const defaultFilters = {
    school: null,
    status: null,
    searchTerm: ''
};

function UserManagementContainer() {
    const [deletingUser, setDeletingUser] = useState(null);
    const [editingUser, setEditingUser] = useState(null);
    const [lockingUser, setLockingUser] = useState(null);
    const [filters, setFilters] = useState({...defaultFilters});

    const [lockUser] = useLockUserMutation();
    const [deleteUser] = useDeleteUserMutation();

    const { data: users = [], isLoading } = useGetAllUsersQuery();
    const filteredUsers = useMemo(() => {
        return users?.filter(u => {
            if (filters.status?.id && u.systemStatusId !== filters.status.id) return false;
            if (filters.searchTerm && 
                !(u.firstName?.toUpperCase().includes(filters.searchTerm.toUpperCase()) ||
                u.lastName?.toUpperCase().includes(filters.searchTerm.toUpperCase()) ||
                u.emailAddress.toUpperCase().includes(filters.searchTerm.toUpperCase())))
                return false;
            
            return true;
        })
    }, [users, filters]);

    function onDeleteUser(id) {
        deleteUser(id)
            .then((response) => {
                const isDeleting = response?.data?.isDeleting;

                if (isDeleting === null)
                    toast.error(`Failed to delete user`);

                setDeletingUser(null);
                toast.success('Successfully deleted user');
            })
            .catch((error) => {
                toast.error(error);
                setDeletingUser(null);
            });
    }

    function onLockUser(id) {
        lockUser(id)
            .then((response) => {
                const isLocked = response?.data?.isLocked;

                if (isLocked === null)
                    toast.error(`Failed to unlock/lock user`);
                
                setLockingUser(null);
                toast.success(`Successfully ${isLocked ? 'locked' : 'unlocked'} user`);
            })
            .catch(error => {
                toast.error(error);
                setLockingUser(null);
            })
    }

    return (
        <>
            <Stack spacing={3}>
                <AddUser />
                <UserManagementFilters 
                    filters={filters}
                    setFilters={setFilters}
                />
            </Stack>
            <UserManagementDataGrid 
                users={filteredUsers}
                isLoading={isLoading}
                setDelete={setDeletingUser}
                setEdit={setEditingUser}
                setLock={setLockingUser}
            />
            <EditUserDialog 
                isOpen={!!editingUser}
                user={editingUser}
                close={() => setEditingUser(null)}
            />
            <ConfirmationModal
                isOpen={!!deletingUser}
                close={() => setDeletingUser(null)}
                onConfirm={() => onDeleteUser(deletingUser.id)}
                message={<div>Please confirm you'd like to delete the selected options. <br /> This is not a reversible option once it's deleted.</div>}
                saveBtnText="Save and Delete"
            />
            <ConfirmationModal
                isOpen={!!lockingUser}
                user={lockingUser}
                close={() => setLockingUser(null)}
                onConfirm={() => onLockUser(lockingUser)}
                message="Please confirm you'd like to continue with the requested change."
                saveBtnText="Save and Continue"
            />
        </>
    );
};

export default UserManagementContainer;