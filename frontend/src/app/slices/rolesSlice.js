import { api } from "../api";

export const rolesApi = api.injectEndpoints({
    tagTypes: ['Roles'],
    endpoints: builder => ({
        getRoles: builder.query({
            query: () => '/lookup/roles',
            providesTags: (result, _error, _arg) =>
                result
                    ? result.map(({ id }) => ({ type: 'Role', id }))
                    : ['Role'],
        })
    })
})

export const {
    useGetRolesQuery
} = rolesApi