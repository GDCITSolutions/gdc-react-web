import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { ToastContainer } from "react-toastify";
import { ThemeProvider } from "@emotion/react";
import theme from "./theme";
import Home from "./components/Home";
import ForgotPassword from './components/ForgotPassword';
import NotFound from "./components/Common/NotFound";
import Login from './components/Login';
import Error from './Error';
import RequireAuth from './components/Common/RequireAuth';
import SelfRegistration from './components/SelfRegistration/SelfRegistration';
import SelfRegistrationConfirmationMessage from './components/SelfRegistration/SelfRegistrationConfirmationMessage';
import SystemSettings from './components/SystemSettings';
import Accessibility from './components/Accessibility';
import PrivacyPolicy from './components/PrivacyPolicy';
import TermsOfService from './components/TermsOfService';
import PasswordReset from './components/PasswordReset';
import 'react-toastify/dist/ReactToastify.css';
import UserManagementContainer from './components/SystemSettings/UserManagement/UserManagementContainer';
import BlankPage from './components/Home/BlankPage';

const router = createBrowserRouter([
    {
        path: '/login',
        element: <Login />,
        error: <NotFound />
    },
    {
        path: '/forgot-password',
        element: <ForgotPassword />,
        error: <NotFound />
    },
    {
        path: '/self-registration',
        element: <SelfRegistration />,
        error: <NotFound />
    },
    {
        path: '/self-registration-confirmation',
        element: <SelfRegistrationConfirmationMessage />,
        error: <NotFound />
    },
    {
        path: '/reset-password',
        element: <PasswordReset />,
        error: <NotFound />
    },
    {
        path: "/",
        element: <RequireAuth><Home /></RequireAuth>,
        errorElement: <Error />,
        children: [
            {
                path: '/home',
                element: <BlankPage />,
                error: <NotFound />
            },
            {
                path: '/system-settings',
                element: <SystemSettings />,
                error: <NotFound />,
                children: [
                    {
                        path: '/system-settings/user-management',
                        element: <UserManagementContainer />,
                        error: <NotFound />
                    }
                ]
            },
            {
                path: '/accessibility',
                element: <Accessibility />,
                error: <NotFound />
            },
            {
                path: '/privacy-policy',
                element: <PrivacyPolicy />,
                error: <NotFound />
            },
            {
                path: '/terms-of-service',
                element: <TermsOfService />,
                error: <NotFound />
            }
        ]
    }
]);

function App() {
    return (
        <ThemeProvider theme={theme}>
            <RouterProvider router={router} />
            <ToastContainer position="bottom-right" />
        </ThemeProvider>
    );
}

export default App;
