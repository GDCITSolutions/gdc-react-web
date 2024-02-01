import { api } from '../../app/api';

export const selfRegistrationApi = api.injectEndpoints({
    endpoints: (builder) => ({
        selfRegister: builder.mutation({
            query: (payload) => ({
                url: 'user/self-registration',
                method: 'POST',
                body: payload
            })
        })
    })
})

export const {
    useSelfRegisterMutation
} = selfRegistrationApi;