import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

const url = process.env.NODE_ENV === 'production'
    ? "https://local-accountability-system-api-test.azurewebsites.net"
    : "http://localhost:5181"

export const api = createApi({
    baseQuery: fetchBaseQuery({
        baseUrl: url + "/api/",
        credentials: "include",
        prepareHeaders: (headers, { endpoint }) => {
            // fetch cannot be relied on to set this
            // yes, "theoretically" we set these at the endpoint level but I have not seen this behavior first hand
            // so, any special endpoints need to be handled here
            
            if (endpoint !== 'bulkImportUsers' && endpoint !== 'bulkImportSchools')
                headers.set('Content-Type', 'application/json');

            return headers;
        },
    }),
    tagTypes: ['SomeTag'],
    endpoints: () => ({})
});