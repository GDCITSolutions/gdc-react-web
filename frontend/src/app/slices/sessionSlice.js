import { api } from "../api";

export const sessionApi = api.injectEndpoints({
    tagTypes: ['Session'],
    endpoints: builder => ({
        getSession: builder.query({
            query: () => 'user/session',
            providesTags: (result, error, arg) => [{ type: 'Session' }]
        })
    })
})

export const {
    useGetSessionQuery
} = sessionApi;