"use client"
import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox"
import { Input } from "@/components/ui/input"
import { TagExercises } from "@/model/Exercises"
import { useEffect, useRef, useState } from "react"

const status = ['Đã giải', 'Đang giải', 'Chưa giải', 'Đã lưu']

const difficultLevel = [
    { label: 'Dễ', color: '#7bc043' },
    { label: 'Trung bình', color: '#faa05e' },
    { label: 'Khó', color: '#e64f4a' }
]

type Props = {
    tagExercises: TagExercises,
    onChooseFilter: (option: number, value: any, numerical: number) => void,
    isChecked: boolean[],
    onFindByName: (value: string) => void,
    name: string
}

export default function TagFilter({ tagExercises, onChooseFilter, isChecked, onFindByName, name }: Props) {
    const inputRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (inputRef.current) {
            inputRef.current.focus();
        }
    }, []);
    return (
        <section className='round-md mt-9' style={{ fontWeight: 500, background: 'none' }}>
            <div className="flex items-center justify-start mr-2">
                <Input ref={inputRef} onChange={(event) => onFindByName(event.target.value)} value={name} placeholder="Nhập tên bài muốn tìm" />
                <Button className="ml-2">Tìm kiếm</Button>
            </div>

            <div style={{ borderBottom: '1px solid #1d2432', paddingTop: 0, padding: '20px 0' }}>
                <div style={{ marginBottom: '10px', color: '#c1c2d6', textTransform: 'uppercase', boxSizing: 'inherit' }}>Độ khó</div>
                <div>
                    <div style={{ boxSizing: 'inherit' }}>
                        {
                            status.map((item, index) => {
                                return <div key={index} className="flex items-center space-x-2 mt-4">
                                    <Checkbox checked={isChecked[index] || false} onCheckedChange={() => onChooseFilter(0, index + 1, index)} id="terms" />
                                    <label
                                        htmlFor="terms"
                                        className={`text-2sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70`}>
                                        {item}
                                    </label>
                                </div>
                            })
                        }
                    </div>
                </div>
            </div>
            <div style={{ borderBottom: '1px solid #1d2432', paddingTop: 0, padding: '20px 0' }}>
                <div style={{ marginBottom: '10px', color: '#c1c2d6', textTransform: 'uppercase', boxSizing: 'inherit' }}>Độ khó</div>
                <div>
                    <div style={{ boxSizing: 'inherit' }}>
                        {
                            difficultLevel.map((item, index) => {
                                return <div key={index} className="flex items-center space-x-2 mt-4">
                                    <Checkbox checked={isChecked[status.length + index] || false} onCheckedChange={() => onChooseFilter(1, index + 1, status.length + index)} id="terms" />
                                    <label
                                        htmlFor="terms"
                                        className={`text-[${item.color}] text-2sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70`}>
                                        {item.label}
                                    </label>
                                </div>
                            })
                        }
                    </div>
                </div>
            </div>
            <div style={{ borderBottom: '1px solid #1d2432', paddingTop: 0, padding: '20px 0' }}>
                <div style={{ marginBottom: '10px', color: '#c1c2d6', textTransform: 'uppercase', boxSizing: 'inherit' }}>Thể loại</div>
                <div>
                    <div style={{ boxSizing: 'inherit' }}>
                        {
                            tagExercises.map((item, index) => {
                                return <div key={index} className="flex items-center space-x-2 mt-4">
                                    <Checkbox checked={isChecked[status.length + difficultLevel.length + index] || false} onCheckedChange={() => onChooseFilter(2, item.id.toString(), status.length + difficultLevel.length + index)} id="terms" />
                                    <label
                                        htmlFor="terms"
                                        className={`text-2sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70`}>
                                        {item.tagName}
                                    </label>
                                </div>
                            })
                        }
                    </div>
                </div>
            </div>
        </section>
    )
}
