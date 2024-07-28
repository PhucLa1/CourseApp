"use client"
import React, { useState } from 'react'
import MenuBar from './_components/MenuBar'
import UpdateCourse from './_components/UpdateCourse'
import CourseInfoDetail from './_components/CourseInfoDetail'

export default function page({ params }: { params: { courseId: number } }) {
    const [show, setShow] = useState<number>(1)
    const onSetShow = (data: number) => {setShow(data)}
    return (
        <div className='w-full'>
            <MenuBar onSetShow={onSetShow}></MenuBar>
            {show == 1 && <UpdateCourse courseId={params.courseId} />}
            {show == 2 && <CourseInfoDetail courseId={params.courseId}/>}
        </div>
    )
}
