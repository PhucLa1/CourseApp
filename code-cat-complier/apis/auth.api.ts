import { ApiResponse } from "@/model/ApiResponse";
import { ResetPassword, UserLogin, UserSignUp, VerifyVerificationCodeRequest } from "@/model/User";
import http from "@/util/http";

export const SignUp = (userSignUp: UserSignUp) => http.post<ApiResponse<string>>('/Auth/sign-up', userSignUp)

export const Login = (userLogin: UserLogin) => http.post<ApiResponse<string>>('/Auth/login', userLogin)

export const GenerateCode = (email: string) => http.post<ApiResponse<boolean>>('/Auth/generate-code', email)

export const VerifyCode = (verifyVerificationCodeRequest: VerifyVerificationCodeRequest) => http.post<ApiResponse<boolean>>('/Auth/verify-code', verifyVerificationCodeRequest)

export const ChangePassword = (resetPassword: ResetPassword) => http.post<ApiResponse<boolean>>('/Auth/change-password', resetPassword)

export const SignOut = () => http.delete<{}>('/Auth/log-out')