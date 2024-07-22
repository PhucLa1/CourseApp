export type ApiResponse<T> = {
    isSuccess: boolean,
    metadata: T,
    message: string[]
}