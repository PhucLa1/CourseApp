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
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPen } from '@fortawesome/free-solid-svg-icons'
import { useMutation } from '@tanstack/react-query'
import Loading from '@/components/Loading'
import toast from 'react-hot-toast'
import { CourseType } from '@/model/Course'
import { UpdateCourseType } from '@/apis/course.api'

export default function Update({ courseType, onRerender }: { courseType: CourseType, onRerender: () => void }) {
    const [value, setValue] = useState<string>(courseType.typeName)
    const { mutate, isPending } = useMutation({
        mutationFn: () => {
            return UpdateCourseType(
                courseType.id,
                { typeName: value }
            )
        },
        onSuccess(data) {
            if (data.data.isSuccess) {
                onRerender()
                toast.success("Chỉnh sửa thành công")
            }
        },
    })
    if (isPending) return <Loading />
    return (
        <Dialog>
            <DialogTrigger asChild>
                <FontAwesomeIcon className='pl-4 cursor-pointer hover:text-slate-50' icon={faPen} />
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Chỉnh sửa loại khóa học</DialogTitle>
                    <DialogDescription>
                        Chỉnh sửa loại khóa học của bạn tại đây
                    </DialogDescription>
                </DialogHeader>
                <div className="grid gap-4 py-4">
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="name" className="text-right">
                            Tên loại
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
                    <Button onClick={() => mutate()}>Chỉnh sửa</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}
