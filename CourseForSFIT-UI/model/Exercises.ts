export type TagExercise = {
    id: number,
    name: string
}

export type AdminTagExercise = TagExercise & {
    createdAt: string,
    updatedAt: string
}

export type TagExerciseUpdate = {
    name: string
}
export type TagExerciseAdd = {
    name: string
}

export type Exercise = {
    id: number,
    name: string,
    difficultLevel: number
    numberParticipants: number
    successRate: number
}
export type AdminExercise = {
    id: number,
    name: string,
    difficultLevel: number
    numberParticipants: number
    successRate: number,
    tags: string[]
}

export type ExerciseRequest = {
    difficultLevel?: number[];
    tagId?: number[];
    status?: number;
    name?: string
}
export type CommentExercise = {
    content: string,
    userName: string,
    userAvatar: string
}
export type CommentExerciseAdd = {
    content: string,
    exerciseId: number
}

export type ContentExercise = {
    description: string;
    constraints: string;
    inputFormat: string;
    outputFormat: string;
    input: string[];
    output: string[];
    explaintation: string;
}


export type TestCase = {
    id:number;
    inputData?: string;
    exerciseId: number;
    expectedOutput: string;
    isLock: boolean
};
export type TestCaseNotLock = {
    testCaseDtos: TestCase[]
    totalTestCaseLockCounts: string[]
}

export type ExerciseCode = {
    exerciseId: number;
    contentCode: string;
    language: string;
    version: string;
    avatar:string
};

export type ContentCodes = {
    contentCode?: string;
    language?: string;
    version?: string;
    avatar?: string;
}

export type TopicExercise =  {
    topicExercise: ContentExercise
    name: string,
    difficult: number
}

export type ExerciseUpdate = TopicExercise & {
    tagIds?: number[]
}


export type ExerciseDto = Exercise

export type TagExercises = TagExercise[]