import { api } from '../../app/api';

export const passwordResetApi = api.injectEndpoints({
    endpoints: (builder) => ({
        validateToken: builder.mutation({
            query: (token) => ({
                url: `user/reset?token=${encodeURIComponent(token)}`,
                method: 'GET',
            }),
        }),
        resetPassword: builder.mutation({
            query: ({ newPassword, resetToken }) => ({
                url: `user/credentials?resetToken=${encodeURIComponent(resetToken)}`,
                method: 'PUT',
                body: JSON.stringify(newPassword),
            }),
        }),
    }),
});

export const {
    useValidateTokenMutation,
    useResetPasswordMutation
} = passwordResetApi;
