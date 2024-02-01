import { api } from '../../../app/api';
const TAG = 'UserManagement';

export const userManagementApi = api.injectEndpoints({
    tagTypes: [TAG],
    endpoints: (builder) => ({
        getAllUsers: builder.query({
            query: () => 'user',
            providesTags: (result, error, arg) => [{ type: TAG }]
        }),
        bulkImportUsers: builder.mutation({
            query: (file) => ({
                url: 'user/bulk',
                method: 'POST',
                body: file
            }),
            invalidatesTags: [TAG]
        }),
        addUser: builder.mutation({
            query: (user) => ({
                url: 'user',
                method: 'POST',
                body: JSON.stringify(user)
            }),
            invalidatesTags: [TAG]
        }),
        updateUser: builder.mutation({
            query: (user) => ({
                url: 'user',
                method: 'PUT',
                body: JSON.stringify(user)
            }),
            invalidatesTags: (_result, _error, _arg) => [{ type: TAG }]
        }),
        deleteUser: builder.mutation({
            query: (id) => ({
                url: `user/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, _arg) => [{ type: TAG }]
        }),
        lockUser: builder.mutation({
            query: (id) => ({
                url: `user/toggle-lock/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, _arg) => [{ type: TAG }]
        })
    })
})

export const {
    useGetAllUsersQuery,
    useAddUserMutation,
    useBulkImportUsersMutation,
    useUpdateUserMutation,
    useDeleteUserMutation,
    useLockUserMutation
} = userManagementApi;