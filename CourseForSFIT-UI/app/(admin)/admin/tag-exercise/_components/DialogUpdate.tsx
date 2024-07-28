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
import { AdminTagExercise, TagExerciseUpdate } from '@/model/Exercises'
import { eventNames } from 'process'
import { useMutation } from '@tanstack/react-query'
import { UpdateAdminTagExercises } from '@/apis/exercises.api'
import Loading from '@/components/Loading'
import toast from 'react-hot-toast'
export default function DialogUpdate({ adminTagExercise, onRerender }: { adminTagExercise: AdminTagExercise, onRerender: () => void }) {
  const [value, setValue] = useState<string>(adminTagExercise.name)
  const { mutate, isPending } = useMutation({
    mutationFn: () => {
      return UpdateAdminTagExercises(
        adminTagExercise.id,
        { name: value }
      )
    },
    onSuccess(data) {
      if (data.data.isSuccess) {  
        toast.success("Chỉnh sửa thành công")
      }else{
        toast.error(data.data.message[0])
      }
      onRerender()
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
          <DialogTitle>Chỉnh sửa nhãn dán</DialogTitle>
          <DialogDescription>
            Chỉnh sửa nhãn dán của bạn tại đây
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
          <Button onClick={() => mutate()}>Chỉnh sửa</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
