import { api } from '../../app/api';
const TAG = 'ManageProfile';

export const manageProfileApi = api.injectEndpoints({
    tagTypes: [TAG],
    endpoints: builder => ({
        updateProfile: builder.mutation({
            query: profile => ({
                url: 'user/profile',
                method: 'PUT',
                body: JSON.stringify(profile)
            })
        })
    })
});

export const {
    useUpdateProfileMutation
} = manageProfileApi;