import { Label } from '@/components/ui/label';
import { ContentExercise } from '@/model/Exercises';
import { faAdd, faDeleteLeft, faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import React, { useState } from 'react'
import ReactQuill from 'react-quill';

type Props = {
    value: ContentExercise,
    handleChange: (field: keyof ContentExercise, newValue: any) => void,
    addTestCaseTest: () => void,
    removeCaseTest: (index: number) => void
}
export default function ContentExercises({ value, handleChange, addTestCaseTest,removeCaseTest }: Props) {
    const [isDisable, setIsDisable] = useState<boolean[]>([])
    const updateDisable = (index: number, newValue: boolean) => {
        setIsDisable(prevState => {
            const newState = [...prevState];
            newState[index] = newValue;
            return newState;
        });
    };
    return (
        <div>
            <div className='descritption border  border-gray-500 rounded-md bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                <div className='flex items-center justify-between'>
                    <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Yêu cầu/ Mô tả bài tập</h2>
                    <FontAwesomeIcon onClick={() => updateDisable(0, !isDisable[0])} icon={!isDisable[0] ? faPen : faDeleteLeft} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                </div>
                <ReactQuill readOnly={!isDisable[0]} value={value.description} onChange={(event) => handleChange('description', event.toString())} theme="snow" className='h-full mt-6 rounded-md text-slate-50 text-[14px]' />
            </div>
            <div className='input_format border-t  border-gray-500 rounded-md  bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                <div className='flex items-center justify-between'>
                    <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Định dạng đầu vào</h2>
                    <FontAwesomeIcon onClick={() => updateDisable(1, !isDisable[1])} icon={!isDisable[1] ? faPen : faDeleteLeft} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                </div>
                <ReactQuill readOnly={!isDisable[1]} value={value.inputFormat} onChange={(event) => handleChange('inputFormat', event.toString())} theme="snow" className='w-full  mt-6 rounded-md text-slate-50 text-[14px] ' />
            </div>
            <div className='output_format border-t  border-gray-500 rounded-md  bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                <div className='flex items-center justify-between'>
                    <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Định dạng đầu ra</h2>
                    <FontAwesomeIcon onClick={() => updateDisable(2, !isDisable[2])} icon={!isDisable[2] ? faPen : faDeleteLeft} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2 ' />
                </div>
                <ReactQuill readOnly={!isDisable[2]} value={value.outputFormat} onChange={(event) => handleChange('outputFormat', event.toString())} theme="snow" className='w-full  mt-6 rounded-md text-slate-50 text-[14px]' />
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
                            <ReactQuill readOnly={!isDisable[4 + index]} value={value.input[index]} theme="snow" className='w-full mt-6 rounded-md text-slate-50 text-[14px]' />
                        </div>
                        <div className='mt-4 ml-4 w-1/2'>
                            <Label className='ml-2 mb-2' htmlFor="picture">Đầu ra minh họa</Label>
                            <ReactQuill readOnly={!isDisable[4 + index]} value={item} theme="snow" className='w-full mt-6 rounded-md text-slate-50 text-[14px]' />
                        </div>
                        <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[18px] ml-4 mt-4' onClick={() => updateDisable(4 + index, !isDisable[4 + index])} icon={!isDisable[4 + index] ? faPen : faDeleteLeft} />
                        <FontAwesomeIcon className='hover:text-slate-50 cursor-pointer text-[18px] ml-4 mt-4' onClick={() => removeCaseTest(index)} icon={faTrash} />
                    </div>
                })}
            </div>
            <div className='explaintation border-t  border-gray-500 rounded-md bg-card mt-4 px-2 py-4 transition-all hover:-translate-y-1.5 hover:shadow-lg'>
                <div className='flex items-center justify-between'>
                    <h2 className='text-[16px] text-slate-50 font-bold ml-4'>Giải thích</h2>
                    <FontAwesomeIcon onClick={() => updateDisable(3, !isDisable[3])} icon={!isDisable[3] ? faPen : faDeleteLeft} className='hover:text-slate-50 cursor-pointer text-[18px] mr-2' />
                </div>
                <ReactQuill readOnly={!isDisable[3]} value={value.explaintation} onChange={(event) => handleChange('explaintation', event.toString())} theme="snow" className='w-full  mt-6 rounded-md text-slate-50 text-[14px]' />
            </div>
        </div>
    )
}
