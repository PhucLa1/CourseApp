
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
import { faAdd } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
import { useMutation } from '@tanstack/react-query'
import { AddTagExercises } from '@/apis/exercises.api'
import toast from 'react-hot-toast'
import Loading from '@/components/Loading'
export default function DialogAdd({ onRerender }: { onRerender: () => void }) {
    const [value, setValue] = useState<string>("")
    const { mutate, isPending } = useMutation({
        mutationFn: () => {
            return AddTagExercises({ tagName: value })
        },
        onSuccess(data) {
            if (data.data.isSuccess) {
                onRerender()
                setValue("")
                toast.success("Thêm thành công")
            }
        },
    })
    if (isPending) return <Loading />
    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button variant='default'>Thêm nhãn dán</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Thêm nhãn dán</DialogTitle>
                    <DialogDescription>
                        Thêm nhãn dán của bạn tại đây
                    </DialogDescription>
                </DialogHeader>
                <div className="grid gap-4 py-4">
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="name" className="text-right">
                            Tên nhãn
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
