"use client"
import React, { useState } from 'react'
import {
    Table,
    TableBody,
    TableCaption,
    TableCell,
    TableFooter,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"
import { useMutation, useQuery } from '@tanstack/react-query'
import { DeleteAdminTagExercises, GetAllAdminTagExercises } from '@/apis/exercises.api'
import Loading from '@/components/Loading'
import moment from 'moment';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons'
import DialogUpdate from './_components/Update'
import { AlertDialog } from '@/components/ui/alert-dialog'
import AlertDialogs from '@/components/AlertDialogs'
import DialogAdd from './_components/Add'
import toast from 'react-hot-toast'
import { DeleteCourseType, GetAllCourseType } from '@/apis/course.api'
import Update from './_components/Update'
import Add from './_components/Add'

export default function page() {
    const [action, setAction] = useState<number>(0)
    const { data: dataCourseType, isLoading } = useQuery({
        queryKey: ['course-type', action],
        queryFn: () => GetAllCourseType()
    })
    const { mutate: mutateDelete, isPending: isPendingDelete } = useMutation({
        mutationFn: (id: number) => {
            return DeleteCourseType(id)
        },
        onSuccess(data) {
            if (data.data.isSuccess) {
                setAction(action + 1)
                toast.success("Xóa loại khóa học thành công")
            }
        },
    })
    const onRerenderUpdate = () => {
        setAction(action + 1)
    }
    const onDelete = (id: number) => {
        mutateDelete(id)
    }
    const onAdd = () => {
        setAction(action + 1)
    }
    moment.locale('vi');
    return (
        <div className='w-full'>
            <div className='header flex items-center justify-between'>
                <h2 className='text-[20px] text-slate-50 font-bold'>Quản lí loại khóa học</h2>
                <Add onRerender={onAdd} />
            </div>
            <div className='content pb-2'>
                {isPendingDelete ? <Loading /> : <></>}
                {isLoading ? <Loading /> : <Table className='mt-2'>
                    <TableCaption>Danh sách loại khóa học</TableCaption>
                    <TableHeader>
                        <TableRow>
                            <TableHead className="w-[100px]">STT</TableHead>
                            <TableHead>Tên loại khóa học</TableHead>
                            <TableHead>Ngày tạo</TableHead>
                            <TableHead>Ngày sửa đổi</TableHead>
                            <TableHead className="text-right">Hành động</TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {dataCourseType?.data && dataCourseType?.data.metadata.map((item, index) => {
                            return <TableRow key={index}>
                                <TableCell className="font-medium">{index + 1}</TableCell>
                                <TableCell>{item.name}</TableCell>
                                <TableCell>{moment(item.createdAt).format('dddd, DD-MM-YYYY HH:mm:ss')}</TableCell>
                                <TableCell>{moment(item.updatedAt).format('dddd, DD-MM-YYYY HH:mm:ss')}</TableCell>
                                <TableCell className="text-center">
                                    <div className='flex items-center justify-center pl-2'>
                                        <Update onRerender={onRerenderUpdate} courseType={item} />
                                        <AlertDialogs
                                            trigger={<FontAwesomeIcon className='ml-6 cursor-pointer hover:text-slate-50' icon={faTrash} />}
                                            title="Bạn có chắc chắn muốn xóa loại khóa học này không"
                                            description='Sau khi thực hiện hành động này sẽ không thể khôi phục'
                                            actionTitle='Đồng ý'
                                            cancelTitle='Hủy'
                                            onAction={() => onDelete(item.id)} />
                                    </div>
                                </TableCell>
                            </TableRow>
                        })}
                    </TableBody>
                </Table>}

            </div>
        </div>
    )
}
