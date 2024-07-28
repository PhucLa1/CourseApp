"use client"
import React, { useEffect, useState } from 'react'
import { Drawer } from "antd";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAdd } from '@fortawesome/free-solid-svg-icons';
import { Button, Modal } from 'antd';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectLabel,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import { PlusOutlined } from '@ant-design/icons';
import { Image, Upload } from 'antd';
import type { GetProp, UploadFile, UploadProps } from 'antd';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { AddLesson } from '@/apis/course.api';
import toast from 'react-hot-toast';
import Loading from '@/components/Loading';
export default function LessonAdd({ chapterId }: { chapterId: number }) {
    const [isModalOpen, setIsModalOpen] = useState(false);

    const showModal = () => {
        setIsModalOpen(true);
    };
    const handleCancel = () => {
        setIsModalOpen(false);
    };
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [videoPreviewUrl, setVideoPreviewUrl] = useState<string | null>(null);
    const [name, setName] = useState<string>("")
    const queryClient = useQueryClient()
    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files && event.target.files[0];
        if (file) {
            setSelectedFile(file);
            const videoUrl = URL.createObjectURL(file);
            setVideoPreviewUrl(videoUrl);
        }
    };
    const { mutate: mutateAdd, isPending: isPendingAdd } = useMutation({
        mutationFn: () => {
            return AddLesson({
                name: name,
                chapterId: chapterId
            })
        }, onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Thêm thành công bài học")
                setName("")
                setVideoPreviewUrl(null)
                setSelectedFile(null)
                queryClient.invalidateQueries({queryKey:['lesson']})
                setIsModalOpen(false);
            } else {
                toast.error(data.data.message[0])
            }
        },
    })

    // Cleanup URL object when component is unmounted
    useEffect(() => {
        return () => {
            if (videoPreviewUrl) {
                URL.revokeObjectURL(videoPreviewUrl);
            }
        };
    }, [videoPreviewUrl]);
    return (
        <>
            <div className='header flex items-center justify-start'>
                <h2 className='text-[20px] text-slate-50 font-bold'>Chỉnh sửa chương học</h2>
                <FontAwesomeIcon onClick={showModal} className='hover:text-slate-50 cursor-pointer text-[18px] ml-4 text-gray-500' icon={faAdd} />
            </div>
            {isPendingAdd ? <Loading/> : <></>}
            <Modal okText='Đồng ý' cancelText='Hủy' title="Thêm mới bài học" open={isModalOpen} onOk={() => mutateAdd()} onCancel={handleCancel}>
                <div className='w-full h-full border border-gray-500 rounded-md bg-card p-4 text-gray-200'>
                    <div className="grid w-full max-w-sm items-center gap-1.5 mt-4">
                        <Label htmlFor="picture">Tên bài học</Label>
                        <Input value={name} onChange={(e) => setName(e.target.value)} className='mt-2 w-full' type="text" placeholder='Nhập tên bài học' />
                    </div>
                    <div className="grid w-full max-w-sm items-center gap-1.5 mt-4">
                        <Label htmlFor="picture">Loại bài học</Label>
                        <Select value='2'>
                            <SelectTrigger className="w-full z-50">
                                <SelectValue placeholder="Chọn định dạng bài học" />
                            </SelectTrigger>
                            <SelectContent style={{ zIndex: 9999 }}>
                                <SelectGroup>
                                    <SelectLabel>Định dạng bài học</SelectLabel>
                                    <SelectItem disabled value="1">Văn bản</SelectItem>
                                    <SelectItem value="2">Video</SelectItem>
                                </SelectGroup>
                            </SelectContent>
                        </Select>
                    </div>
                    <div className="grid w-full max-w-sm items-center gap-1.5 mt-4">
                        <Label htmlFor="picture">Nội dung bài học</Label>
                        <div className='py-4' onClick={() => document.getElementById('file')?.click()}>
                            <div style={{ minHeight: '150px', border: '1px dashed #fff', borderWidth: '2px', padding: '20px', display: 'flex', alignItems: 'center', justifyContent: 'center', borderRadius: '7px', marginBottom: '0.5rem' }} className="w-full p-2 bg-card cursor-pointer">
                                <div>
                                    {videoPreviewUrl == null ? "Nhấn vào đây để chọn tệp ảnh cho khóa học" : <video width="300" controls>
                                        <source src={videoPreviewUrl} type="video/mp4" />
                                        Ứng dụng không hỗ trợ.
                                    </video>}
                                </div>
                                <Input onChange={(e) => handleFileChange(e)} accept='.mp4' type="file" id='file' className='w-full mt-2 rounded-md hidden' placeholder="Nhập tên khóa học" />
                            </div>
                            <p className='text-center' style={{ fontSize: '0.75rem' }}>Chọn video có đuôi .mp4</p>
                        </div>
                    </div>
                </div>
            </Modal>
        </>

    )
}
