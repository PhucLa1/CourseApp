"use client"
import React, { useState } from 'react'

import Link from 'next/link'
import ThirdAuth from '../_components/ThirdAuth'
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { z } from "zod"
import { useRouter } from 'next/navigation'
import { Button } from "@/components/ui/button"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { useMutation } from '@tanstack/react-query'
import { SignUp } from '@/apis/auth.api'
import Loading from '@/components/Loading'
import toast from 'react-hot-toast'

const formSchema = z.object({
    firstName: z.string().min(1, "Trường này không được để trống"),
    lastName: z.string().min(1, "Trường này không được để trống"),
    email: z.string().email({
        message: "Không đúng định dạng email",
    }),
    password: z
        .string()
        .regex(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/,
            "Password phải bao gồm ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường và số"
        ),
    rePassword: z.string().min(1, "Trường này không được để trống"),
}).refine((data) => {
    return data.password === data.rePassword
}, {
    message: "Mật khẩu không khớp"
});



export default function page() {
    const router = useRouter();
    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues : {
            email: ""
        },
    })
    const {mutate,isPending} = useMutation({
        mutationFn: (values: z.infer<typeof formSchema>) => {
            return SignUp(values)
        },
        onSuccess: (data) => {
            if(data.data.isSuccess){
                router.push('/');
            }
            toast.error(data.data.message[0])
        }
    })
    const handleSubmit = (values: z.infer<typeof formSchema>) => 
    {  
        mutate(values)
        console.log(values)
    }
    return (
        <Form {...form}>
            {isPending && <Loading/>}
            <form onSubmit={form.handleSubmit(handleSubmit)}>
                <div>
                    <div className='bg-gray-800' style={{ display: 'flex', justifyContent: 'center', alignItems: 'center'}}>
                        <div className='rounded-lg border bg-card text-card-foreground shadow-sm'>
                            <div className='flex flex-col p-6 space-y-1'>
                                <h3 className='font-semibold tracking-tight text-2xl'>Đăng kí tài khoản</h3>
                                <p className='text-sm text-muted-foreground'>Thêm tài khoản vào hệ thống giáo dục SFIT - UTC</p>
                            </div>
                            <div className='p-6 pt-0 grid gap-4'>
                                <ThirdAuth />
                                <div className='relative'>
                                    <div className='absolute inset-0 flex items-center'>
                                        <span className='w-full border-t'></span>
                                    </div>
                                    <div className='relative flex justify-center text-xs uppercase'>
                                        <span className='bg-background px-2 text-muted-foreground'>Hoặc tiếp tục với</span>
                                    </div>
                                </div>

                                <div className='gap-2 flex items-center'>
                                    <div>
                                        <FormField
                                            control={form.control}
                                            name="firstName"
                                            render={({ field }) => (
                                                <FormItem>
                                                    <FormLabel className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>Họ</FormLabel>
                                                    <FormControl>
                                                        <Input placeholder="Lã" {...field} />
                                                    </FormControl>
                                                    <FormMessage />
                                                </FormItem>
                                            )}
                                        />
                                    </div>
                                    <div>
                                        <FormField
                                            control={form.control}
                                            name="lastName"
                                            render={({ field }) => (
                                                <FormItem>
                                                    <FormLabel className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>Tên</FormLabel>
                                                    <FormControl>
                                                        <Input placeholder="Phúc" {...field} />
                                                    </FormControl>
                                                    <FormMessage />
                                                </FormItem>
                                            )}
                                        />
                                    </div>
                                </div>
                                <div className='grid gap-2'>
                                    <FormField
                                        control={form.control}
                                        name="email"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>Email</FormLabel>
                                                <FormControl>
                                                    <Input placeholder="phucminhbeos@gmail.com" type='email' {...field} />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                </div>
                                <div className='grid gap-2'>
                                    <FormField
                                        control={form.control}
                                        name="password"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>Mật khẩu</FormLabel>
                                                <FormControl>
                                                    <Input placeholder="........" type='password' {...field} />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                </div>
                                <div className='grid gap-2'>
                                    <FormField
                                        control={form.control}
                                        name="rePassword"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>Xác nhận mật khẩu</FormLabel>
                                                <FormControl>
                                                    <Input placeholder="........" type='password' {...field} />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                </div>
                            </div>
                            <div className='mt-2 flex items-center p-6 pt-0 justify-center'>
                                <Button type="submit" className='inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 bg-primary text-primary-foreground hover:bg-primary/90 h-10 px-4 py-2 w-full'>Đăng kí</Button>
                            </div>
                            <div className='p-6 pt-0 grid gap-4'>
                                <div className='relative'>
                                    <div className='absolute inset-0 flex items-center'>
                                        <span className='w-full border-t'></span>
                                    </div>
                                    <div className='relative flex justify-center text-xs uppercase'>
                                        <span className='bg-background px-2 text-muted-foreground'>Đã có tài khoản</span>
                                    </div>
                                </div>
                                <div className='relative flex justify-center text-xs uppercase mt-1'>
                                    <span className='bg-background px-2 text-muted-foreground cursor-pointer hover:text-slate-50'><Link href='/sign-in'>Đăng nhập tại đay</Link></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </Form>
    )
}
