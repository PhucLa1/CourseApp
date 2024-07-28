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
import { useMutation } from '@tanstack/react-query'
import toast from 'react-hot-toast'
import Loading from '@/components/Loading'
import { AddNewCourseType } from '@/apis/course.api'


export default function Add({ onRerender }: { onRerender: () => void }) {
    const [value, setValue] = useState<string>("")
    const { mutate, isPending } = useMutation({
        mutationFn: () => {
            return AddNewCourseType({ name: value })
        },
        onSuccess(data) {
            if (data.data.isSuccess) {
                toast.success("Thêm thành công")
            }else{
                toast.error(data.data.message[0])
            }             
            onRerender()
            setValue("")
        },
    })
    if (isPending) return <Loading />
    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button variant='default'>Thêm loại khóa học</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Thêm loại khóa học</DialogTitle>
                    <DialogDescription>
                        Thêm loại khóa học của bạn tại đây
                    </DialogDescription>
                </DialogHeader>
                <div className="grid gap-4 py-4">
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="name" className="text-right">
                            Tên loại khóa học
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
                    <Button onClick={() => mutate()}>Thêm</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}
