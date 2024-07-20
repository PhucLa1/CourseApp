"use client"
import React, { useEffect, useRef, useState } from 'react'
import {
    ResizableHandle,
    ResizablePanel,
    ResizablePanelGroup,
} from "@/components/ui/resizable"
import Editor from '@monaco-editor/react';
import { useTheme } from 'next-themes'
import { Loader, Mail, Play, TriangleAlert } from 'lucide-react'
import { codeSnippets, languageOptions } from '@/config/config'
import { selectedLanguageOptionProps } from '@/model/selectedLanguageOptionProps'
import { compileCode } from '@/actions/complie'
import toast from 'react-hot-toast'
import { TestCaseData } from '@/config/testcasedata'
import { testCase } from '@/model/testCase'
import { ModeToggleBtn } from '@/components/mode-toggle-btn';
import SelectLanguages from './_components/SelectLanguages';
import MenuBar from './_components/MenuBar';
import Topic from '@/app/(study)/exercises/[exerciseId]/_components/Topic';
import { ListSubmission } from './_components/ListSubmission';
import { Button } from '@/components/ui/button';
import { useMutation, useQuery } from '@tanstack/react-query';
import { GetCommentExercise, GetContentCodes, GetTestCaseExerciseNotLock, GetTopicExercise, GetUserSubmision, SolveTestCase } from '@/apis/exercises.api';
import Loading from '@/components/Loading';
import Comments from './_components/Comments';




