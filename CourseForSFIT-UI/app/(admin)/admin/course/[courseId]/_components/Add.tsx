"use client"
import React, { useState } from 'react'
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
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import toast from 'react-hot-toast'
import Loading from '@/components/Loading'
import { ChapterAdd } from '@/model/Course'
import { AddChapter } from '@/apis/course.api'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faAdd } from '@fortawesome/free-solid-svg-icons'
export default function Add({ courseId }: { courseId: number }) {
    const queryClient = useQueryClient()
    const { mutate: mutateAdd, isPending: isPendingAdd } = useMutation({
        mutationFn: (data: ChapterAdd) => AddChapter(data),
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Thêm thành công chương học này")
                queryClient.invalidateQueries({ queryKey: ['chapter'] })
                
            }else{
                toast.error(data.data.message[0])
            }
            setValue("")
        },
    })
    const [value, setValue] = useState<string>("")
    if (isPendingAdd) return <Loading />
    return (
        <Dialog>
            <DialogTrigger asChild>
                <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[18px] ml-6' icon={faAdd} />
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Thêm mới chương học</DialogTitle>
                    <DialogDescription>
                        Thêm loại chương học của bạn tại đây
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
                    <Button onClick={() => mutateAdd({ courseId: courseId, name: value })}>Thêm</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}
