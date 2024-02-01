import { isRejectedWithValue } from '@reduxjs/toolkit';
import { clearSession } from "../../utility";
import { api } from "../api";

export const unauthenticatedMiddleware = ({ dispatch }) => next => action => {
    if (isRejectedWithValue(action) && action.payload.status === 401) {
        clearSession();
        dispatch(api.util.resetApiState());
    }

    return next(action);
};