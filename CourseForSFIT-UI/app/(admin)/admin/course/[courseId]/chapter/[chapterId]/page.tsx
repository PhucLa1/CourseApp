"use client"
import React, { useEffect, useState } from 'react'
import LessonAdd from './_components/LessonAdd';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { DeleteLesson, GetLessonByChapterId } from '@/apis/course.api';
import Loading from '@/components/Loading';
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { decimalToRoman } from '../../_components/CourseInfoDetail';
import toast from 'react-hot-toast';
export default function Page({ params }: { params: { chapterId: number } }) {
    const queryClient = useQueryClient()
    const { data: dataLesson, isLoading: isLoadingLesson } = useQuery({
        queryKey: ['lesson'],
        queryFn: () => GetLessonByChapterId(params.chapterId)
    })
    const { mutate: mutateDelete, isPending: isPendingDelete } = useMutation({
        mutationFn: (id: number) => {
            return DeleteLesson(id)
        }, 
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Xóa thành công bài học")
                queryClient.invalidateQueries({queryKey:['lesson']})
            } else {
                toast.error(data.data.message[0])
            }
        },
    })
    if (isLoadingLesson) return <Loading />
    return (
        <div className='w-full'>
            <LessonAdd chapterId={params.chapterId}/>
            <div className='mt-4' style={{ display: 'grid', gap: '40px 50px', gridTemplateColumns: 'repeat(2, 1fr)', paddingBottom: '10px', }}>
                {dataLesson?.data.metadata.map((item, index) => {
                    return <div key={index} className='bg-card transition-all  hover:shadow-lg hover:cursor-pointer' style={{ border: '1px solid #1f202a', borderRadius: '1rem', boxShadow: 'none', position: 'relative', display: 'flex', flexDirection: 'column', overflow: 'hidden', boxSizing: 'border-box', padding: '30px', minHeight: '50px', justifyContent: 'space-between', alignItems: 'flex-start', transition: 'background 0.1s ease-in-out 0s' }}>
                        <h3 className='w-full' style={{ overflow: 'hidden', maxHeight: '72.8px', display: '-webkit-box', WebkitBoxOrient: 'vertical', WebkitLineClamp: 2, position: 'relative', zIndex: 1, color: 'rgb(14, 20, 30)', fontWeight: 700, fontSize: '20px' }}>
                            <div className='flex items-center justify-between'>
                                <span style={{ paddingRight: '5px', color: '#eef4fc', display: 'inline', verticalAlign: 'middle', lineHeight: '1.4', fontWeight: 600 }}>Bài học {decimalToRoman(index + 1)}</span>
                                <div className='flex items-center justify-end'>
                                    <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[14px] text-gray-500 ' icon={faPen} />
                                    <AlertDialog>
                                        <AlertDialogTrigger asChild>
                                            <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[14px] ml-4 text-gray-500' icon={faTrash} />
                                        </AlertDialogTrigger>
                                        <AlertDialogContent>
                                            <AlertDialogHeader>
                                                <AlertDialogTitle>Bạn có chắc chắn muốn xóa bài học này?</AlertDialogTitle>
                                                <AlertDialogDescription>
                                                    Hành động này không thể được hoàn tác. Thao tác này sẽ xóa vĩnh viễn
                                                    tài khoản của bạn và xóa dữ liệu của bạn khỏi máy chủ của chúng tôi.
                                                </AlertDialogDescription>
                                            </AlertDialogHeader>
                                            <AlertDialogFooter>
                                                <AlertDialogCancel>Hủy</AlertDialogCancel>
                                                <AlertDialogAction onClick={() => mutateDelete(item.id)}>Tiếp tục</AlertDialogAction>
                                            </AlertDialogFooter>
                                        </AlertDialogContent>
                                    </AlertDialog>
                                </div>
                            </div>
                        </h3>
                        <div className='mt-2  w-full' style={{ display: 'inline' }}>
                            <div className='flex items-center justify-start w-full'>
                                <h2 style={{ color: '#c9c9cf', marginBottom: '10px', opacity: .75, textTransform: 'uppercase', lineHeight: 1.4, fontWeight: 400, letterSpacing: '.6px', fontSize: '12px' }}>{item.name}</h2>
                            </div>
                        </div>
                    </div>
                })}
            </div>
        </div>
    );
}
