import { ApiResponse } from "@/model/ApiResponse";
import { CourseType } from "@/model/Course";
import http from "@/util/http";

export const GetAllCourseType = () => http.get<ApiResponse<CourseType[]>>("CourseTypes");

export const AddNewCourseType = (data: Pick<CourseType, 'typeName'>) => http.post<ApiResponse<boolean>>("CourseTypes", data);

export const UpdateCourseType = (id: number, data: Pick<CourseType, 'typeName'>) => http.put<ApiResponse<boolean>>("CourseTypes/" + id, data);

export const DeleteCourseType = (id: number) => http.delete<ApiResponse<CourseType>>("CourseTypes/" + id);
