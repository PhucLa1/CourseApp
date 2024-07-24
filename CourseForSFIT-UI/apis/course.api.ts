import { ApiResponse } from "@/model/ApiResponse";
import { CourseRequest, CourseShowAdminDto, CourseType } from "@/model/Course";
import http from "@/util/http";

export const GetAllCourseType = () => http.get<ApiResponse<CourseType[]>>("CourseTypes");

export const AddNewCourseType = (data: Pick<CourseType, 'typeName'>) => http.post<ApiResponse<boolean>>("CourseTypes", data);

export const UpdateCourseType = (id: number, data: Pick<CourseType, 'typeName'>) => http.put<ApiResponse<boolean>>("CourseTypes/" + id, data);

export const DeleteCourseType = (id: number) => http.delete<ApiResponse<CourseType>>("CourseTypes/" + id);

export const AddNewCourse = (courseAdd: FormData) => http.post<ApiResponse<boolean>>("Courses", courseAdd, {
    headers: {
        'Content-Type': 'multipart/form-data'
    }
})

export const GetCourseByOptiosnInAdminPage = (courseRequest: CourseRequest) => http.post<ApiResponse<CourseShowAdminDto[]>>("Courses/get-course-by-options", courseRequest)

export const DeleteCourse = (id: number) => http.delete<ApiResponse<boolean>>("Courses/" + id)
