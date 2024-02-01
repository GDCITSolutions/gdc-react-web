import { api } from '../../app/api';

export const forgotPasswordApi = api.injectEndpoints({
    endpoints: (builder) => ({
        forgotPassword: builder.mutation({
            query: (payload) => ({
                url: 'user/reset',
                method: 'POST',
                body: JSON.stringify(payload)
            })
        })
    })
})

export const {
    useForgotPasswordMutation
} = forgotPasswordApi;