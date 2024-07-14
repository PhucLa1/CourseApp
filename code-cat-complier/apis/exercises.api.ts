import { ApiResponse } from "@/model/ApiResponse";
import { AdminTagExercise, CommentExercise, CommentExerciseAdd, ExerciseDto, ExerciseRequest, TagExerciseAdd, TagExercises, TagExerciseUpdate } from "@/model/Exercises";
import { PagedResult } from "@/model/PagedResult";
import http from "@/util/http";

type Test = {
    myProperty: File | null;
};
export const GetAllTagExercises = () => http.get<ApiResponse<TagExercises>>("/TagExercises")

export const GetAllAdminTagExercises = () => http.get<ApiResponse<AdminTagExercise[]>>("/TagExercises/admin");

export const UpdateAdminTagExercises = (id: number, tagExerciseUpdate: TagExerciseUpdate) => http.put<ApiResponse<boolean>>(`/TagExercises/admin/${id}`, tagExerciseUpdate);

export const DeleteAdminTagExercises = (id: number) => http.delete<ApiResponse<boolean>>(`/TagExercises/admin/${id}`);

export const DeleteAdminExercises = (id: number) => http.delete<ApiResponse<boolean>>(`/Exercises/${id}`);

export const AddTagExercises = (tagExerciseAdd: TagExerciseAdd) => http.post<ApiResponse<boolean>>("TagExercises/admin", tagExerciseAdd)

export const AddExercise = (body: FormData) => {
    return http.post<ApiResponse<boolean>>("/Exercises", body, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    })
}
export const GetTopicExercise = (id: number) => http.get<ApiResponse<string>>(`/Exercises/${id}`)

export const GetCommentExercise = (exerciseId: number) => http.get<ApiResponse<CommentExercise[]>>(`ExerciseComment/get-by-exercise-id/${exerciseId}`)

export const PostCommentExercise = (commentAddExercise: CommentExerciseAdd) => http.post<ApiResponse<CommentExercise[]>>(`ExerciseComment`, commentAddExercise)

export const GetExercisesPaginatedByOptions = (exerciseRequest: ExerciseRequest, pageNumber: number) => http.post<ApiResponse<PagedResult>>(`/Exercises/get-exercises-by-options?pageNumber=${pageNumber}&pageSize=10`, exerciseRequest)

export const GetAdminExercisesPaginatedByOptions = (exerciseRequest: ExerciseRequest, pageNumber: number) => http.post<ApiResponse<PagedResult>>(`/Exercises/get-admin-exercises-by-options?pageNumber=${pageNumber}&pageSize=10`, exerciseRequest)