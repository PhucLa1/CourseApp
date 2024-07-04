import { Checkbox } from "@/components/ui/checkbox"
import { TagExercise, TagExercises } from "@/model/Exercises"
import { Label } from "@radix-ui/react-dropdown-menu"
import { title } from "process"
import React from 'react'
import toast from "react-hot-toast"

type Filter = {
    title: string,
    value: LabelValue[]
}
type LabelValue = {
    label: string,
    color?: string
}

const FilterData: Filter[] = [
    {
        title: 'Trạng thái',
        value: [
            { label: 'Đã giải' },
            { label: 'Đang giải' },
            { label: 'Chưa giải' },
            { label: 'Đã lưu' }
        ]
    },
    {
        title: 'Độ khó',
        value: [
            { label: 'Dễ', color: '#7bc043' },
            { label: 'Trung bình', color: '#faa05e' },
            { label: 'Khó', color: '#e64f4a' }
        ]
    }
]

export default function TagFilter({ tagExercises }: { tagExercises: TagExercises }) {
    return (
        <section className='round-md mt-9' style={{ fontWeight: 500, background: 'none' }}>
            {
                FilterData.map((item, index) => {
                    return <div key={index} style={{ borderBottom: '1px solid #1d2432', paddingTop: 0, padding: '20px 0' }}>
                        <div style={{ marginBottom: '10px', color: '#c1c2d6', textTransform: 'uppercase', boxSizing: 'inherit' }}>{item.title}</div>
                        <div>
                            <div style={{ boxSizing: 'inherit' }}>
                                {
                                    item.value.map((subItem, subIndex) => {
                                        return <div key={subIndex} className="flex items-center space-x-2 mt-4">
                                            <Checkbox id="terms" />
                                            <label
                                                htmlFor="terms"
                                                className={`text-[${subItem.color == null ? '' : subItem.color}] text-2sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70`}>
                                                {subItem.label}
                                            </label>
                                        </div>
                                    })
                                }
                            </div>
                        </div>
                    </div>
                })
            }
            <div style={{ borderBottom: '1px solid #1d2432', paddingTop: 0, padding: '20px 0' }}>
                <div style={{ marginBottom: '10px', color: '#c1c2d6', textTransform: 'uppercase', boxSizing: 'inherit' }}>Thể loại</div>
                <div>
                    <div style={{ boxSizing: 'inherit' }}>
                        {
                            tagExercises.map((item) => {
                                return <div key={item.id} className="flex items-center space-x-2 mt-4">
                                    <Checkbox id="terms" />
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
