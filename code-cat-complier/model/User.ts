export type User = {
    id: number;
    firstName: string,
    lastName: string,
    email: string,
    password: string,
    avatar: string,
}
export type UserSignUp = Omit<User,'id'|'avatar'> & {
    rePassword: string
}
export type UserLogin = Pick<User,'email'|'password'>

export type VerifyVerificationCodeRequest = {
    email: string,
    code: string
}
export type ResetPassword = UserLogin & {
    rePassword: string
}

export type UserCurrent = {
    fullName: string,
    avatar: string
}