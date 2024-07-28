"use client"
import React, { useEffect, useState } from 'react'
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
import { AddExercise, GetAllTagExercises, GetExerciseInfoAdmin, GetTagExerciseByExerciseId, GetTopicExercise, UpdateExercise, } from '@/apis/exercises.api';
import Loading from '@/components/Loading';
import toast from 'react-hot-toast';
import { Button } from '@/components/ui/button';
import FirstArticle from './_components/FirstArticle';
import { Tags } from 'lucide-react';
import ContentExercises from './_components/ContentExercises';
import { ContentExercise } from '@/model/Exercises';
import TestCase from './_components/TestCase';



export default function page({ params }: { params: { exerciseId: number } }) {
    const [exerciseName, setExerciseName] = useState<string>("")
    const [difficult, setDifficult] = useState<number>(0)
    const [tagIds, setTagIds] = useState<number[]>([])
    const [value, setValue] = useState<ContentExercise>({
        description: '',
        constraints: '',
        inputFormat: '',
        outputFormat: '',
        input: [],
        output: [],
        explaintation: ''
    });
    const { data: dataTag, isLoading: isLoadingTag } = useQuery({
        queryKey: ['tag-exercise'],
        queryFn: () => GetAllTagExercises()
    })
    const { data: dataTopic, isLoading: isLoadingTopic } = useQuery({
        queryKey: ['topic-exercise'],
        queryFn: () => GetExerciseInfoAdmin(params.exerciseId)
    })
    const { data: dataValueTags, isLoading: isLoadingValueTags } = useQuery({
        queryKey: ['tag-exercise-valueOf'],
        queryFn: () => GetTagExerciseByExerciseId(params.exerciseId)
    })
    const { mutate: mutateExercise, isPending: isPendingExercise } = useMutation({
        mutationFn: () => {
            console.log({
                topicExercise: value,
                name: exerciseName,
                difficult: difficult,
                tagIds: tagIds
            })
            return UpdateExercise(params.exerciseId, {
            topicExercise: value,
            name: exerciseName,
            difficult: difficult,
            tagIds: tagIds
        })},
        onSuccess(data) {
            if (data.data.isSuccess == true) {
                toast.success("Thành công chỉnh sửa bài tập")
            }else{
                toast.error(data.data.message[0])
            }
        },
    })
    const handleChange = (field: keyof ContentExercise, newValue: any) => {
        setValue(prevValue => ({
            ...prevValue,
            [field]: newValue
        }));
    };
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
    const removeTestCaseTest = (index: number) => {
        setValue(prevValue => ({
            ...prevValue,
            output: prevValue.output.filter((_, i) => i !== index),
            input: prevValue.input.filter((_, i) => i !== index)
        }));
    };
    useEffect(() => {
        if (dataTopic) {
            setValue(dataTopic.data.metadata.topicExercise)
            setExerciseName(dataTopic.data.metadata.name)
            setDifficult(dataTopic.data.metadata.difficult)
        }
    }, [dataTopic])
    useEffect(() => {
        if (dataValueTags) {
            setTagIds(dataValueTags.data.metadata.map((tagValue) => {
                return tagValue.id
            }))
        }
    }, [dataValueTags])
    const onSetExerciseName = (value: string) => {
        setExerciseName(value)
    }
    const onHandleTagIds = (value: number[]) => {
        setTagIds(value)
    }
    const onSetDifficult = (value: number) => {
        setDifficult(value)
    }
    return (
        <div>
            {isPendingExercise ?? <Loading/>}
            <div className='header flex items-center justify-between'>
                <h2 className='text-[20px] text-slate-50 font-bold'>Chỉnh sửa bài tập</h2>
            </div>
            <div className='first-article mt-4 rounded-md'>
                <div className='header flex items-center justify-between'>
                    <h2 className='text-[18px] text-slate-50 font-bold'>Chỉnh sửa tên, độ khó, nhãn dán</h2>
                </div>
                {isLoadingTag || isLoadingValueTags || isLoadingTopic ? <Loading /> :
                    <FirstArticle defaultValueTag={dataValueTags?.data.metadata.map((tag) => {
                        return {
                            label: tag.name,
                            value: tag.id
                        }
                    }) ?? []} tags={dataTag?.data.metadata.map((tag) => {
                        return {
                            label: tag.name,
                            value: tag.id
                        }
                    }) ?? []} exerciseName={exerciseName} difficult={difficult} onSetExerciseName={onSetExerciseName} onSetDifficult={onSetDifficult} onHandleTagIds={onHandleTagIds} />}
            </div>

            <div className='content-exercise mt-4 rounded-md'>
                <div className='header flex items-center justify-between'>
                    <h2 className='text-[18px] text-slate-50 font-bold'>Chỉnh sửa đề bài</h2>
                </div>
                {dataTopic?.data.metadata.topicExercise != null && <ContentExercises removeCaseTest={removeTestCaseTest} addTestCaseTest={addTestCaseTest} value={value} handleChange={handleChange} />}
                <Button variant='default' className='mt-4' onClick={() => mutateExercise()}>Chỉnh sửa bài tập</Button>
            </div>
            <TestCase exerciseId={params.exerciseId} />
        </div>
    )
}
