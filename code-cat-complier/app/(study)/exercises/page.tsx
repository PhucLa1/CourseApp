"use client"
import React, { useState } from 'react'
import ExerciseIntro from './_components/ExerciseIntro'
import TagFilter from './_components/TagFilter'
import { useQuery } from '@tanstack/react-query'
import { GetAllTagExercises, GetExercisesPaginated } from '@/apis/exercises.api'
import Loading from '@/components/Loading'
import toast from 'react-hot-toast'
import Pagination from '@/components/Pagination'
import { ExerciseDto } from '@/model/Exercises'
import Paginations from '@/components/Pagination'

export default function page() {
  const [currentPage, setCurrentPage] = useState<number>(2)
  const {data: dataTag, isLoading: isLoadingTag, error: errorTag} = useQuery({
    queryKey: ['tag-exercises'],
    queryFn: () => GetAllTagExercises(),
  })

  const {data: dataExercises, isLoading: isLoadingExercises, error: errorExercises} = useQuery({
    queryKey: ['exercises',currentPage],
    queryFn: () => GetExercisesPaginated(currentPage),
  })

  const onClickPaginate = (page : number) => {
    setCurrentPage(page)
  }

  if(errorTag?.message){
    toast.error(errorTag.message)
  }
  if(errorExercises?.message){
    toast.error(errorExercises.message)
  }
  console.log(dataExercises?.data.metadata)
  return (
    <div className='pb-12' style={{ minWidth: 'auto', boxSizing: 'border-box', margin: '0 auto', paddingRight: '20px', paddingLeft: '20px', maxWidth: '1240px' }}>
      {isLoadingTag || isLoadingExercises  ? <Loading/> : ''}
      <div className='' style={{ boxSizing: 'inherit', display: 'flex', position: 'relative' }}>
        <section style={{ boxSizing: 'border-box', flex: 1, marginBottom: '30px', minWidth: 0 }}>
          <div className='flex mt-10 mb-5 items-baseline'>
            <h2 style={{ fontSize: '18px', lineHeight: 1.4, fontWeight: 700 }}>Luyện tập</h2>
          </div>
          {
            dataExercises?.data.metadata.items?.map((item: any) => {
              return <ExerciseIntro key={item.id} exercise={item}  />
            })
          }
          <Paginations onClickPaginate={onClickPaginate} pagedResult={dataExercises?.data.metadata}/>
          
        </section>
        <section style={{ boxSizing: 'border-box', margin: '0 0 30px 30px', width: '280px' }}>
          <div style={{ boxSizing: 'border-box' }}>
            <div style={{ position: 'relative', transform: 'translate3d(0px, 0px, 0px)' }}>
              <div style={{ boxSizing: 'inherit' }}>
                <TagFilter tagExercises={dataTag?.data.metadata ?? []} />
              </div>
            </div>
          </div>
        </section>
      </div>
    </div>
  )
}

