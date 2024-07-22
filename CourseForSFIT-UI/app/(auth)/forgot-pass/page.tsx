"use client"
import React, { useState } from 'react'
import {
    InputOTP,
    InputOTPGroup,
    InputOTPSeparator,
    InputOTPSlot,
} from "@/components/ui/input-otp"
import { Input } from "@/components/ui/input"
import Link from 'next/link'
import { useMutation } from '@tanstack/react-query'
import { ChangePassword, GenerateCode, VerifyCode } from '@/apis/auth.api'
import { useRouter } from 'next/navigation'
import Loading from '@/components/Loading'
import toast from 'react-hot-toast'
import { ResetPassword, VerifyVerificationCodeRequest } from '@/model/User'



export default function page() {
    const router = useRouter();
    const [otp, setOtp] = useState<boolean>(false)
    const [email, setEmail] = useState<string>("")
    const [otpValue, setOtpValue] = useState<string>("")
    const [password, setPassword] = useState<string>("")
    const [rePassword, setRePassword] = useState<string>("")
    const [successOtp, setSuccessOtp] = useState<boolean>(false)
    const HandleSubmit = () => {
        if (email == "") {
            toast.error("Email không được để trống")
            return;
        }
        if (!otp) {
            mutateGenerateCode(email)
        }
        else if(successOtp){
            mutateChangePass({
                email: email,
                password: password,
                rePassword: rePassword
            })
        }
        else {
            mutateOtp({
                email: email,
                code: otpValue
            })
        }

    }
    const { mutate: mutateGenerateCode, isPending: isPendingGenerateCode } = useMutation({
        mutationFn: (email: string) => {
            return GenerateCode(email)
        },
        onSuccess: (data) => {
            if (data.data.isSuccess) {
                setOtp(true)
                toast.success("Gửi thành công")
                const inputElement = document.querySelector('.input-email') as HTMLInputElement
                inputElement.disabled = true
                return
            }
            toast.error(data.data.message[0])
        },
        onError(error, variables, context) {
            toast.error(error.message)
        }
    })

    const { mutate: mutateOtp, isPending: isPendingOtp } = useMutation({
        mutationFn: (value: VerifyVerificationCodeRequest) => {
            return VerifyCode(value)
        },
        onSuccess: (data) => {
            if (data.data.isSuccess) {
                toast.success("Bạn đã thành công nhập mã code")
                setSuccessOtp(true)
                //router.push('/sign-in')
                return
            }
            setOtpValue('')
            toast.error(data.data.message[0])
        },
        onError(error, variables, context) {
            toast.error(error.message)
        }
    })

    const {mutate: mutateChangePass, isPending: isPendingChangePass} = useMutation({
        mutationFn: (value: ResetPassword) => {
            return ChangePassword(value)
        },
        onSuccess: (data) => {
            if (data.data.isSuccess){
                toast.success("Thay đổi mật khẩu thành công")
                router.push('/sign-in')
                return;
            }
            toast.error(data.data.message[0])
        },
        onError(error, variables, context) {
            toast.error(error.message)
        }
    })

    return (
        <div className='bg-gray-800' style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            {isPendingGenerateCode || isPendingOtp || isPendingChangePass ? <Loading /> : <></>}
            <div className='rounded-lg border bg-card text-card-foreground shadow-sm'>
                <div className='flex flex-col p-6 space-y-1'>
                    <div className='items-center flex justify-between mb-1'>
                        <h3 className='font-semibold tracking-tight text-2xl'>Quên mật khẩu</h3>
                        <span className='text-sm text-muted-foreground cursor-pointer hover:text-slate-50'><Link href='/sign-in'>Đăng nhập</Link></span>
                    </div>

                    <p className='text-sm text-muted-foreground'>Nhập email để chúng tôi có thể gửi OTP cho bạn</p>
                </div>
                <div className='p-6 pt-0 grid gap-4' style={{display: `${successOtp ? 'none' :'block'}` }}>
                    <div className='grid gap-2'>
                        <label className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70' htmlFor="">Email </label>
                        <Input className='input-email' type='email' onChange={event => setEmail(event.target.value)} placeholder='phucminhbeos@gmail.com' />
                    </div>
                    {otp && <div className='grid gap-2'>
                        <label className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70' htmlFor="">Nhập mã OTP vừa được gửi </label>
                        <InputOTP maxLength={6} value={otpValue} onChange={(value) => setOtpValue(value)}>
                            <InputOTPGroup>
                                <InputOTPSlot index={0} />
                                <InputOTPSlot index={1} />
                            </InputOTPGroup>
                            <InputOTPSeparator />
                            <InputOTPGroup>
                                <InputOTPSlot index={2} />
                                <InputOTPSlot index={3} />
                            </InputOTPGroup>
                            <InputOTPSeparator />
                            <InputOTPGroup>
                                <InputOTPSlot index={4} />
                                <InputOTPSlot index={5} />
                            </InputOTPGroup>
                        </InputOTP>
                    </div>}
                </div>
                <div className='p-6 pt-0 grid gap-4' style={{display: `${!successOtp ? 'none' :'block'}` }}>
                    <div className='grid gap-2'>
                        <label className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70' htmlFor="">Nhập lại mật khẩu mới </label>
                        <Input onChange={event => setPassword(event.target.value)} placeholder="........" />
                    </div>
                    <div className='grid gap-2 mt-2'>
                        <label className='text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70' htmlFor="">Nhập lại mật khẩu mới </label>
                        <Input onChange={event => setRePassword(event.target.value)} placeholder="........" />
                    </div>
                </div>
                <div className='p-6 pt-0 grid gap-4'>
                    <div className='relative'>
                        <div className='absolute inset-0 flex items-center'>
                            <span className='w-full border-t'></span>
                        </div>
                        <div onClick={() => mutateGenerateCode(email)} className='relative flex justify-center text-xs uppercase' style={{display: `${successOtp || !otp ? 'none' :'block'}` }}>
                            <span className='bg-background px-2 text-muted-foreground'>Chưa nhận được OTP <span className='text-slate-50 cursor-pointer'>Nhấn để gửi lại</span></span>
                        </div>
                    </div>
                </div>
                <div className='flex items-center p-6 pt-0 justify-center'>
                    <button onClick={() => HandleSubmit()} className='inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 bg-primary text-primary-foreground hover:bg-primary/90 h-10 px-4 py-2'>{otp ? 'Thay đổi mật khẩu' : 'Gửi OTP đến email'}</button>
                </div>
            </div>
        </div>
    )
}
