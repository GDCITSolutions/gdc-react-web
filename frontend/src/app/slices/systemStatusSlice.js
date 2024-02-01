import { api } from "../api";

export const systemStatusesApi = api.injectEndpoints({
    tagTypes: ['SystemStatuses'],
    endpoints: builder => ({
        getSystemStatuses: builder.query({
            query: () => '/lookup/system-statuses',
            providesTags: (result, _error, _arg) =>
                result
                    ? result.map(({ id }) => ({ type: 'SystemStatus', id }))
                    : ['SystemStatus'],
        })
    })
})

export const {
    useGetSystemStatusesQuery
} = systemStatusesApi