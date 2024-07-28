"use client"
import React, { useState } from 'react'
import Course from './_components/CourseAdmin'
import { useQuery } from '@tanstack/react-query'
import { GetAllCourseType, GetCourseByOptiosnInAdminPage } from '@/apis/course.api'
import CourseAdmin from './_components/CourseAdmin'
import Loading from '@/components/Loading'
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectLabel,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import { Checkbox } from '@/components/ui/checkbox'
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from '@/components/ui/dropdown-menu'
import { Button } from '@/components/ui/button'
import ReactSelect from '@/components/ReactSelect'

export default function page() {
    const [hidden, setHidden] = useState<boolean>(true)
    const [listId, setListId] = useState<number[]>([])
    const [courseName, setCourseName] = useState<string>("")
    const addId = (id: number) => {
        setListId((prevList) => [...prevList, id]);
    };

    const removeId = (id: number) => {
        setListId((prevList) => prevList.filter(item => item !== id));
    };
    const { data: dataCourse, isLoading: isLoadingCourse } = useQuery({
        queryKey: ['course', listId, courseName],
        queryFn: () => GetCourseByOptiosnInAdminPage({
            courseTypeId: listId,
            name: courseName
        })
    })
    const { data: dataCourseType, isLoading: isLoadingCourseType } = useQuery({
        queryKey: ['course-type'],
        queryFn: () => GetAllCourseType()
    })
    const handleCheck = (checked: boolean, id: number) => {
        if (checked == true) {
            setListId((prevList) => [...prevList, id]);
        } else {
            setListId((prevList) => prevList.filter(item => item !== id));
        }
    }
    return (
        <div className='w-full'>
            <div className='header flex items-center justify-between'>
                <h2 className='text-[20px] text-slate-50 font-bold'>Danh sách khóa học</h2>
            </div>
            <section>
                <div className='flex items-start justify-start'>
                    <div className="grid w-full max-w-sm items-center gap-1.5 mt-4">
                        <Label htmlFor="picture">Tên khóa học</Label>
                        <Input value={courseName} onChange={(e) => setCourseName(e.target.value)} type="text" placeholder='Nhập tên khóa học cần tìm kiếm' />
                    </div>
                    <div className="grid w-full max-w-sm items-center gap-1.5 mt-4 ml-2">
                        <Label htmlFor="picture">Tên loại khóa học</Label>
                        <Input onClick={() => setHidden(!hidden)} readOnly={true} value='Chọn loại khóa học' id="picture" type="text" placeholder='' />
                        <div onClick={() => setHidden(!hidden)} className='h-[200px] rounded-md overflow-y-auto bg-card p-8 w-[380px]' style={{ position:'absolute',top: '24%',zIndex: 99, display: `${hidden == true ? 'none' : 'block'}` }}>
                            {dataCourseType?.data.metadata.map((item, index) => {
                                return <div key={index} className="flex items-center space-x-2 border-b border-gray-500 mt-4">
                                    <Checkbox onCheckedChange={(e) => handleCheck(e as boolean, item.id)} className='mb-2' id="terms" />
                                    <label htmlFor="terms" className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 pb-2">{item.name}</label>
                                </div>
                            })}
                        </div>
                    </div>
                </div>
                {isLoadingCourse ?? <Loading />}
                {dataCourse?.data.metadata.map((item, index) => {
                    return <section key={index}>
                        <div className='flex mt-10 mb-5 items-baseline'>
                            <h2 style={{ fontSize: '18px', lineHeight: 1.4, fontWeight: 700 }}>{item.name}</h2>
                        </div>
                        <div style={{ display: 'grid', gap: '40px 50px', gridTemplateColumns: 'repeat(3, 1fr)', paddingBottom: '10px', }}>
                            {item.courseAdminDtos?.map((item, index) => {
                                return <CourseAdmin courseInfo={item} key={index} />
                            })}
                        </div>
                    </section>
                })}

            </section>
        </div>
    )
}
