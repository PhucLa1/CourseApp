export type TagExercise = {
    id: number,
    tagName:string
}

export type AdminTagExercise = TagExercise & {
    createdAt: string,
    updatedAt: string
}

export type TagExerciseUpdate = {
    tagName: string
}
export type TagExerciseAdd = {
    tagName: string
}

export type Exercise = {
    id: number,
    exerciseName:string,
    difficultLevel:number
    numberParticipants:number
    successRate: number
}
export type AdminExercise = {
    id: number,
    exerciseName:string,
    difficultLevel:number
    numberParticipants:number
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
    content:string,
    userName:string,
    userAvatar: string
}
export type CommentExerciseAdd = {
    content:string,
    exerciseId: number
}

export type ExerciseDto = Exercise

export type TagExercises = TagExercise[]