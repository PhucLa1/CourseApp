"use client"
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import React, { useState } from 'react'
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectLabel,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faAdd, faBook, faTrash } from '@fortawesome/free-solid-svg-icons'
import { Button } from '@/components/ui/button'
import { useMutation, useQuery } from '@tanstack/react-query'
import { AddNewCourse, GetAllCourseType } from '@/apis/course.api'
import toast from 'react-hot-toast'
import { Value } from '@radix-ui/react-select'
import Loading from '@/components/Loading'
type ThumbnailProps = {
    src: string;
    alt: string;
}
export default function page() {
    const [courseType, setCourseType] = useState<string | undefined>(undefined);
    const [status, setStatus] = useState<string | undefined>(undefined);
    const [courseName, setCoureName] = useState<string>("")
    const [description, setDescription] = useState<string>("")
    const [learnAbout, setLearnAbout] = useState<boolean[]>([true])
    const [prepared, setPrepared] = useState<boolean[]>([true])
    const [listLearnAbout, setListLearnAbout] = useState<string[]>([""])
    const [listPrepared, setListPrepared] = useState<string[]>([""])
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [imagePreviewUrl, setImagePreviewUrl] = useState<string | null>(null);
    const addElement = (option: number) => {
        if (option == 1) {
            setLearnAbout([...learnAbout, false]);
            setListLearnAbout([...listLearnAbout, ""]);
        } else {
            setPrepared([...prepared, false]);
            setListPrepared([...listPrepared, ""]);
        }
    };
    const resetStates = () => {
        setCourseType(undefined);
        setStatus(undefined);
        setCoureName("");
        setDescription("");
        setLearnAbout([true]);
        setPrepared([true]);
        setListLearnAbout([""]);
        setListPrepared([""]);
        setSelectedFile(null);
        setImagePreviewUrl(null);
      };
    const changeLearnAboutValue = (index: number, newValue: string) => {
        const updatedList = [...listLearnAbout];
        updatedList[index] = newValue;
        setListLearnAbout(updatedList);
    };
    const handleChangeSelect = (opt: number, value: string) => {
        if (opt == 1) {
            setStatus(value);
        } else {
            setCourseType(value);
        }

    };

    const changePreparedValue = (index: number, newValue: string) => {
        const updatedList = [...listPrepared];
        updatedList[index] = newValue;
        setListPrepared(updatedList);
    };

    // Hàm xóa phần tử theo chỉ số
    const removeElement = (option: number, index: number) => {
        if (option == 1) {
            setLearnAbout(learnAbout.filter((_, i) => i !== index));
            setListLearnAbout(listLearnAbout.filter((_, i) => i !== index));
        } else {
            setPrepared(prepared.filter((_, i) => i !== index));
            setListPrepared(listPrepared.filter((_, i) => i !== index));
        }
    };

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files && event.target.files[0];
        if (file) {
            setSelectedFile(file);
            const reader = new FileReader();
            reader.onloadend = () => {
                setImagePreviewUrl(reader.result as string);
            };
            reader.readAsDataURL(file);
        }
    };

    //Query
    const { data: dataCourseType, isLoading: isLoadingCourseType } = useQuery({
        queryKey: ['course-type'],
        queryFn: () => {
            return GetAllCourseType();
        }
    })
    const { mutate, isPending } = useMutation({
        mutationFn: (data: FormData) => {
            return AddNewCourse(data)
        },
        onSuccess(data, variables, context) {
            toast.success("Thành công thêm mới khóa học")
            resetStates()
        },
    })
    const PostNewCourse = () => {
        const formData = new FormData()
        const canAdd = true
        const messageCourseName = document.getElementById('message-coursename');
        const messageDes = document.getElementById('message-des');
        const messageStatus = document.getElementById('message-status');
        const messageCourseType = document.getElementById('message-course-type');
        if (courseName == "") {
            if (messageCourseName) {
                messageCourseName.style.color = 'red';
                messageCourseName.innerHTML = 'Tên khóa học không được để trống';
            }
            return;
        }
        if (description == "") {
            if (messageDes) {
                messageDes.style.color = 'red';
                messageDes.innerHTML = 'Phần mô tả không được để trống';
            }
            return;
        }
        if(courseType == undefined){
            if (messageCourseType) {
                messageCourseType.style.color = 'red';
                messageCourseType.innerHTML = 'Phẩn loại khóa học không được bỏ trống';
            }
            return;
        }
        if(status == undefined){
            if (messageStatus) {
                messageStatus.style.color = 'red';
                messageStatus.innerHTML = 'Phẩn trạng thái không được bỏ trống';
            }
            return;
        }
        formData.append("courseName", courseName)
        formData.append("description", description)
        listLearnAbout.forEach((item, index) => {
            formData.append(`listLearnAbout[${index}]`, item.toString());
        });
        listPrepared.forEach((item, index) => {
            formData.append(`listPrepared[${index}]`, item.toString());
        });
        if (selectedFile != null) {
            formData.append("thumbnail", selectedFile)
        }
        formData.append("courseTypeId", courseType)
        formData.append("status", status)
        mutate(formData)
    }
    return (
        <div className='w-full'>            
            <div className='header flex items-center justify-between'>
                <h2 className='text-[20px] text-slate-50 font-bold'>Thêm mới khóa học</h2>
            </div>
            {isPending ? <Loading/> : <></>}
            <div className='flex flex-wrap'>
                <div className='w-full lg:w-2/3 p-1'>
                    <div className='card my-6 border rounded-md border-gray-700'>
                        <div className='p-4' style={{ boxSizing: 'border-box' }}>
                            <div className='header flex items-center justify-between'>
                                <h3 className='text-[16px] text-slate-50 font-bold'>Thông tin</h3>
                            </div>
                            <div className='py-4'>
                                <div className="w-full p-2">
                                    <Label htmlFor="email">Tên khóa học</Label>
                                    <Input value={courseName} onChange={(e) => setCoureName(e.target.value)} type="text" className='w-full mt-2 rounded-md' placeholder="Nhập tên khóa học" />
                                    <p className='text-left mt-1' id='message-coursename' style={{ fontSize: '0.75rem' }}>Đây là phần nhập tên khóa học</p>
                                </div>
                                <div className="w-full p-2">
                                    <Label htmlFor="email">Mô tả khóa học</Label>
                                    <Textarea value={description} onChange={(e) => setDescription(e.target.value)} className='w-full h-[250px] mt-2 rounded-md' placeholder="Nhập mô tả khóa học vào đây" />
                                    <p className='text-left mt-1 mb-24' id='message-des' style={{ fontSize: '0.75rem' }}>Phần mô tả cho khóa học</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className='w-full lg:w-1/3 p-1'>
                    <div className='card my-6 border rounded-md border-gray-700'>
                        <div className='p-4' style={{ boxSizing: 'border-box' }}>
                            <div className='header flex items-center justify-between'>
                                <h3 className='text-[16px] text-slate-50 font-bold'>Ảnh đại diện khóa học</h3>
                            </div>
                            <div className='py-4' onClick={() => document.getElementById('file')?.click()}>
                                <div style={{ minHeight: '150px', border: '1px dashed #fff', borderWidth: '2px', borderColor: 'rgba(0, 0, 0, 0.3)', padding: '20px', display: 'flex', alignItems: 'center', justifyContent: 'center', borderRadius: '7px', marginBottom: '0.5rem' }} className="w-full p-2 bg-card cursor-pointer">
                                    <div>
                                        {imagePreviewUrl == null ? "Nhấn vào đây để chọn tệp ảnh cho khóa học" : <img src={imagePreviewUrl} alt="Selected" className="max-w-full h-auto" />}
                                    </div>
                                    <Input onChange={(e) => handleFileChange(e)} accept='.png,.img,.imeg' type="file" id='file' className='w-full mt-2 rounded-md hidden' placeholder="Nhập tên khóa học" />
                                </div>
                                <p className='text-center' style={{ fontSize: '0.75rem' }}>Chọn tệp ảnh có đuôi là .png, .img, .imeg</p>
                            </div>
                        </div>
                    </div>
                    <div className='card my-6 border rounded-md border-gray-700'>
                        <div className='p-4' style={{ boxSizing: 'border-box' }}>
                            <div className='header flex items-center justify-between'>
                                <h3 className='text-[16px] text-slate-50 font-bold'>Trạng thái khóa học</h3>
                                <div className='bg-green-500 h-full p-2' style={{ borderRadius: '50%' }}></div>
                            </div>
                            <div className='py-4'>
                                <Select value={status} onValueChange={value => handleChangeSelect(1, value)}>
                                    <SelectTrigger className="w-full">
                                        <SelectValue placeholder="Chọn trạng thái khóa học" />
                                    </SelectTrigger>
                                    <SelectContent>
                                        <SelectGroup>
                                            <SelectLabel>Trạng thái khóa học</SelectLabel>
                                            <SelectItem value="1">Trạng thái chờ</SelectItem>
                                            <SelectItem value="2">Công khai</SelectItem>
                                        </SelectGroup>
                                    </SelectContent>
                                </Select>
                                <p className='text-left mt-1' id='message-status' style={{ fontSize: '0.75rem' }}>Chọn trạng thái cho phần khóa học</p>
                            </div>
                        </div>
                    </div>
                    <div className='card my-6 border rounded-md border-gray-700'>
                        <div className='p-4' style={{ boxSizing: 'border-box' }}>
                            <div className='header flex items-center justify-between'>
                                <h3 className='text-[16px] text-slate-50 font-bold'>Loại khóa học</h3>
                                <FontAwesomeIcon icon={faBook} />
                            </div>
                            <div className='py-4'>
                                <Select value={courseType} onValueChange={value => handleChangeSelect(2, value)}>
                                    <SelectTrigger className="w-full">
                                        <SelectValue placeholder="Chọn loại khóa học" />
                                    </SelectTrigger>
                                    <SelectContent>
                                        <SelectGroup>
                                            <SelectLabel>Loại khóa học</SelectLabel>
                                            {dataCourseType?.data.metadata.map((item, index) => {
                                                return <SelectItem key={index} value={item.id.toString()}>{item.typeName}</SelectItem>
                                            })}
                                        </SelectGroup>
                                    </SelectContent>
                                </Select>
                                <p className='text-left mt-1' id='message-course-type' style={{ fontSize: '0.75rem' }}>Chọn loại khóa học</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div className='flex flex-wrap'>
                <div className='w-full lg:w-1/2 p-1'>
                    <div className='card my-6 border rounded-md border-gray-700'>
                        <div className='p-4' style={{ boxSizing: 'border-box' }}>
                            <div className='header flex items-center justify-between'>
                                <h3 className='text-[16px] text-slate-50 font-bold'>Những gì sẽ học được</h3>
                                <FontAwesomeIcon onClick={() => addElement(1)} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' icon={faAdd} />
                            </div>
                            <div className='py-4'>
                                <div className="w-full p-2">
                                    {learnAbout.map((item, index) => {
                                        return <div key={index} className='flex items-center justify-start'>
                                            <div className='w-full'>
                                                <Textarea onChange={(e) => changeLearnAboutValue(index, e.target.value)} value={listLearnAbout[index]} className='w-full h-[20px] mt-2 rounded-md' placeholder="Nhập những gì sinh viên sẽ học được" />
                                                <p className='text-left mt-1' style={{ fontSize: '0.75rem' }}>Nhập những gì học sinh sẽ học được</p>
                                            </div>
                                            <FontAwesomeIcon onClick={() => removeElement(1, index)} className='hover:text-slate-50 cursor-pointer text-[18px] ml-4' icon={faTrash} />
                                        </div>
                                    })}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className='w-full lg:w-1/2 p-1'>
                    <div className='card my-6 border rounded-md border-gray-700'>
                        <div className='p-4' style={{ boxSizing: 'border-box' }}>
                            <div className='header flex items-center justify-between'>
                                <h3 className='text-[16px] text-slate-50 font-bold'>Những gì cần chuẩn bị</h3>
                                <FontAwesomeIcon onClick={() => addElement(2)} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' icon={faAdd} />
                            </div>
                            <div className='py-4'>
                                <div className="w-full p-2">
                                    {prepared.map((item, index) => {
                                        return <div key={index} className='flex items-center justify-start'>
                                            <div className='w-full'>
                                                <Textarea onChange={(e) => changePreparedValue(index, e.target.value)} value={listPrepared[index]} className='w-full h-[20px] mt-2 rounded-md' placeholder="Nhập những gì sinh viên cần chuẩn bị" />
                                                <p className='text-left mt-1' style={{ fontSize: '0.75rem' }}>Nhập những gì học sinh cần chuẩn bị</p>
                                            </div>
                                            <FontAwesomeIcon onClick={() => removeElement(2, index)} className='hover:text-slate-50 cursor-pointer text-[18px] ml-4' icon={faTrash} />
                                        </div>
                                    })}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div className='flex items-center justify-start'>
                <Button onClick={() => PostNewCourse()} variant='default'>Thêm mới khóa học</Button>
                <Button onClick={() => resetStates()} variant='destructive' className='ml-2'>Hủy</Button>
            </div>
        </div>
    )
}
