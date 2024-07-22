import { ApiResponse } from "@/model/ApiResponse";
import { UserCurrentInfo } from "@/model/User";
import http from "@/util/http";

export const GetCurrentUserInfo = () => http.get<ApiResponse<UserCurrentInfo>>("Users/get-current-user-info");

export const UpdateCurrentUser = (user: FormData) => http.put<ApiResponse<boolean>>("Users/update-current-user", user, {
    headers: {
        'Content-Type': 'multipart/form-data'
    }
})

export const UpdateAchievements = (achievements: string[]) => http.put<ApiResponse<boolean>>("Users/update-achievements", achievements);
