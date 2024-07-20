import ReactSelect from '@/components/ReactSelect'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from '@/components/ui/select'
import { TagExercises } from '@/model/Exercises'
import React, { useState } from 'react'

type Tag = {
    label:string,
    value:number
}
type Props = {
    tags: Tag[],
    exerciseName: string,
    difficult: number,
    defaultValueTag: Tag[],
    onSetExerciseName:(value:string) => void,
    onSetDifficult:(value:number) => void,
    onHandleTagIds:(data: number[]) => void
}

export default function FirstArticle({ tags, exerciseName, difficult, defaultValueTag,onSetExerciseName,onHandleTagIds,onSetDifficult }: Props) {
    return (
        <div className='flex items-center justify-between'>
            <div className="grid w-full max-w-sm items-center gap-1.5">
                <Label className='ml-2 mb-2' htmlFor="picture">Nhập tên bài tập</Label>
                <Input value={exerciseName} onChange={(e) => onSetExerciseName(e.target.value)} placeholder='Nhập tên bài tập vào đây' type="text" />
            </div>
            <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                <Label className='ml-2 mb-2' htmlFor="picture">Độ khó</Label>
                <Select value={difficult?.toString()} onValueChange={(e) => onSetDifficult(parseInt(e, 10))}>
                    <SelectTrigger className="w-[180px]">
                        <SelectValue placeholder="Lựa chọn loại" />
                    </SelectTrigger>
                    <SelectContent>
                        <SelectGroup>
                            <SelectLabel>Lựa chọn độ khó</SelectLabel>
                            <SelectItem className='text-[#7bc043]' value="1">Dễ</SelectItem>
                            <SelectItem className='text-[#faa05e]' value="2">Trung bình</SelectItem>
                            <SelectItem className='text-[#e64f4a]' value="3">Khó</SelectItem>
                        </SelectGroup>
                    </SelectContent>
                </Select>
            </div>
            <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                <Label className='ml-2 mb-2' htmlFor="picture">Lựa chọn nhãn dán</Label>
                <ReactSelect defaultValue={defaultValueTag}  onHandleTagIds={onHandleTagIds} value={tags} />
            </div>
        </div>
    )
}
