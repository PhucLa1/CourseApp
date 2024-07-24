import { CourseAdminDto } from '@/model/Course'
import { faPeace, faPen, faTrash } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React from 'react'
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
import { Button } from "@/components/ui/button"
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { DeleteCourse } from '@/apis/course.api'
import toast from 'react-hot-toast'
import Loading from '@/components/Loading'

export default function CourseAdmin({ courseInfo }: { courseInfo: CourseAdminDto }) {
    const queryClient = useQueryClient();
    const { mutate: mutateDelete, isPending: isPendingDelete } = useMutation({
        mutationFn: (id: number) => {
            return DeleteCourse(id)
        },
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Xóa khóa học thành công")
                queryClient.invalidateQueries({ queryKey: ['course'] });
            }
        },
    })
    return (
        <div className='transition-all hover:bg-gray-500 bg-[#1f202a] hover:-translate-y-4 hover:shadow-lg hover:cursor-pointer' style={{ border: '1px solid #1f202a', borderRadius: '1rem', boxShadow: 'none', position: 'relative', display: 'flex', flexDirection: 'column', overflow: 'hidden', boxSizing: 'border-box', padding: '30px', minHeight: '183px', justifyContent: 'space-between', alignItems: 'flex-start', transition: 'background 0.1s ease-in-out 0s' }}>
            <h3 style={{ overflow: 'hidden', maxHeight: '72.8px', display: '-webkit-box', WebkitBoxOrient: 'vertical', WebkitLineClamp: 2, position: 'relative', zIndex: 1, color: 'rgb(14, 20, 30)', fontWeight: 700, fontSize: '20px' }}>
                <span style={{ paddingRight: '5px', color: '#eef4fc', display: 'inline', verticalAlign: 'middle', lineHeight: '1.4', fontWeight: 600 }}>{courseInfo.courseName}</span>
            </h3>
            {isPendingDelete ?? <Loading/>}
            <div style={{ display: 'inline' }}>
                <h2 style={{ color: '#c9c9cf', marginBottom: '40px', opacity: .75, textTransform: 'uppercase', lineHeight: 1.4, fontWeight: 400, letterSpacing: '.6px', fontSize: '12px' }}>{courseInfo.createdByPerson}</h2>
            </div>
            <div className='flex items-center justify-end'>
                <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' icon={faPen} />
                <AlertDialog>
                    <AlertDialogTrigger asChild>
                        <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[18px] ml-2' icon={faTrash} />
                    </AlertDialogTrigger>
                    <AlertDialogContent>
                        <AlertDialogHeader>
                            <AlertDialogTitle>Bạn có chắc chắn muốn xóa khóa học này?</AlertDialogTitle>
                            <AlertDialogDescription>
                                Hành động này không thể được hoàn tác. Thao tác này sẽ xóa vĩnh viễn
                                tài khoản của bạn và xóa dữ liệu của bạn khỏi máy chủ của chúng tôi.
                            </AlertDialogDescription>
                        </AlertDialogHeader>
                        <AlertDialogFooter>
                            <AlertDialogCancel>Hủy</AlertDialogCancel>
                            <AlertDialogAction onClick={() => mutateDelete(courseInfo.id)}>Tiếp tục</AlertDialogAction>
                        </AlertDialogFooter>
                    </AlertDialogContent>
                </AlertDialog>
            </div>
            <img style={{ position: 'absolute', right: '-10%', bottom: '0px', height: '85%', opacity: '0.1', pointerEvents: 'none' }} src={`https://localhost:7130/Uploads/${courseInfo.thumbnail}`} alt="" />
        </div>
    )
}
