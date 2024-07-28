export type CourseType = {
    id: number;
    name: string,
    createdAt: string,
    updatedAt: string
}

export type CourseShowAdminDto = {
    id: number;
    name?: string;
    courseAdminDtos?: CourseAdminDto[];
}

export type CourseAdminDto = {
    id: number;
    thumbnail?: string;
    name: string;
    createdByPerson?: string;
}

export type CourseRequest = {
    courseTypeId?: number[];
    name?: string;
}

export type CourseDetail = {
    name: string; // required
    description: string; // required
    listLearnAbout?: string[]; // optional
    listPrepared?: string[]; // optional
    thumbnail?: string; // optional
    status: number; // 1: Draft, 2: Public
    courseTypeId: number;
}

export type Chapter = {
    id: number,
    name: string
}

export type ChapterAdd = {
    courseId: number,
    name: string
}

export type Lesson = {
    id: number,
    name: string,
    chunkIndex?: number,
    totalChunk?: number,
    statusUpload?:number
}

export type LessonAdd = {
    name: string,
    chapterId: number
}