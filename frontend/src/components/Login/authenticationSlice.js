import { api } from '../../app/api';

export const authenticationApi = api.injectEndpoints({
    endpoints: (builder) => ({
        login: builder.mutation({
            query: (payload) => ({
                url: 'login',
                method: 'POST',
                body: payload
            })
        }),
        logout: builder.mutation({
            query: () => ({
                url: 'logout',
                method: 'POST',
                body: {}
            })
        })
    })
})

export const {
    useLoginMutation,
    useLogoutMutation
} = authenticationApi;