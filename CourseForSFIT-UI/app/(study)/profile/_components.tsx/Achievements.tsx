"use client"
import { faAdd, faCheck, faPen, faPenToSquare, faTrash } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React, { useState } from 'react'
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import { Button } from '@/components/ui/button'
import { useMutation } from '@tanstack/react-query'
import { UpdateAchievements } from '@/apis/user.api'
import toast from 'react-hot-toast'
import { Input } from '@/components/ui/input'
import { Textarea } from '@headlessui/react'
import Loading from '@/components/Loading'
export default function Achievements({ achievements, onUpdate }: { achievements: string[], onUpdate: () => void }) {
    const [value, setValue] = useState<string[]>(achievements)
    const [canChange, setCanChange] = useState<boolean[]>(achievements.map(() => false));
    const { mutate, isPending } = useMutation({
        mutationFn: () => {
            return UpdateAchievements(value);
        },
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                onUpdate()
                toast.success("Cập nhật thành tích học tập thành công")
            }
        },
    })
    const addAchievement = (newAchievement: string) => {
        setValue((prevValue) => [...prevValue, newAchievement]);
        addItemAtEnd()
    };
    const removeItemAtIndex = (index: number) => {
        setCanChange(prevState => 
            prevState.filter((_, i) => i !== index) // Removes the item at the specified index
        );
    };
    const addItemAtEnd = () => {
        setCanChange(prevState => [...prevState, false]); // Appends a `false` at the end
    };
    const removeAchievement = (index: number) => {
        setValue((prevValue) => prevValue.filter((_, i) => i !== index));
        removeItemAtIndex(index)
    };
    const toggleValueAtIndex = (index: number) => {
        setCanChange(prevState => 
            prevState.map((value, i) => (i === index ? !value : value))
        );
    };
    const updateValueAtIndex = (index: number, newValue: string) => {
        setValue(prevValue => 
            prevValue.map((item, i) => (i === index ? newValue : item))
        );
    };
    return (
        <div className='text-gray-400' style={{ border: '1px solid #3a3a40', borderRadius: '1rem', background: '#121418', padding: '1.5rem', minWidth: 'auto', maxWidth: '636px', width: '100%', margin: '0px', boxSizing: 'border-box' }}>
            {isPending ?? <Loading/>}
            <div className='flex items-center justify-between'>
                <h3 className='my-0' style={{ fontSize: '18px', lineHeight: 1.4, fontWeight: 700 }}>Thành tích học tập</h3>
                <div>
                    <div className='flex items-center cursor-pointer justify-center'>
                        <Dialog>
                            <DialogTrigger asChild>
                                <FontAwesomeIcon onClick={() => setValue(achievements)} icon={faPenToSquare} />
                            </DialogTrigger>
                            <DialogContent className="sm:max-w-[425px]">
                                <DialogHeader>
                                    <DialogTitle>Chỉnh sửa thông tin cá nhân</DialogTitle>
                                    <DialogDescription>
                                        Thay đổi thông tin cá nhân. Nhấn lưu trước khi thoát ra.
                                    </DialogDescription>
                                </DialogHeader>
                                <div className="grid gap-4 py-4">
                                    <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[18px]' onClick={() => addAchievement("")} icon={faAdd}/>
                                    {value.map((item, index) => {
                                        return <div key={index} className='flex items-center justify-between'>
                                            <Textarea onChange={(e) => updateValueAtIndex(index, e.target.value)}  disabled={canChange[index] ? false : true} value={item} className='w-full rounded-md p-2' />
                                            <div className='flex items-center justify-end'>
                                                <FontAwesomeIcon onClick={() => toggleValueAtIndex(index)} className='ml-4 hover:text-slate-50 cursor-pointer text-[18px]' icon={faPen} />
                                                <FontAwesomeIcon onClick={() => removeAchievement(index)} className='ml-4 hover:text-slate-50 cursor-pointer text-[18px]' icon={faTrash} />
                                            </div>
                                        </div>
                                    })}
                                </div>
                                <DialogFooter>
                                    <Button onClick={() => mutate()} type="submit">Lưu lại</Button>
                                </DialogFooter>
                            </DialogContent>
                        </Dialog>
                    </div>
                </div>
            </div>
            <div className='mt-4'>
                <ul style={{ marginBottom: '10px', paddingLeft: 0 }}>
                    {achievements.map((item, index) => {
                        return <li style={{ flexBasis: '50%', fontSize: '1.4rem', lineHeight: 1.6, marginBottom: '12px', position: 'relative', padding: '0 24px' }}>
                            <FontAwesomeIcon className='text-green-400 text-sm absolute left-0 top-3' icon={faCheck} />
                            <span className='text-sm text-gray-400 ml-1'>{item}</span>
                        </li>
                    })}
                </ul>
            </div>
        </div>
    )
}
