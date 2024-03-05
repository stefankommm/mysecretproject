import {create} from "zustand"

type AuthState = {
    loggedIn: boolean,
    sessionToken: null | string
}

type AuthAction = {
    login: (token: string) => void,
    logout: () => void
}

export const useAuthStore = create<AuthState & AuthAction>((set) => ({
    loggedIn: false,
    sessionToken: null,
    login: (token: string) => set({loggedIn: true, sessionToken: token}),
    logout: () => set({loggedIn: false, sessionToken: null})
}))
