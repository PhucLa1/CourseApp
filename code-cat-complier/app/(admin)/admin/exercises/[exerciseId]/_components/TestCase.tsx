import { AddTestCase, DeleteTestCase, GetTestCase, UpdateTestCase } from '@/apis/exercises.api'
import Loading from '@/components/Loading'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from '@/components/ui/select'
import { faAd, faAdd, faCheck, faCheckCircle, faEye, faL, faRightLeft, faTrashAlt } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

import { useMutation, useQuery } from '@tanstack/react-query'
import React, { useEffect, useState } from 'react'
import toast from 'react-hot-toast'
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
type TestCase = {
    input: File | null,
    output: File | null,
    isLock: boolean | null,
    id: number
}
export default function TestCase({ exerciseId }: { exerciseId: number }) {
    const [testCase, setTestCase] = useState<TestCase[]>([])
    const [canMutate, setCanMutate] = useState<boolean[]>([])
    const { data: dataTestCase, isLoading: isLoadingTestCase } = useQuery({
        queryKey: ['test-case'],
        queryFn: () => GetTestCase(exerciseId)
    })
    const { mutate: mutateDelete, isPending: isPendingDelete, isSuccess: isSuccessDelete } = useMutation({
        mutationFn: ({ id, index }: { id: number, index: number }) => {
            return DeleteTestCase(id);
        },
        onMutate: (variables) => {
            // Lưu trữ index trong context để sử dụng trong onSuccess
            return { index: variables.index };
        },
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Xóa test case thành công");
                removeTestCase(context.index)
                removeAtIndex(context.index)
            }
        },
    })
    const { mutate: mutateAdd, isPending: isPendingAdd } = useMutation({
        mutationFn: ({ testCaseAdd, index }: { testCaseAdd: FormData, index: number }) => {
            return AddTestCase(exerciseId, testCaseAdd)
        },
        onMutate: (variables) => {
            // Lưu trữ index trong context để sử dụng trong onSuccess
            return { index: variables.index };
        },
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Đã thêm thành công test case mới")
                console.log(data.data.metadata)
                updateTestCase(context.index)
                updateId(context.index, data.data.metadata)
                updateAtIndex(context.index, false)
            }
        },
    })

    const { mutate: mutateUpdate, isPending: isPendingUpdate } = useMutation({
        mutationFn: ({ testCaseUpdate, index, id }: { testCaseUpdate: FormData, index: number, id: number }) => {
            return UpdateTestCase(id, testCaseUpdate)
        },
        onMutate: (variables) => {
            // Lưu trữ index trong context để sử dụng trong onSuccess
            return { index: variables.index };
        },
        onSuccess(data, variables, context) {
            if (data.data.isSuccess) {
                toast.success("Đã sửa thành công test case đó")
                updateTestCase(context.index)
                updateAtIndex(context.index, false)
            }
        },
    })
    const updateTestCase = (index: number) => {
        const updatedTestCase = [...testCase];
        updatedTestCase[index] = {
            ...updatedTestCase[index], // Giữ nguyên các thuộc tính hiện tại
            input: null,
            output: null,
        };
        setTestCase(updatedTestCase);
    };
    const addTestCase = () => {
        setTestCase([...testCase, {
            input: null,
            output: null,
            isLock: null,
            id: 0
        }]);
    };
    const removeTestCase = (index: number) => {
        setTestCase(testCase.filter((_, i) => i !== index));
    };

    const updateIsLock = (index: number, newIsLock: any) => {
        setTestCase(prevTestCase => {
            const updatedTestCase = [...prevTestCase];
            updatedTestCase[index] = { ...updatedTestCase[index], isLock: newIsLock };
            return updatedTestCase;
        });
        updateAtIndex(index, true)
    };
    const updateId = (index: number, newId: number) => {
        setTestCase(prevTestCase => {
            const updatedTestCase = [...prevTestCase];
            updatedTestCase[index] = { ...updatedTestCase[index], id: newId };
            return updatedTestCase;
        });
    }
    const handleFileChange = (opt: number, index: number, event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0] || null;
        setTestCase(prevTestCase => {
            return prevTestCase.map((testCase, i) => {
                if (i === index) {
                    if (opt === 1) {
                        return { ...testCase, input: file };
                    } else if (opt === 2) {
                        return { ...testCase, output: file };
                    }
                }
                return testCase;
            });
        });
        updateAtIndex(index, true)
    };
    const onRemoveTestCase = (index: number, id: number) => {
        if (id === 0) {
            removeTestCase(index)
            removeAtIndex(index)
        } else {
            mutateDelete({ id, index });
        }
    }
    useEffect(() => {
        if (dataTestCase) {
            setTestCase(dataTestCase.data.metadata.map((item, index) => ({
                input: null,
                output: null,
                isLock: item.isLock,
                id: item.id
            })))
            setCanMutate(dataTestCase.data.metadata.map((item, index) => false))
        }
    }, [dataTestCase])
    const updateAtIndex = (index: number, value: boolean) => {
        setCanMutate(prev => {
            const newArray = [...prev];
            newArray[index] = value;
            return newArray;
        });
    };

    const removeAtIndex = (index: number) => {
        setCanMutate(prev => {
            const newArray = [...prev];
            newArray.splice(index, 1);
            return newArray;
        });
    };
    const addToEnd = () => {
        setCanMutate(prev => [...prev, false]);
    };
    const handleClickInput = (index: number) => {
        const fileInput = document.querySelectorAll('.fileInput')[index] as HTMLElement;
        fileInput.click();
    };
    const handleClickOutput = (index: number) => {
        const fileInput = document.querySelectorAll('.fileOutput')[index] as HTMLElement;
        fileInput.click();
    };
    const onHandleTestCase = (index: number, id: number) => {
        if (id == 0) {
            const formData = new FormData();
            if (testCase[index].input != null && testCase[index].output != null && testCase[index].isLock != null) {
                console.log(testCase[index].input)
                formData.append(`inputData`, testCase[index].input)
                formData.append(`expectedOutput`, testCase[index].input)
                formData.append(`isLock`, testCase[index].isLock == true ? "true" : "false")
                mutateAdd({
                    testCaseAdd: formData,
                    index: index
                })
            } else {
                toast.error("Phải cho đầy đủ các thông số")
            }
        } else {
            const formData = new FormData();
            if (testCase[index].output != null) {
                formData.append(`expectedOutput`, testCase[index].output)
            }
            if (testCase[index].input != null) {
                formData.append(`inputData`, testCase[index].input)
            }
            if (testCase[index].isLock != null) {
                formData.append(`isLock`, testCase[index].isLock == true ? "true" : "false")
            }
            mutateUpdate({
                testCaseUpdate: formData,
                index: index,
                id: id
            })
        }
    }
    console.log(testCase)
    return (
        <div className='test-case mt-4 rounded-md'>
            {isPendingDelete || isPendingAdd || isPendingUpdate ? <Loading /> : <></>}
            <div className='header flex items-center justify-start'>
                <h2 className='text-[18px] text-slate-50 font-bold'>Chỉnh sửa test case</h2>
                <FontAwesomeIcon onClick={() => addTestCase()} className='hover:text-slate-50 cursor-pointer text-[18px] ml-6' icon={faAdd} />
            </div>
            <div className='mt-4'>
                {isLoadingTestCase ? <Loading /> : <></>}
                <div className='mt-1'>
                    {testCase.map((item, index) => {
                        return <div key={index} className='mt-4 mr-4'>
                            <div className='flex items-center justify-between'>
                                <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                                    <div className='flex items-center justify-between'>
                                        <Label className='ml-2 mb-2' htmlFor="picture">Đầu vào</Label>
                                        <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[18px]' icon={faEye} />
                                    </div>
                                    <Input className="hidden fileInput" type="file" accept=".in" onChange={(e) => handleFileChange(1, index, e)} />
                                    <Button onClick={() => handleClickInput(index)} variant='outline'>Test case đầu vào {index + 1}</Button>
                                </div>
                                <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                                    <div className='flex items-center justify-between'>
                                        <Label className='ml-2 mb-2' htmlFor="picture">Đầu ra</Label>
                                        <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[18px]' icon={faEye} />
                                    </div>
                                    <Input className="hidden fileOutput" type="file" accept=".out" onChange={(e) => handleFileChange(2, index, e)} />
                                    <Button onClick={() => handleClickOutput(index)} variant='outline'>Test case đầu ra {index + 1}</Button>
                                </div>
                                <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                                    <Label className='ml-2 mb-2' htmlFor="picture">Loại test case</Label>
                                    <Select value={item.isLock === null ? undefined : (item.isLock ? "1" : "2")} onValueChange={value => updateIsLock(index, value)}>
                                        <SelectTrigger className="w-[180px]">
                                            <SelectValue placeholder="Lựa chọn loại" />
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectGroup>
                                                <SelectLabel>Test Case</SelectLabel>
                                                <SelectItem value="1">Test case ẩn</SelectItem>
                                                <SelectItem value="2">Test case mở</SelectItem>
                                            </SelectGroup>
                                        </SelectContent>
                                    </Select>
                                </div>
                                <FontAwesomeIcon onClick={() => onRemoveTestCase(index, item.id)} className='hover:text-slate-50 cursor-pointer text-[18px] mt-4 mr-6' icon={faTrashAlt} />
                                {canMutate[index] ? <FontAwesomeIcon onClick={() => onHandleTestCase(index, item.id)} className='hover:text-slate-50 cursor-pointer text-[18px] mt-4 mr-6' icon={faCheckCircle} /> : <></>}
                            </div>
                        </div>
                    })}
                </div>
            </div>
        </div>

    )
}