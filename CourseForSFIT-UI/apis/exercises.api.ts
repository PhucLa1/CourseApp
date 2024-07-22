import { ApiResponse } from "@/model/ApiResponse";
import { AdminTagExercise, CommentExercise, CommentExerciseAdd, ContentCodes, ContentExercise, ExerciseCode, ExerciseDto, ExerciseRequest, ExerciseUpdate, TagExerciseAdd, TagExercises, TagExerciseUpdate, TestCase, TestCaseNotLock, TopicExercise } from "@/model/Exercises";
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
export const GetTopicExercise = (id: number) => http.get<ApiResponse<ContentExercise>>(`/Exercises/${id}`)

export const GetCommentExercise = (exerciseId: number) => http.get<ApiResponse<CommentExercise[]>>(`ExerciseComments/get-by-exercise-id/${exerciseId}`)

export const PostCommentExercise = (commentAddExercise: CommentExerciseAdd) => http.post<ApiResponse<CommentExercise[]>>(`ExerciseComments`, commentAddExercise)

export const GetExercisesPaginatedByOptions = (exerciseRequest: ExerciseRequest, pageNumber: number) => http.post<ApiResponse<PagedResult>>(`/Exercises/get-exercises-by-options?pageNumber=${pageNumber}&pageSize=10`, exerciseRequest)

export const GetAdminExercisesPaginatedByOptions = (exerciseRequest: ExerciseRequest, pageNumber: number) => http.post<ApiResponse<PagedResult>>(`/Exercises/get-admin-exercises-by-options?pageNumber=${pageNumber}&pageSize=10`, exerciseRequest)

export const GetTestCaseExerciseNotLock = (exerciseId: number) => http.get<ApiResponse<TestCaseNotLock>>("TestCase/get-test-cases-not-lock/" + exerciseId)

export const SolveTestCase = (data: ExerciseCode) => http.post<ApiResponse<boolean[]>>("TestCase/solve-test-case", data)

export const GetContentCodes = (exerciseId: number) => http.get<ApiResponse<ContentCodes>>("Exercises/get-content-code/" + exerciseId)

export const GetUserSubmision = (exerciseId: number, isMine: number, pageNumber: number) => http.get<ApiResponse<PagedResult>>("Exercises/get-user-submission/" + exerciseId + "?isMine=" + isMine + "&pageNumber=" + pageNumber + "&pageSize=10")

export const GetTagExerciseByExerciseId = (exerciseId: number) => http.get<ApiResponse<TagExercises>>("TagExercises/get-tags-exericse-by-exercise-id/" + exerciseId)

export const GetExerciseInfoAdmin = (exerciseId: number) => http.get<ApiResponse<TopicExercise>>("Exercises/get-exercise-info-admin/" + exerciseId)

export const UpdateExercise = (id: number, exerciseUpdate: ExerciseUpdate) => http.put<ApiResponse<boolean>>("Exercises/" + id, exerciseUpdate)

export const GetTestCase = (exerciseId: number) => http.get<ApiResponse<TestCase[]>>("TestCase/get-test-cases/" + exerciseId)

export const DeleteTestCase = (id: number) => http.delete<ApiResponse<boolean>>("TestCase/" + id)

export const UpdateTestCase = (id: number, testCaseUpdate: FormData) => http.put<ApiResponse<boolean>>("TestCase/" + id, testCaseUpdate, {
    headers: {
        'Content-Type': 'multipart/form-data'
    }
})

export const AddTestCase = (exerciseId: number, testCaseAdd: FormData) => http.post<ApiResponse<number>>("Exercises/" + exerciseId + "/test-cases", testCaseAdd,{
    headers: {
        'Content-Type': 'multipart/form-data'
    }
})