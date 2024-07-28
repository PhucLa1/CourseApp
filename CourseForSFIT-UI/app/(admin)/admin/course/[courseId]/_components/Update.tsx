"use client"
import { UpdateChapterById } from '@/apis/course.api'
import { Chapter } from '@/model/Course'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import React, { useState } from 'react'
import toast from 'react-hot-toast'
import { Button } from "@/components/ui/button"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import Loading from '@/components/Loading'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPencil } from '@fortawesome/free-solid-svg-icons'
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
export default function Update({ chapter }: { chapter: Chapter}) {
    const queryClient = useQueryClient()
    const [value, setValue] = useState<string>(chapter.name)
    
    const { mutate: mutateUpdate, isPending: isPendingUpdate } = useMutation({
        mutationFn: ({ id, data }: { id: number, data: string }) => {
            return UpdateChapterById(id, data)
        },
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Sửa thành công chương học này")
                queryClient.invalidateQueries({ queryKey: ['chapter'] })
            } else {
                toast.error(data.data.message[0])
            }
            setValue(data.data.metadata)
        },
    })
    if (isPendingUpdate) return <Loading />
    return (
        <Dialog>
            <DialogTrigger asChild>
                <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[14px] text-gray-500 ml-4' icon={faPencil} />
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Chỉnh sửa chương học</DialogTitle>
                    <DialogDescription>
                        Chỉnh sửa chương học của bạn tại đây
                    </DialogDescription>
                </DialogHeader>
                <div className="grid gap-4 py-4">
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="name" className="text-right">
                            Tên chương học
                        </Label>
                        <Input
                            onChange={(event) => setValue(event.target.value)}
                            value={value}
                            id="name"
                            className="col-span-3"
                        />
                    </div>
                </div>
                <DialogFooter>
                    <Button onClick={() => mutateUpdate({ id: chapter.id, data: value })}>Chỉnh sửa</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}
