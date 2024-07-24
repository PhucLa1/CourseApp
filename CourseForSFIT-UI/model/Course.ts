export type CourseType = {
    id: number;
    typeName: string,
    createdAt: string,
    updatedAt: string
}

export type CourseShowAdminDto = {
    id: number;
    typeName?: string;
    courseAdminDtos?: CourseAdminDto[];
}

export type CourseAdminDto = {
    id: number;
    thumbnail?: string;
    courseName: string;
    createdByPerson?: string;
}

export type CourseRequest = {
    courseTypeId?: number[];
    courseName?: string;
}