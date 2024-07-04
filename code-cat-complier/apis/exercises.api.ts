import { ApiResponse } from "@/model/ApiResponse";
import { ExerciseDto, TagExercises } from "@/model/Exercises";
import { PagedResult } from "@/model/PagedResult";
import http from "@/util/http";

export const GetAllTagExercises = () => http.get<ApiResponse<TagExercises>>("/Exercises")

export const GetExercisesPaginated = (pageNumber : number) => http.get<ApiResponse<PagedResult>>(`/Exercises/get-exercises?pageNumber=${pageNumber}&pageSize=10`)