"use client"
import React, { useState } from 'react'
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableFooter,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import TagFilter from '@/app/(study)/exercises/_components/TagFilter'
import { Pagination } from '@/components/ui/pagination'
import Paginations from '@/components/Pagination'
import { ExerciseRequest } from '@/model/Exercises'
import { DeleteAdminExercises, GetAdminExercisesPaginatedByOptions, GetAllTagExercises } from '@/apis/exercises.api'
import { useMutation, useQuery } from '@tanstack/react-query'
import Loading from '@/components/Loading'
import AlertDialogs from '@/components/AlertDialogs'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTrash } from '@fortawesome/free-solid-svg-icons'
import toast from 'react-hot-toast'
export default function page() {
  const [IsChecked, setIsChecked] = useState<boolean[]>([])
  const [exerciseRequest, setExerciseRequest] = useState<ExerciseRequest>({})
  const [currentPage, setCurrentPage] = useState<number>(1)
  const [action, setAction] = useState<number>(0)
  const { data: dataTag, isLoading: isLoadingTag, error: errorTag } = useQuery({
    queryKey: ['tag-exercises'],
    queryFn: () => GetAllTagExercises(),
  })
  const { data: dataAdminExercise, isLoading: isLoadingAdminExercise, error: errorAdminExercise } = useQuery({
    queryKey: ['admin-exercises', currentPage, exerciseRequest, action],
    queryFn: () => GetAdminExercisesPaginatedByOptions(exerciseRequest, currentPage),
  })
  const onClickPaginate = (page: number) => {
    setCurrentPage(page)
  }

  const onFindByName = (value: string) => {
    setCurrentPage(1)
    setExerciseRequest(prevState => ({
      ...prevState,
      name: value
    }));
  }

  const onChooseFilter = (option: number, value: any, numerical: number) => {
    setCurrentPage(1)
    setIsChecked(prevState => {
      const newState = [...prevState];
      newState[numerical] = !newState[numerical];
      return newState;
    });
    if (option === 0) {
      setExerciseRequest(prevState => ({
        ...prevState,
        status: 1
      }));
    }
    else if (option === 1) {
      setExerciseRequest(prevState => ({
        ...prevState,
        difficultLevel: (prevState.difficultLevel && prevState.difficultLevel.includes(value))
          ? prevState.difficultLevel.filter(diffLevel => diffLevel !== value)
          : [...(prevState.difficultLevel || []), value]
      }));
    } else {
      setExerciseRequest(prevState => ({
        ...prevState,
        tagId: (prevState.tagId && prevState.tagId.includes(value))
          ? prevState.tagId.filter(tagId => tagId !== value)
          : [...(prevState.tagId || []), value]
      }));
    }
  }
  const { mutate: mutateDelete, isPending: isPendingDelete } = useMutation({
    mutationFn: (id: number) => {
      return DeleteAdminExercises(id)
    },
    onSuccess(data) {
      if (data.data.isSuccess) {
        if (dataAdminExercise?.data.metadata.items?.length === 1) {
          setCurrentPage(currentPage - 1)
        }

        setAction(action + 1)
        toast.success("Xóa nhãn dán thành công")
      }
    },
  })
  const onDelete = (id: number) => {
    mutateDelete(id)
  }

  return (
    <div className='w-full'>
      <div className='header flex items-center justify-between'>
        <h2 className='text-[20px] text-slate-50 font-bold'>Danh sách bài tập</h2>
      </div>
      {isLoadingTag || isLoadingAdminExercise ? <Loading /> : <></>}
      <div className='content pb-2 flex'>
        <TagFilter tagExercises={dataTag?.data.metadata ?? []} onChooseFilter={onChooseFilter} isChecked={IsChecked} onFindByName={onFindByName} name={exerciseRequest.name ?? ""} />
        <div className='w-full'>
          <Table>
            <TableCaption>Danh sách bài tập</TableCaption>
            <TableHeader>
              <TableRow>
                <TableHead className="w-[50px]">STT</TableHead>
                <TableHead>Tên </TableHead>
                <TableHead>Độ khó</TableHead>
                <TableHead>Số người tham gia</TableHead>
                <TableHead>AC</TableHead>
                <TableHead>Nhãn dán</TableHead>
                <TableHead>Hành động</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {dataAdminExercise?.data.metadata.items?.map((item: any, index: number) => (
                <TableRow key={index}>
                  <TableCell className="font-medium">{index + 1}</TableCell>
                  <TableCell>{item.exerciseName}</TableCell>
                  <TableCell className={`text-[${item.difficultLevel == 1 ? "#7bc043" : item.difficultLevel == 2 ? "#faa05e" : "#e64f4a"}]`}>{item.difficultLevel == 1 ? "Dễ" : item.difficultLevel == 2 ? "Trung bình" : "Khó"}</TableCell>
                  <TableCell>{item.numberParticipants}</TableCell>
                  <TableCell>%{item.successRate}</TableCell>
                  <TableCell className='text-left'><div className='flex-wrap h-[25px] overflow-hidden justify-center flex gap-1 text-xs text-[#333]'>
                    {item.tags.map((tag: any) => {
                      return <div className='bg-[#F5F6F7] px-[2px] h-[25px] flex items-center rounded-md'>{tag}</div>
                    })}
                  </div>
                  </TableCell>
                  <TableCell>
                    <div className='flex items-center justify-center pl-2'>
                      <AlertDialogs
                        trigger={<FontAwesomeIcon className='ml-6 cursor-pointer hover:text-slate-50' icon={faTrash} />}
                        title="Bạn có chắc chắn muốn xóa nhãn dán này không"
                        description='Sau khi thực hiện hành động này sẽ không thể khôi phục'
                        actionTitle='Đồng ý'
                        cancelTitle='Hủy'
                        onAction={() => onDelete(item.id)} />
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
          <Paginations pagedResult={dataAdminExercise?.data.metadata} onClickPaginate={onClickPaginate} />
        </div>
      </div>
    </div>
  )
}
