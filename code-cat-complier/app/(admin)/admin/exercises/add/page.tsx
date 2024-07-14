"use client"
import React, { useState } from 'react'
import 'react-quill/dist/quill.snow.css';
import ReactQuill from 'react-quill';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faA, faAdd, faMinus } from '@fortawesome/free-solid-svg-icons';
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectLabel,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import ReactSelect from '@/components/ReactSelect';
import { useMutation, useQuery } from '@tanstack/react-query';
import { AddExercise, GetAllTagExercises, } from '@/apis/exercises.api';
import Loading from '@/components/Loading';
import toast from 'react-hot-toast';
import { Button } from '@/components/ui/button';
type ContentExercise = {
    description: string,
    constraints: string,
    input_format: string,
    output_format: string,
    input: string[],
    output: string[],
    explaintation: string
}
type TestCase = {
    input: File | null,
    output: File | null,
    isLock: boolean
}
export default function page() {
    const [value, setValue] = useState<ContentExercise>({
        description: '',
        constraints: '',
        input_format: '',
        output_format: '',
        input: [],
        output: [],
        explaintation: ''
    });
    const [exerciseName, setExerciseName] = useState<string>("")
    const [difficult, setDifficult] = useState<number>()
    const [isOpen, setIsOpen] = useState<boolean[]>([])
    const [testCase, setTestCase] = useState<TestCase[]>([])
    const [tagIds, setTagIds] = useState<number[]>([])
    const addTestCaseTest = () => {
        setValue(prevValue => ({
            ...prevValue,
            output: [...prevValue.output, ""]
        }));
        setValue(prevValue => ({
            ...prevValue,
            input: [...prevValue.input, ""]
        }));
    };
    const updateInputValue = (index: number, newValue: any) => {
        setValue(prevValue => ({
            ...prevValue,
            input: prevValue.input.map((item, i) => (i === index ? newValue : item))
        }));
    };


    const updateIsLock = (index: number, newIsLock: any) => {
        setTestCase(prevTestCase => {
            const updatedTestCase = [...prevTestCase];
            updatedTestCase[index] = { ...updatedTestCase[index], isLock: newIsLock };
            return updatedTestCase;
        });
    };


    // Hàm để sửa đổi giá trị của phần tử trong mảng output
    const updateOutputValue = (index: number, newValue: any) => {
        setValue(prevValue => ({
            ...prevValue,
            output: prevValue.output.map((item, i) => (i === index ? newValue : item))
        }));
    };
    const addTestCase = () => {
        setTestCase([...testCase, {
            input: null,
            output: null,
            isLock: true
        }]);
    };
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
    };
    const updateIsOpen = (index: number, newValue: boolean) => {
        setIsOpen(prevState => {
            const newState = [...prevState];
            newState[index] = newValue;
            return newState;
        });
    };

    const handleChange = (field: keyof ContentExercise, newValue: any) => {
        setValue(prevValue => ({
            ...prevValue,
            [field]: newValue
        }));
    };
    const { data, isLoading } = useQuery({
        queryKey: ['tag-exercise'],
        queryFn: () => GetAllTagExercises()
    })
    const HandleTagIds = (data: number[]) => {
        setTagIds(data)
    }

    const { mutate, isPending } = useMutation({
        mutationFn: () => {
            return new Promise((resolve, reject) => {
                if (exerciseName == "") {
                    toast.error("Vui lòng nhập tên bài tập")
                    reject(new Error("Tên bài tập không được để trống"));
                    return;
                } else if (difficult == null) {
                    toast.error("Vui lòng chọn độ khó")
                    reject(new Error("Độ khó không được để trống"));
                    return;
                } else if (testCase.length == 0 || testCase.every(test => test.output === null)) {
                    toast.error("Phải có test case mới hoàn thành xong được bài tập")
                    reject(new Error("Test case không được để trống"));
                    return;
                }
    
                const formData = new FormData();
                formData.append("exerciseName", exerciseName)
                formData.append("difficultLevel", difficult.toString())
                formData.append("contentExercise", JSON.stringify(value))
    
                tagIds.forEach((item, index) => {
                    formData.append(`tagIds[${index}]`, item.toString());
                });
    
                let indexTestCase = 0;
                testCase.forEach((item) => {
                    if (item.input !== null) {
                        formData.append(`testCaseAddDtos[${indexTestCase}].inputData`, item.input);
                    }
                    if (item.output !== null) {
                        formData.append(`testCaseAddDtos[${indexTestCase}].expectedOutput`, item.output);
                    }
                    formData.append(`testCaseAddDtos[${indexTestCase}].isLock`, item.isLock.toString());
                    indexTestCase++;
                });
    
                console.log(formData);
                resolve(AddExercise(formData));
            });
        },
        onSuccess(data) {
            toast.success("Thêm bài tập thành công")
            setValue(
                {description: '',
                constraints: '',
                input_format: '',
                output_format: '',
                input: [],
                output: [],
                explaintation: ''}
            )
            setExerciseName("")
            setDifficult(undefined)
            setIsOpen([])
            setTestCase([])
            setTagIds([])
        },
        
    });
    return (
        <div>
            {isLoading ? <Loading /> : <></>}
            {isPending ? <Loading /> : <></>}
            <div className='header flex items-center justify-between'>
                <h2 className='text-[20px] text-slate-50 font-bold'>Thêm bài tập lập trình</h2>
            </div>
            <div className='mt-4 border border-gray-500 rounded-md p-4 border-box' >
                <div className='flex items-center justify-between'>
                    <div className="grid w-full max-w-sm items-center gap-1.5">
                        <Label className='ml-2 mb-2' htmlFor="picture">Nhập tên bài tập</Label>
                        <Input value={exerciseName} onChange={(e) => setExerciseName(e.target.value)} placeholder='Nhập tên bài tập vào đây' type="text" />
                    </div>
                    <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                        <Label className='ml-2 mb-2' htmlFor="picture">Độ khó</Label>
                        <Select value={difficult?.toString()} onValueChange={(e) => setDifficult(parseInt(e, 10))}>
                            <SelectTrigger className="w-[180px]">
                                <SelectValue placeholder="Lựa chọn loại" />
                            </SelectTrigger>
                            <SelectContent>
                                <SelectGroup>
                                    <SelectLabel>Lựa chọn độ khó</SelectLabel>
                                    <SelectItem className='text-[#7bc043]' value="1">Dễ</SelectItem>
                                    <SelectItem className='text-[#faa05e]' value="2">Trung bình</SelectItem>
                                    <SelectItem className='text-[#e64f4a]' value="3">Khó</SelectItem>
                                </SelectGroup>
                            </SelectContent>
                        </Select>
                    </div>
                    <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                        <Label className='ml-2 mb-2' htmlFor="picture">Lựa chọn nhãn dán</Label>
                        <ReactSelect onHandleTagIds={HandleTagIds} value={data?.data.metadata.map((tag) => ({
                            label: tag.tagName,
                            value: tag.id
                        })) ?? []} />
                    </div>
                </div>
                <div className='descritption border  border-gray-500 rounded-md bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                    <div className='flex items-center justify-between'>
                        <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Yêu cầu/ Mô tả bài tập</h2>
                        <FontAwesomeIcon onClick={() => updateIsOpen(0, !isOpen[0])} icon={!isOpen[0] ? faAdd : faMinus} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                    </div>
                    {isOpen[0] && <ReactQuill value={value.description} onChange={(event) => handleChange('description', event.toString())} theme="snow" className='h-full mt-6 rounded-md text-slate-50 text-[14px]' />}
                </div>
                <div className='constraints border-t  border-gray-500 rounded-md bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                    <div className='flex items-center justify-between'>
                        <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Ràng buộc đề bài</h2>
                        <FontAwesomeIcon onClick={() => updateIsOpen(1, !isOpen[1])} icon={!isOpen[1] ? faAdd : faMinus} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                    </div>
                    {isOpen[1] && <ReactQuill value={value.constraints} onChange={(event) => handleChange('constraints', event.toString())} theme="snow" className='w-full  mt-6 rounded-md text-slate-50 text-[14px] ' />}
                </div>
                <div className='input_format border-t  border-gray-500 rounded-md  bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                    <div className='flex items-center justify-between'>
                        <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Định dạng đầu vào</h2>
                        <FontAwesomeIcon onClick={() => updateIsOpen(2, !isOpen[2])} icon={!isOpen[2] ? faAdd : faMinus} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                    </div>
                    {isOpen[2] && <ReactQuill value={value.input_format} onChange={(event) => handleChange('input_format', event.toString())} theme="snow" className='w-full  mt-6 rounded-md text-slate-50 text-[14px] ' />}
                </div>
                <div className='output_format border-t  border-gray-500 rounded-md  bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                    <div className='flex items-center justify-between'>
                        <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Định dạng đầu ra</h2>
                        <FontAwesomeIcon onClick={() => updateIsOpen(3, !isOpen[3])} icon={!isOpen[3] ? faAdd : faMinus} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2 ' />
                    </div>
                    {isOpen[3] && <ReactQuill value={value.output_format} onChange={(event) => handleChange('output_format', event.toString())} theme="snow" className='w-full  mt-6 rounded-md text-slate-50 text-[14px]' />}
                </div>
                <div className='output border-t  border-gray-500 rounded-md bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                    <div className='flex items-center justify-between'>
                        <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Đầu ra/đầu ra minh họa</h2>
                        <FontAwesomeIcon onClick={() => addTestCaseTest()} icon={faAdd} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                    </div>
                    {value.output.map((item, index) => {
                        return <div key={index} className='flex items-center justify-between'>
                            <div className='mt-4 ml-4 w-1/2'>
                                <Label className='ml-2 mb-2' htmlFor="picture">Đầu vào minh họa</Label>
                                <ReactQuill onChange={(event) => updateInputValue(index, event.toString())} value={value.input[index]} theme="snow" className='w-full mt-6 rounded-md text-slate-50 text-[14px]' />
                            </div>
                            <div className='mt-4 ml-4 w-1/2'>
                                <Label className='ml-2 mb-2' htmlFor="picture">Đầu ra minh họa</Label>
                                <ReactQuill onChange={(event) => updateOutputValue(index, event.toString())} value={item} theme="snow" className='w-full mt-6 rounded-md text-slate-50 text-[14px]' />
                            </div>
                        </div>
                    })}

                </div>
                <div className='explaintation border-t  border-gray-500 rounded-md bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                    <div className='flex items-center justify-between'>
                        <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Giải thích</h2>
                        <FontAwesomeIcon onClick={() => updateIsOpen(6, !isOpen[6])} icon={!isOpen[6] ? faAdd : faMinus} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                    </div>
                    {isOpen[6] && <ReactQuill value={value.explaintation} onChange={(event) => handleChange('explaintation', event.toString())} theme="snow" className='w-full  mt-6 rounded-md text-slate-50 text-[14px]' />}
                </div>
                <div className='testcase border-t  border-gray-500 rounded-md bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                    <div className='flex items-center justify-between'>
                        <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Thêm đầu vào/ đầu ra mong muốn</h2>
                        <FontAwesomeIcon onClick={() => addTestCase()} icon={faAdd} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                    </div>
                    {testCase.map((item, index) => {
                        return <div key={index} className='mt-4 ml-4'>
                            <div className='flex items-center justify-between'>
                                <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                                    <Label className='ml-2 mb-2' htmlFor="picture">Đầu vào</Label>
                                    <Input type="file" accept=".in" onChange={(e) => handleFileChange(1, index, e)} />
                                </div>
                                <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                                    <Label className='ml-2 mb-2' htmlFor="picture">Đầu ra</Label>
                                    <Input type="file" accept=".out" onChange={(e) => handleFileChange(2, index, e)} />
                                </div>
                                <div className="grid w-full max-w-sm items-center gap-1.5 ml-4">
                                    <Label className='ml-2 mb-2' htmlFor="picture">Loại test case</Label>
                                    <Select value={item.isLock == true ? "1" : "2"} onValueChange={value => updateIsLock(index, value)}>
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
                            </div>
                        </div>
                    })}
                </div>
                <Button variant='default' className='mt-4' onClick={() => mutate()}>Thêm bài tập</Button>
            </div>
            
        </div>
    )

}
