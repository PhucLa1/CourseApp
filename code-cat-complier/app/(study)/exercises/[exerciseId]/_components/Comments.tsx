"use client"
import React, { useContext, useState } from 'react'
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from '../../../../../components/ui/button';
import { CommentExercise } from '@/model/Exercises';
import { useMutation } from '@tanstack/react-query';
import { PostCommentExercise } from '@/apis/exercises.api';
import Loading from '@/components/Loading';
import toast from 'react-hot-toast';
import { useCurrentUser } from '../../../../../util/CurrentUserProvider';
type Props = {
    commentExercise: CommentExercise[] | undefined,
    exerciseId: number,
    onAddComment: () => void
}
export default function Comments({ commentExercise, exerciseId, onAddComment }: Props) {
    const { currentUser } = useCurrentUser();
    const [value, setValue] = useState<string>('');
    const [isComment, setIsComment] = useState<boolean>(false)
    const { mutate, isPending } = useMutation({
        mutationFn: () => {
            console.log({
                content: value,
                exerciseId: exerciseId
            })
            return PostCommentExercise({
                content: value,
                exerciseId: exerciseId
            })
        },
        onSuccess: (data) => {
            if (data.data.isSuccess) {
                toast.success("Thêm bình luận thành công")
                onAddComment()
                setValue('')
                setIsComment(false)
            }
        }
    })
    console.log(currentUser)
    if (!commentExercise) return <></>
    return (
        <div className='h-full'>
            {isPending ? <Loading /> : <></>}
            <div className='flex items-start ml-5 mt-5'>
                <Avatar className='mt-2'>
                    <AvatarImage src="https://github.com/shadcn.png" />
                    <AvatarFallback>CN</AvatarFallback>
                </Avatar>
                {isComment ? <div className='w-full'>
                    <ReactQuill placeholder='Vui lòng nhập bình luận' theme="snow" value={value} onChange={(event) => setValue(event.toString())} className='h-[150px] w-full pl-3 pr-2' />
                    <div className=' w-2/5 h-[50px] rounded-md mt-12 ml-auto mr-2 flex items-end justify-between'>
                        <Button onClick={() => setIsComment(false)} className='bg-red-900 w-1/3 rounded-2xl' variant="destructive">Hủy</Button>
                        <Button onClick={() => mutate()} className='bg-green-900 w-3/4 ml-1 rounded-2xl' variant="secondary">Bình luận</Button>
                    </div>
                </div> : <div onClick={() => setIsComment(true)} className='ml-4 mt-5 border-b border-gray'>
                    <span className='text-sm text-gray-400'>Bạn có muốn bình luận gì về bài này không ?</span>
                </div>}
            </div>
            <div className='border border-gray rounded-md mt-5 h-[550px] w-full ml-2 mr-2 overflow-y-auto'>
                {commentExercise.map((item) => {
                    return <div className='mb-2'>
                        <div className="flex items-center justify-start ml-2 my-4">
                            <Avatar className='mt-2'>
                                <AvatarImage src={item.userAvatar} />
                                <AvatarFallback>CN</AvatarFallback>
                            </Avatar>
                            <span className='text-sm text-gray-400 mt-2 mx-2'>{item.userName}</span>
                        </div>
                        <div className='border border-gray rounded-md bg-gray-700 text-sm ml-1 mr-4 mt-2 pb-3 pl-3'>
                            <div dangerouslySetInnerHTML={{ __html: item.content }} className='leading-normal mt-3'></div>
                        </div>
                    </div>
                })}
            </div>
        </div>
    )
}
