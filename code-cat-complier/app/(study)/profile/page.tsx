
"use client"
import React, { useEffect, useState } from 'react'
import Info from './_components.tsx/Info'
import Achievements from './_components.tsx/Achievements'
import Certificate from './_components.tsx/Certificate'
import CourseProgress from './_components.tsx/CourseProgress'
import ContestParticipate from './_components.tsx/ContestParticipate'
import ExerciseInfo from './_components.tsx/ExerciseInfo'
import { GetCurrentUserInfo } from '@/apis/user.api'
import Loading from '@/components/Loading'
import { useQuery } from '@tanstack/react-query'

export default function page() {
  const [update, setUpdate] = useState<number>(0)
  const { data: dataUserInfo, isLoading: isLoadingUserInfo } = useQuery({
    queryKey: ['user-info', update],
    queryFn: () => GetCurrentUserInfo()
  })
  const onUpdate = () => {
    setUpdate(update + 1)
  }
  return (
    <div style={{ flexGrow: 1 }}>
      <article style={{ minWidth: 'auto', fontWeight: 400, lineHeight: '1.2em', marginTop: '2em' }}>
        <div style={{ display: 'flex', boxSizing: 'border-box', margin: '0 auto', paddingRight: '20px', paddingLeft: '20px', maxWidth: '1440px', minWidth: 'auto' }}>
          <div className='mb-12' style={{ display: 'flex', alignItems: 'flex-start', flexDirection: 'column', gap: '2rem', boxSizing: 'border-box', width: '380px', alignSelf: 'flex-start', flexShrink: 0 }}>
            {isLoadingUserInfo && <Loading/>}
            {dataUserInfo?.data.metadata && <Info onUpdate={onUpdate} userInfo={dataUserInfo?.data.metadata} />}
            {dataUserInfo?.data.metadata.achivementsDeserialize && <Achievements onUpdate={onUpdate} achievements={dataUserInfo?.data.metadata.achivementsDeserialize}/>} 
            <Certificate />
          </div>
          <div style={{ flex: 1, marginLeft: '2rem' }}>
            <CourseProgress />
            <ContestParticipate />
            <ExerciseInfo />
          </div>
        </div>
      </article>
    </div>
  )
}
