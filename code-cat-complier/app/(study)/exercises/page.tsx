"use client"
import React, { useState } from 'react'
import ExerciseIntro from './_components/ExerciseIntro'
import TagFilter from './_components/TagFilter'
import { useQuery } from '@tanstack/react-query'
import { GetAllTagExercises, GetExercisesPaginatedByOptions } from '@/apis/exercises.api'
import Loading from '@/components/Loading'
import toast from 'react-hot-toast'
import Paginations from '@/components/Pagination'
import { ExerciseRequest } from '@/model/Exercises'
import { number } from 'zod'

export default function page() {
  const [IsChecked, setIsChecked] = useState<boolean[]>([])
  const [exerciseRequest, setExerciseRequest] = useState<ExerciseRequest>({})
  const [currentPage, setCurrentPage] = useState<number>(1)
  const { data: dataTag, isLoading: isLoadingTag, error: errorTag } = useQuery({
    queryKey: ['tag-exercises'],
    queryFn: () => GetAllTagExercises(),
  })
  console.log(IsChecked)
  const { data: dataExercises, isLoading: isLoadingExercises, error: errorExercises } = useQuery({
    queryKey: ['exercises', currentPage, exerciseRequest],
    queryFn: () => GetExercisesPaginatedByOptions(exerciseRequest, currentPage),
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

  if (errorTag?.message) {
    toast.error(errorTag.message)
  }
  if (errorExercises?.message) {
    toast.error(errorExercises.message)
  }
  return (
    <div className='pb-12' style={{ minWidth: 'auto', boxSizing: 'border-box', margin: '0 auto', paddingRight: '20px', paddingLeft: '20px', maxWidth: '1240px' }}>
      {isLoadingTag || isLoadingExercises ? <Loading /> : ''}
      <div className='' style={{ boxSizing: 'inherit', display: 'flex', position: 'relative' }}>
        <section style={{ boxSizing: 'border-box', flex: 1, marginBottom: '30px', minWidth: 0 }}>
          <div className='flex mt-10 mb-5 items-baseline'>
            <h2 style={{ fontSize: '18px', lineHeight: 1.4, fontWeight: 700 }}>Luyện tập</h2>
          </div>
          {
            dataExercises?.data.metadata.items?.map((item: any) => {
              return <ExerciseIntro key={item.id} exercise={item} />
            })
          }
          <Paginations onClickPaginate={onClickPaginate} pagedResult={dataExercises?.data.metadata} />

        </section>
        <section style={{ boxSizing: 'border-box', margin: '0 0 30px 30px', width: '280px' }}>
          <div style={{ boxSizing: 'border-box' }}>
            <div style={{ position: 'relative', transform: 'translate3d(0px, 0px, 0px)' }}>
              <div style={{ boxSizing: 'inherit' }}>
                <TagFilter onFindByName={onFindByName} onChooseFilter={onChooseFilter} tagExercises={dataTag?.data.metadata ?? []} isChecked={IsChecked} name={exerciseRequest.name ?? ""} />
              </div>
            </div>
          </div>
        </section>
      </div>
    </div>
  )
}

