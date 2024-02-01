import { TextField, Grid } from "@mui/material";
import React from "react";
import { useGetSystemStatusesQuery } from "../../../app/slices/systemStatusSlice";
import Dropdown from '../../Common/FormElements/Dropdown';

function UserManagementFilters({ filters, setFilters, schools }) {
    const { data: systemStatuses = [] } = useGetSystemStatusesQuery();

    const onFilterChange = (name, value) => {
        const f = { ...filters };

        f[name] = value;

        setFilters(f);
    };

    return (
        <Grid container>
            <Grid item md={8} lg={8} alignContent="center" fontWeight="bold">
                User Management
            </Grid>
            <Grid item md={4} lg={4} justifyContent="space-between">
                <Grid container>
                    <Grid item md={4} lg={4} paddingLeft="10px">
                        <Dropdown 
                            label="Status"
                            menuItems={systemStatuses}
                            value={filters.statusId}
                            onChange={v => onFilterChange('status', v)}
                        />
                    </Grid>
                    <Grid item md={4} lg={4} paddingLeft="10px">
                        <TextField
                            id="user-search-term"
                            label="Search"
                            variant="outlined"
                            size="small"
                            type="text"
                            value={filters.searchTerm}
                            onChange={e => onFilterChange('searchTerm', e.target.value)}
                        />
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    );
}

export default UserManagementFilters;