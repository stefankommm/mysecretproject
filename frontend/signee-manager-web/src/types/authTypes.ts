export interface IUserData {
    username: string,
    email: string,
    token: string
}

export interface ILoginRequest {
    email: string,
    password: string
}

export interface IRegisterRequest {
    email: string,
    username: string,
    password: string,
    role: IUserRole
}

export enum IUserRole {
    ADMIN = 'Admin',
    USER = 'User'
}