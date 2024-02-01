import React from 'react';
import { useLocation, Navigate } from "react-router-dom";
import { isLoggedIn } from "../../utility";
import { useGetSessionQuery } from '../../app/slices/sessionSlice';

function RequireAuth({ children }) {
    const location = useLocation();
    const { isError: isAuthError } = useGetSessionQuery();

    // check for authenticated session
    if (!isLoggedIn() || isAuthError)
        return <Navigate to="/login" state={{ from: location, isSessionStale: true }} replace />;

    // authorized
    return children;
}

export default RequireAuth;
