import { jwtDecode } from "jwt-decode";

/**
 * Checks if JWT token exists and is valid (e.g., not expired).
 * @returns boolean - True if token exists and is valid, false otherwise.
 */
export const isUserLoggedIn = async () => {
    const accessToken = localStorage.getItem("accessToken");

    if (accessToken) {
        try {
            const decodedToken = jwtDecode(accessToken);
            const currentTime = Math.floor(Date.now() / 1000);
            return (decodedToken.exp ?? 0) > currentTime;
        } catch (error) {
            console.error('Error decoding token:', error);
            return false;
        }
    }

    return false;
};