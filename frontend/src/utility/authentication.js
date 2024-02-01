/**
 * Clears any local storage keys and other items related to
 * a session and routes to the login page.
 */
export const clearSession = () => {
    localStorage.removeItem('sample_session');
}

/**
 * Replaces the current URL with the login URL.
 */
export const navigateToLogin = () => {
    window.location.replace(window.location.origin + "/login");
}

/**
 * Determines if there is an active session.
 * @returns boolean
 */
export const isLoggedIn = () => {
    return !!+localStorage.getItem('sample_session');
}

/**
 * Sets a helper key in local storage used for determining if there
 * is an active session.
 */
export const setSession = () => {
    localStorage.setItem('sample_session', 1);
}