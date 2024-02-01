import { Delete, Edit, Lock, Unlock } from "../../Icons";
import { DataGrid, GridActionsCellItem, GridToolbar } from "@mui/x-data-grid";
import { useGetSessionQuery } from '../../../app/slices/sessionSlice';
import { useMemo } from "react";

function UserManagementDataGrid({ users, isLoading, setEdit, setDelete, setLock }) {
    const { data: session = {} } = useGetSessionQuery();

    const columns = useMemo(() => {
        const columns = [
            {
                field: 'actions',
                type: 'actions',
                headerName: 'Action',
                headerAlign: 'center',
                flex: .75,
                getActions: ({ row }) => {
                    const isLocked = row.lockedByUserId;
                    const cnEdit = isLocked ? row.lockedByUserId !== session?.id ? 'action-locked' : 'action-blue' : 'action-blue';
                    const cnDelete = isLocked ? row.lockedByUserId !== session?.id ? 'action-locked' : 'action-red' : 'action-red';
                    const cnLock = isLocked ? row.lockedByUserId !== session?.id ? 'action-locked' : 'action-purple' : 'action-purple';
                    const isDisabled = isLocked ? row.lockedByUserId !== session?.id : false;

                    return [
                        <GridActionsCellItem
                            icon={<Edit className={cnEdit} />}
                            onClick={() => setEdit(row)}
                            label="Edit"
                            disabled={isDisabled}
                        />,
                        <GridActionsCellItem
                            icon={<Delete className={cnDelete} />}
                            onClick={() => setDelete(row)}
                            label="Delete"
                            disabled={isDisabled}
                        />,
                        <GridActionsCellItem
                            icon={isLocked ? <Lock className={cnLock} /> : <Unlock className={cnLock} />}
                            onClick={() => setLock(row.id)}
                            label="Lock"
                            disabled={isDisabled}
                        />
                    ];
                }
            },
            {
                field: 'firstName',
                headerName: 'First Name',
                flex: 1,
                editable: false,
            },
            {
                field: 'lastName',
                headerName: 'Last Name',
                flex: 1,
                editable: false,
            },
            {
                field: 'emailAddress',
                headerName: 'Email',
                flex: 1,
                editable: false,
            },
            {
                field: 'systemStatusName',
                headerName: 'Status',
                flex: .5,
                editable: false,
            },
            {
                field: 'roles',
                headerName: 'System Role(s)',
                flex: 1,
                editable: false,
                renderCell: ({ row }) => <span>{row.roles?.map(x => x.name).join(', ')}</span>
            }
        ];

        return columns;
    }, [setDelete, setEdit, setLock, session]);

    return (
        <DataGrid
            slots={{ toolbar: GridToolbar }}
            rows={users}
            columns={columns}
            initialState={{
                pagination: { paginationModel: { pageSize: 25, } },
                sorting: { sortModel: [{ field: 'name', sort: 'asc' }] },
            }}
            loading={isLoading}
            getRowClassName={(params) => params.indexRelativeToCurrentPage % 2 !== 0 ? 'grid-odd' : null}
            pageSizeOptions={[25, 50, 100]}
            disableRowSelectionOnClick
        />
    );
}

export default UserManagementDataGrid;
