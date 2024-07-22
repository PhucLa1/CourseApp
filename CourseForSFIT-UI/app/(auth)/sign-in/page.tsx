"use client"
import React from 'react'
import { Input } from "@/components/ui/input"
import { zodResolver } from "@hookform/resolvers/zod"
import { useRouter } from 'next/navigation'
import Link from 'next/link'
import { z } from "zod"
import { useForm } from "react-hook-form"
import ThirdAuth from '../_components/ThirdAuth'
import { useMutation } from '@tanstack/react-query'
import { Login } from '@/apis/auth.api'
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import Loading from '@/components/Loading'
import toast from 'react-hot-toast'
const formSchema = z.object({
    email: z.string().email({
        message: "Không đúng định dạng email",
    }),
    password: z
        .string()
        .regex(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/,
            "Password phải bao gồm ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường và số"
        )
})
export default function page() {
    const router = useRouter();
    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            email: "",
            password: "",
        },
    })
    const { mutate, isPending } = useMutation({
        mutationFn: (values: z.infer<typeof formSchema>) => {
            return Login(values)
        },
        onSuccess: (data) => {
            if (data.data.isSuccess) {
                router.push('/');
            }
            toast.error(data.data.message[0])
        }
    })
    const handleSubmit = (values: z.infer<typeof formSchema>) => {
        mutate(values)
        console.log(values)
    }
    return (
        <Form {...form}>
            {isPending && <Loading/>}
            <form onSubmit={form.handleSubmit(handleSubmit)}>
                <div className='bg-gray-800' style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <div className='rounded-lg border bg-card text-card-foreground shadow-sm'>
                        <div className='flex flex-col p-6 space-y-1'>
                            <h3 className='font-semibold tracking-tight text-2xl'>Đăng nhập tài khoản</h3>
                            <p className='text-sm text-muted-foreground'>Nhập tài khoản để vào trang chủ</p>
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
                            <div className='grid gap-2'>
                                <FormField
                                    control={form.control}
                                    name="email"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormLabel className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70'>Email</FormLabel>
                                            <FormControl>
                                                <Input placeholder="phucminhbeos@gmail.com" {...field} />
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
                        </div>
                        <div className='flex items-center p-6 pt-0 justify-between'>
                            <button className='inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 bg-primary text-primary-foreground hover:bg-primary/90 h-10 px-4 py-2'><Link href='/forgot-pass'>Quên mật khẩu</Link></button>
                            <button type='submit' className='inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 bg-primary text-primary-foreground hover:bg-primary/90 h-10 px-4 py-2'>Đăng nhập</button>
                        </div>
                        <div className='p-6 pt-0 grid gap-4'>
                            <div className='relative'>
                                <div className='absolute inset-0 flex items-center'>
                                    <span className='w-full border-t'></span>
                                </div>
                                <div className='relative flex justify-center text-xs uppercase'>
                                    <span className='bg-background px-2 text-muted-foreground'>Chưa có tài khoản</span>
                                </div>
                            </div>
                            <div className='relative flex justify-center text-xs uppercase mt-1'>
                                <span className='bg-background px-2 text-muted-foreground cursor-pointer hover:text-slate-50'><Link href='/sign-up'>Đăng kí tại đây</Link></span>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </Form>
    )
}
