import { AddChapter, DeleteChapter, GetChaptersByCourseId, UpdateChapterById } from '@/apis/course.api';
import Loading from '@/components/Loading';
import { faAdd, faPeace, faPen, faPencil, faSave, faTrash } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { ppid } from 'process';
import React, { useEffect, useState } from 'react'
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
import toast from 'react-hot-toast';
import Link from 'next/link';
import Add from './Add';
import Update from './Update';
export const decimalToRoman = (num: number) => {
    const romanNumerals: { [key: number]: string } = {
        1000: 'M',
        900: 'CM',
        500: 'D',
        400: 'CD',
        100: 'C',
        90: 'XC',
        50: 'L',
        40: 'XL',
        10: 'X',
        9: 'IX',
        5: 'V',
        4: 'IV',
        1: 'I'
    };

    let result = '';
    for (const value in romanNumerals) {
        while (num >= parseInt(value)) {
            result += romanNumerals[value];
            num -= parseInt(value);
        }
    }

    return result;
}

export default function CourseInfoDetail({ courseId }: { courseId: number }) {
    const queryClient = useQueryClient()
    const { data: dataChapter, isLoading: isLoadingChapter } = useQuery({
        queryKey: ['chapter'],
        queryFn: () => GetChaptersByCourseId(courseId)
    })
    const { mutate: mutateDelete, isPending: isPendingDelete } = useMutation({
        mutationFn: (id: number) => DeleteChapter(id),
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Đã xóa thành công chương học này")
                queryClient.invalidateQueries({ queryKey: ['chapter'] })
            }
        },
    })

    return (
        <div className='mt-10'>
            <div className='header flex items-center justify-start'>
                <h2 className='text-[20px] text-slate-50 font-bold'>Thêm mới chương</h2>
                <Add courseId={courseId} />
            </div>
            <div className='mt-4' style={{ display: 'grid', gap: '40px 50px', gridTemplateColumns: 'repeat(2, 1fr)', paddingBottom: '10px', }}>
                {isLoadingChapter ? <Loading /> : <></>}
                {isPendingDelete ? <Loading /> : <></>}
                {dataChapter?.data.metadata.map((item, index) => {
                    return <div key={index} className='bg-card transition-all  hover:shadow-lg hover:cursor-pointer' style={{ border: '1px solid #1f202a', borderRadius: '1rem', boxShadow: 'none', position: 'relative', display: 'flex', flexDirection: 'column', overflow: 'hidden', boxSizing: 'border-box', padding: '30px', minHeight: '50px', justifyContent: 'space-between', alignItems: 'flex-start', transition: 'background 0.1s ease-in-out 0s' }}>
                        <h3 className='w-full' style={{ overflow: 'hidden', maxHeight: '72.8px', display: '-webkit-box', WebkitBoxOrient: 'vertical', WebkitLineClamp: 2, position: 'relative', zIndex: 1, color: 'rgb(14, 20, 30)', fontWeight: 700, fontSize: '20px' }}>
                            <div className='flex items-center justify-between'>
                                <span style={{ paddingRight: '5px', color: '#eef4fc', display: 'inline', verticalAlign: 'middle', lineHeight: '1.4', fontWeight: 600 }}>Chương {decimalToRoman(index + 1)}</span>
                                <div className='flex items-center justify-end'>
                                    <Link href={`${courseId}/chapter/${item.id}`}><FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[14px] text-gray-500 ' icon={faPen} /></Link>
                                    <AlertDialog>
                                        <AlertDialogTrigger asChild>
                                            <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[14px] ml-4 text-gray-500' icon={faTrash} />
                                        </AlertDialogTrigger>
                                        <AlertDialogContent>
                                            <AlertDialogHeader>
                                                <AlertDialogTitle>Bạn có chắc chắn muốn xóa chương này?</AlertDialogTitle>
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
                                <Update chapter={item} />
                            </div>

                        </div>
                    </div>
                })}

            </div>
        </div>

    )
}