export default function page({ params }: { params: { exerciseId: number } }) {
    const { theme } = useTheme()
    const [sourceCode, setSourceCode] = useState<string>("")
    const editorRef = useRef(null);
    const [languageOption, setLanguageOption] = useState(languageOptions[0])
    //const language = languageOption.language
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState(false)
    const [output, setOutput] = useState<String[] | any>(null)
    const [testCase, setTestCase] = useState<testCase>(TestCaseData[0])
    const [contentIndex, setContentIndex] = useState(1)
    const [addComment, setAddComment] = useState<number>(0)
    const [runOutput, setRunOutput] = useState<boolean[]>([])
    const [currentPage, setCurrentPage] = useState<number>(1)
    const [solve, setSolve] = useState<number>(0)
    const [check, setCheck] = useState<number>(0)


    function handleEditorDidMount(editor: any) {
        editorRef.current = editor;
        editor.focus()
    }
    function handleOnChange(value: string | undefined) {
        if (value) {
            setSourceCode(value)
        }
    }
    function onSelect(value: selectedLanguageOptionProps | any) {
        setLanguageOption(value)
        setSourceCode(codeSnippets[value.language])
    }
    function handleChangeContent(index: number) {
        if (!TestCaseData[index].isLock) {
            setTestCase(TestCaseData[index])
        }
    }
    const handleSelectContent = (content: number) => {
        setContentIndex(content);
    };
    async function executeCode() {
        setLoading(true)
        const requestData = {
            "language": languageOption.language,
            "version": languageOption.version,
            "files": [
                {
                    "content": sourceCode
                }
            ],
            "stdin": "3\n4\n"
        }
        try {
            const result = await compileCode(requestData)
            setOutput(result.run.output)
            console.log(result.run)
            console.log(testCase.expectOutput.split("\n"))
            setLoading(false)
            setError(false)
            toast.success('Complie successfully!')
        } catch (error) {
            setError(true)
            console.log(error)
            toast.error('Failed to compile!')
            setLoading(false)
        }
    }
    const onAddComment = () => {
        setAddComment(addComment + 1)
    }
    const getSize = (event: any) => {
        console.log('event', event.target)
        const output = document.getElementById('output');
        //console.log('output', output)

    }
    const onClickPaginate = (page: number) => {
        setCurrentPage(page)
    }
    const onCheck = () => {
        setCheck(check == 1 ? 0 : 1)
    }

    //Gọi API
    const { data: dataTopic, isLoading: isLoadingTopic } = useQuery({
        queryKey: ['topic-exercises', params.exerciseId],
        queryFn: () => GetTopicExercise(params.exerciseId)
    })
    const { data: dataComment, isLoading: isLoadingComment } = useQuery({
        queryKey: ['comment-exercises', params.exerciseId, addComment],
        queryFn: () => GetCommentExercise(params.exerciseId)
    })
    const { data: dataTestCase, isLoading: isLoadingTestCase } = useQuery({
        queryKey: ['test-cases', params.exerciseId],
        queryFn: () => GetTestCaseExerciseNotLock(params.exerciseId)
    })
    const { data: dataContentCodes, isLoading: isLoadingContentCodes, isSuccess } = useQuery({
        queryKey: ['content-codes', params.exerciseId],
        queryFn: () => GetContentCodes(params.exerciseId)
    })
    const { data: dataSubmission, isLoading: isLoadingSubmission } = useQuery({
        queryKey: ['data-submission', solve, check, currentPage],
        queryFn: () => GetUserSubmision(params.exerciseId, check, currentPage)
    })
    const { mutate: mutateRun, isPending: isPendingRun } = useMutation({
        mutationFn: () => {
            return SolveTestCase({
                exerciseId: params.exerciseId,
                contentCode: sourceCode,
                language: languageOption.language,
                version: languageOption.version,
                avatar: languageOption.avatar
            })
        },
        onSuccess(data) {
            if (data.data.isSuccess == true) {
                toast.success("Đã vượt qua tất cả các test case")
            }
            setRunOutput(data.data.metadata)
            setSolve(solve + 1)
        },
    })
    useEffect(() => {
        if (dataContentCodes) {
            setSourceCode(dataContentCodes.data.metadata.contentCode ?? codeSnippets['javascript']);
            setLanguageOption({
                language: dataContentCodes.data.metadata.language ?? languageOptions[0].language,
                version: dataContentCodes.data.metadata.version ?? languageOptions[0].version,
                avatar: dataContentCodes.data.metadata.avatar ?? languageOptions[0].avatar,
                aliases: [],
                runtime: ""
            });

        }
    }, [dataContentCodes]);
    return (
        <div className='min-h-screen dark:bg-slate-900 rounded-2xl shadow-2xl py-6 px-8'>
            {isLoadingTopic || isLoadingComment || isLoadingTestCase || isLoadingContentCodes ? <Loading /> : <></>}
            <div className="flex items-center justify-between pb-3">
                <h2 className='scroll-m-20 text-2xl font-semibold tracking-tight first:mt-0'>UTC - SFIT</h2>
                <div className="flex items-center space-x-2">
                    <ModeToggleBtn />
                    <div className='w-[230px]'>
                        <SelectLanguages onSelect={onSelect} selectedLanguageOption={languageOption} />
                    </div>
                </div>
            </div>
            <div className=" bg-slate-400 dark:bg-slate-950 p-3 rounded-2xl">
                <ResizablePanelGroup
                    direction="horizontal"
                    className="w-full rounded-lg border dark:bg-slate-900"
                >
                    <ResizablePanel className='' defaultSize={40} minSize={35}>
                        <MenuBar onSelectContent={handleSelectContent} />
                        {contentIndex == 1 && <Topic content={dataTopic?.data.metadata ?? {
                            description: '',
                            constraints: '',
                            inputFormat: '',
                            outputFormat: '',
                            input: [],
                            output: [],
                            explaintation: ''
                        }} />}
                        {contentIndex == 2 && <Comments onAddComment={onAddComment} commentExercise={dataComment?.data.metadata} exerciseId={params.exerciseId} />}
                        {dataSubmission?.data.metadata && contentIndex == 3 && <ListSubmission checked={check} onCheck={onCheck} data={dataSubmission?.data.metadata} onClickPaginate={onClickPaginate} />}
                        {contentIndex == 4 && 4}
                    </ResizablePanel>
                    <ResizableHandle withHandle />
                    <ResizablePanel defaultSize={60} minSize={35}>
                        <ResizablePanelGroup direction="vertical">
                            <ResizablePanel defaultSize={65} minSize={40}>
                                <Editor
                                    theme={theme == 'dark' ? 'vs-dark' : 'vs-light'}
                                    height="100vh"
                                    defaultLanguage={languageOption.language}
                                    defaultValue={sourceCode}
                                    onMount={handleEditorDidMount}
                                    value={sourceCode}
                                    onChange={handleOnChange}
                                    language={languageOption.language}
                                />
                            </ResizablePanel>
                            <ResizableHandle withHandle />
                            <ResizablePanel id='output' className='flex flex-col max-h-[310px]' defaultSize={35} minSize={10} onResize={(e) => { getSize(e) }}>
                                <div className="flex items-center justify-between bg-slate-400 dark:bg-slate-950 px-6 py-2">
                                    <h2>Output</h2>
                                    <div className="flex items-center justify-center ">
                                        {loading ? (<Button disabled size={"sm"} className='dark:bg-purple-600 dark:hover:bg-purple-700 text-slate-100 bg-slate-800 hover:bg-slate-900'>
                                            <Loader className='w-4 h-4 mr-2 animate-spin'></Loader>
                                            <span>Running please wait ...</span>
                                        </Button>) : (<Button onClick={executeCode} size={"sm"} className='dark:bg-purple-600 dark:hover:bg-purple-700 text-slate-100 bg-slate-800 hover:bg-slate-900'>
                                            <Play className='w-4 h-4 mr-2'></Play>
                                            <span>Chạy thử</span>
                                        </Button>)}
                                        {isPendingRun ? (<Button disabled size={"sm"} className='dark:bg-purple-600 dark:hover:bg-purple-700 text-slate-100 bg-slate-800 hover:bg-slate-900'>
                                            <Loader className='w-4 h-4 mr-2 animate-spin'></Loader>
                                            <span>Running please wait ...</span>
                                        </Button>) : <Button onClick={() => mutateRun()} size={"sm"} className='dark:bg-green-600 dark:hover:bg-green-700 text-slate-100 bg-slate-800 hover:bg-slate-900 ml-1'>
                                            <Play className='w-4 h-4 mr-2'></Play>
                                            <span>Nộp bài</span>
                                        </Button>}
                                    </div>
                                </div>
                                <div className="flex-1 flex flex-col">
                                    <div className='border-b border-gray-300 rounded-md p-1 w-full'>
                                        <span>KIỂM THỬ</span>
                                    </div>
                                    <span className='text-sm'>Vui lòng chạy thử code của bạn trước</span>
                                    <div className="flex-1 flex items-center justify-center h-full">
                                        <ul className='w-1/5 overflow-y-auto  h-[90%] overflow-x-hidden'>
                                            {dataTestCase?.data.metadata.testCaseDtos.map((item, index) => {
                                                return (
                                                    <li key={index} onClick={() => handleChangeContent(index)} className='mx-2 my-1 w-full text-sm text-clip'>
                                                        <Button variant="outline">
                                                            <img src='/logo/unlock.png' className="mr-2 h-4 w-4" />
                                                            Kiểm thử {index + 1}
                                                            {runOutput[index] != null && <img src={runOutput[index] == true ? '/logo/correct.png' : '/logo/cross.png'} className="ml-2 h-4 w-4" />}
                                                        </Button>
                                                    </li>
                                                )
                                            })}
                                            {dataTestCase?.data.metadata.totalTestCaseLockCounts.map((item, index) => {
                                                const length = dataTestCase?.data.metadata.testCaseDtos.length
                                                console.log(runOutput[index + length], index + length)
                                                return (
                                                    <li key={index} onClick={() => handleChangeContent(index)} className='mx-2 my-1 w-full text-sm text-clip'>
                                                        <Button variant="outline">
                                                            <img src='/logo/lock.png' className="mr-2 h-4 w-4" />
                                                            Kiểm thử {index + 1}
                                                            {runOutput[index + length] != null && <img src={runOutput[index + length] == true ? '/logo/correct.png' : '/logo/cross.png'} className="ml-2 h-4 w-4" />}
                                                        </Button>
                                                    </li>
                                                )
                                            })}
                                        </ul>
                                        <div className='border-b border-gray-300 rounded-md px-6 space-y-2 divide-y-2 w-4/5 h-[85%] mr-4 overflow-y-auto'>
                                            <p className='text-sm text-center pt-1'>Đầu vào : {testCase?.input}</p>
                                            <p className='text-sm text-center pt-1'>Đẩu ra mong muốn : {testCase?.expectOutput}</p>
                                            <p className='text-sm text-center pt-1'>Đẩu ra thực tế : {!error ? output?.split("\n").join(" ") : 'Lỗi'}</p>
                                            <p className='text-sm text-center pt-1'>Thời gian chạy : </p>
                                        </div>
                                    </div>

                                </div>
                            </ResizablePanel>
                        </ResizablePanelGroup>
                        <div className="space-y-3 bg-slate-300 dark:bg-slate-900 min-h-screen"></div>
                    </ResizablePanel>
                </ResizablePanelGroup>
            </div>
        </div>
    )
}

