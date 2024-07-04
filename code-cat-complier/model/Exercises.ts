export type TagExercise = {
    id: number,
    tagName:string
}

export type Exercise = {
    id: number,
    exerciseName:string,
    difficultLevel:number
    numberParticipants:number
    successRate: number
}

export type ExerciseDto = Exercise

export type TagExercises = TagExercise[]