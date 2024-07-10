import { ApiResponse } from "@/model/ApiResponse";
import { AdminTagExercise, CommentExercise, CommentExerciseAdd, ExerciseDto, ExerciseRequest, TagExerciseAdd, TagExercises, TagExerciseUpdate } from "@/model/Exercises";
import { PagedResult } from "@/model/PagedResult";
import http from "@/util/http";

export const GetAllTagExercises = () => http.get<ApiResponse<TagExercises>>("/TagExercises")

export const GetAllAdminTagExercises = () => http.get<ApiResponse<AdminTagExercise[]>>("/TagExercises/admin");

export const UpdateAdminTagExercises = (id: number, tagExerciseUpdate: TagExerciseUpdate) => http.put<ApiResponse<boolean>>(`/TagExercises/admin/${id}`, tagExerciseUpdate);

export const DeleteAdminTagExercises = (id: number) => http.delete<ApiResponse<boolean>>(`/TagExercises/admin/${id}`);

export const AddTagExercises = (tagExerciseAdd : TagExerciseAdd) => http.post<ApiResponse<boolean>>("TagExercises/admin",tagExerciseAdd)

export const GetTopicExercise = (id: number) => http.get<ApiResponse<string>>(`/Exercises/${id}`)

export const GetCommentExercise = (exerciseId: number) => http.get<ApiResponse<CommentExercise[]>>(`ExerciseComment/get-by-exercise-id/${exerciseId}`)

export const PostCommentExercise = (commentAddExercise: CommentExerciseAdd) => http.post<ApiResponse<CommentExercise[]>>(`ExerciseComment`, commentAddExercise)

export const GetExercisesPaginatedByOptions = (exerciseRequest: ExerciseRequest, pageNumber: number) => http.post<ApiResponse<PagedResult>>(`/Exercises/get-exercises-by-options?pageNumber=${pageNumber}&pageSize=10`, exerciseRequest)