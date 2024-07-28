import { ApiResponse } from "@/model/ApiResponse";
import { Chapter, ChapterAdd, CourseDetail, CourseRequest, CourseShowAdminDto, CourseType, Lesson, LessonAdd } from "@/model/Course";
import http from "@/util/http";

export const GetAllCourseType = () => http.get<ApiResponse<CourseType[]>>("CourseTypes");

export const AddNewCourseType = (data: Pick<CourseType, 'name'>) => http.post<ApiResponse<boolean>>("CourseTypes", data);

export const UpdateCourseType = (id: number, data: Pick<CourseType, 'name'>) => http.put<ApiResponse<boolean>>("CourseTypes/" + id, data);

export const DeleteCourseType = (id: number) => http.delete<ApiResponse<CourseType>>("CourseTypes/" + id);

export const AddNewCourse = (courseAdd: FormData) => http.post<ApiResponse<boolean>>("Courses", courseAdd, {
    headers: {
        'Content-Type': 'multipart/form-data'
    }
})

export const UpdateCourseById = (id: number, courseUpdate: FormData) => http.put<ApiResponse<boolean>>("Courses/" + id, courseUpdate, {
    headers: {
        'Content-Type': 'multipart/form-data'
    }
})

export const GetCourseByOptiosnInAdminPage = (courseRequest: CourseRequest) => http.post<ApiResponse<CourseShowAdminDto[]>>("Courses/get-course-by-options", courseRequest)

export const DeleteCourse = (id: number) => http.delete<ApiResponse<boolean>>("Courses/" + id)

export const GetCourseById = (id: number) => http.get<ApiResponse<CourseDetail>>("Courses/" + id)

export const GetChaptersByCourseId = (courseId: number) => http.get<ApiResponse<Chapter[]>>("Chapters/get-by-course-id/" + courseId)

export const DeleteChapter = (id: number) => http.delete<ApiResponse<boolean>>("Chapters/" + id)

export const AddChapter = (data: ChapterAdd) => http.post<ApiResponse<boolean>>("Chapters", data)

export const UpdateChapterById = (id: number, data: string) => http.put<ApiResponse<string>>("Chapters/" + id, data)

export const AddLesson = (data: LessonAdd) => http.post<ApiResponse<number>>("Lessons", data)

export const DeleteLesson = (id: number) => http.delete<ApiResponse<boolean>>("Lessons/" + id)

export const GetLessonByChapterId = (chapterId: number) => http.get<ApiResponse<Lesson[]>>("Lessons/get-lesson-by-chapter-id/" + chapterId)