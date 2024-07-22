import React, { useState } from 'react'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { faGraduationCap, faPenToSquare, faSignature, faUserGraduate, faUsersLine } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { UserCurrentInfo } from '@/model/User'
import moment from 'moment'
import { Button } from "@/components/ui/button"
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectLabel,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
import { useMutation } from '@tanstack/react-query'
import { UpdateCurrentUser } from '@/apis/user.api'
import toast from 'react-hot-toast'
import Loading from '@/components/Loading'
type UserUpdate = {
    avatarUrl?: string
    avatarFile?: File;
    nickName?: string;
    schoolYear?: number;
    class?: string;
}
type AvatarProps = {
    src: string;
    alt: string;
}
export default function Info({ userInfo, onUpdate }: { userInfo: UserCurrentInfo, onUpdate: () => void }) {
    const [isChange, setIsChange] = useState<number>(0)
    const [userUpdate, setUserUpdate] = useState<UserUpdate>({
        avatarUrl: userInfo.avatar ?? "",
        nickName: userInfo.nickName ?? "",
        schoolYear: userInfo.schoolYear ?? undefined,
        class: userInfo.class ?? undefined
    })
    const updateUserUpdate = (key: keyof UserUpdate, value: UserUpdate[keyof UserUpdate]) => {
        setUserUpdate((prev) => ({
            ...prev,
            [key]: value
        }));
    };
    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            const newAvatarUrl = URL.createObjectURL(file);
            setUserUpdate((prevState) => ({
                ...prevState,
                avatarFile: file,
                avatarUrl: newAvatarUrl,
            }));
            setIsChange(isChange + 1)
        }
    };
    const {mutate, isPending} = useMutation({
        mutationFn: (user: FormData) => {
            return UpdateCurrentUser(user);
        },
        onSuccess(data, variables, context) {
            if(data.data.isSuccess){
                onUpdate()
                toast.success("Cập nhật thông tin thành công")
            }
        },
    })
    const onUpdateUser = () => {
        const formData = new FormData()
        if(userUpdate.avatarFile){
            formData.append("avatar", userUpdate.avatarFile)
        }
        if(userUpdate.nickName){
            formData.append("nickName", userUpdate.nickName)
        }
        if(userUpdate.schoolYear){
            formData.append("schoolYear", userUpdate.schoolYear.toString())
        }
        if(userUpdate.class){
            formData.append("class", userUpdate.class)
        } 
        mutate(formData)
    }
    return (
        <div style={{ border: '1px solid #3a3a40', borderRadius: '1rem', background: '#121418', padding: '1rem', minWidth: 'auto', maxWidth: '636px', width: '100%', margin: '0px', boxSizing: 'border-box' }}>
            {isPending && <Loading/>}
            <div className='flex items-center justify-between'>
                <Avatar style={{ width: '50px', height: '50px' }}>
                    <AvatarImage src={`https://localhost:7130/Uploads/${userInfo.avatar}`} alt="@shadcn" />
                    <AvatarFallback>CN</AvatarFallback>
                </Avatar>
                <div>
                    <div className='flex items-center cursor-pointer justify-center'>
                        <Dialog>
                            <DialogTrigger asChild>
                                <FontAwesomeIcon icon={faPenToSquare} />
                            </DialogTrigger>
                            <DialogContent className="sm:max-w-[425px]">
                                <DialogHeader>
                                    <DialogTitle>Chỉnh sửa thông tin cá nhân</DialogTitle>
                                    <DialogDescription>
                                        Thay đổi thông tin cá nhân. Nhấn lưu trước khi thoát ra.
                                    </DialogDescription>
                                </DialogHeader>
                                <div className="grid gap-4 py-4">
                                    <Avatar onClick={() => document.getElementById('fileImage')?.click()} style={{ width: '50px', height: '50px' }}>
                                        {isChange == 0 ? <AvatarImage src={`https://localhost:7130/Uploads/${userUpdate.avatarUrl}`} alt="@shadcn" /> :
                                            <AvatarImage src={userUpdate.avatarUrl} alt="@shadcn" />
                                        }
                                        <AvatarFallback>CN</AvatarFallback>
                                        <Input onChange={(e) => handleFileChange(e)} accept='.png,.img' id='fileImage' type='file' className='hidden' />
                                    </Avatar>
                                    <Input type="email" disabled value={userInfo.email ?? ""} />
                                    <div className='gap-4 flex items-center'>
                                        <div>
                                            <Input disabled value={userInfo.firstName ?? ""} />
                                        </div>
                                        <div>
                                            <Input disabled value={userInfo.lastName ?? ""} />
                                        </div>
                                    </div>
                                    <Input onChange={(e) => updateUserUpdate('nickName', e.target.value)} placeholder='Nick name' value={userUpdate.nickName} />
                                    <div className='gap-4 flex items-center'>
                                        <div>
                                            <Select onValueChange={(e) => updateUserUpdate('schoolYear', parseInt(e, 10))} value={userUpdate.schoolYear !== undefined ? userUpdate.schoolYear.toString() : undefined}>
                                                <SelectTrigger className="w-[180px]">
                                                    <SelectValue placeholder="Chọn năm học" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    <SelectGroup>
                                                        <SelectLabel>Năm học</SelectLabel>
                                                        <SelectItem value="1">Năm nhất</SelectItem>
                                                        <SelectItem value="2">Năm hai</SelectItem>
                                                        <SelectItem value="3">Năm ba</SelectItem>
                                                        <SelectItem value="4">Nam tư</SelectItem>
                                                    </SelectGroup>
                                                </SelectContent>
                                            </Select>
                                        </div>
                                        <div>
                                            <Input placeholder='Nhập tên lớp học' onChange={(e) => updateUserUpdate('class', e.target.value)} value={userUpdate.class} />
                                        </div>
                                    </div>
                                </div>
                                <DialogFooter>
                                    <Button onClick={() => onUpdateUser()} type="submit">Lưu lại</Button>
                                </DialogFooter>
                            </DialogContent>
                        </Dialog>
                    </div>
                </div>
            </div>
            <div className='flex items-center' style={{ marginTop: '24px' }}>
                <h1 style={{ display: 'block', overflow: 'hidden', maxWidth: '300px', textOverflow: 'ellipsis', whiteSpace: 'nowrap', fontSize: '1.75rem', lineHeight: '2.25rem', fontWeight: 700, letterSpacing: '-0.063rem' }}>{userInfo.nickName == null ? "Nick name ?" : userInfo.nickName}</h1>
                <div style={{ marginLeft: '0.5rem' }} className='items-center flex'>
                    <img width='20px' src="/logo/chat-bubble.png" alt="1" />
                </div>
            </div>
            <p style={{ marginTop: '-2px', fontWeight: 400, fontSize: '0.875rem', lineHeight: '1.25rem' }}>{userInfo.email}</p>
            <div style={{ display: 'flex', flexDirection: 'column', boxSizing: 'border-box', minHeight: '80px', marginTop: '20px' }}>
                <div className='flex items-center mt-2'>
                    <div style={{ fontWeight: 600, fontSize: '1rem', lineHeight: '1.5rem' }}>
                        <FontAwesomeIcon icon={faSignature} />
                    </div>
                    <span className='text-gray-400 text-sm ml-2'>Họ và tên : <strong className='text-slate-100'>{userInfo.firstName + " " + userInfo.lastName}</strong></span>
                </div>
                <div className='flex items-center mt-3'>
                    <div style={{ fontWeight: 600, fontSize: '1rem', lineHeight: '1.5rem' }}>
                        <FontAwesomeIcon icon={faUserGraduate} />
                    </div>
                    <span className='text-gray-400 text-sm ml-3'>Lớp : <strong className='text-slate-100'>{userInfo.class}</strong></span>
                </div>
                <div className='flex items-center mt-2'>
                    <div style={{ fontWeight: 600, fontSize: '1rem', lineHeight: '1.5rem' }}>
                        <FontAwesomeIcon icon={faGraduationCap} />
                    </div>
                    <span className='text-gray-400 text-sm ml-3'>Khóa : <strong className='text-slate-100'>{userInfo.schoolYear == null ? "" : `K${66 - userInfo.schoolYear}`}</strong></span>
                </div>
                <div className='flex items-center mt-2'>
                    <div style={{ fontWeight: 600, fontSize: '1rem', lineHeight: '1.5rem' }}>
                        <FontAwesomeIcon icon={faUsersLine} />
                    </div>
                    <span className='text-gray-400 text-sm ml-3'>Tham gia từ  <strong className='text-slate-100'>{moment(userInfo.createdAt).format('dddd, DD-MM-YYYY HH:mm:ss')}</strong></span>
                </div>
            </div>
        </div>
    )
}
