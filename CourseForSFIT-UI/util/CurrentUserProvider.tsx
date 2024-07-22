"use client"
import { GetCurrentUser } from '@/apis/auth.api'
import Loading from '@/components/Loading'
import { UserCurrent } from '@/model/User'
import { useQuery } from '@tanstack/react-query'
import React, { ReactNode, useState, useEffect, createContext, useContext } from 'react'

const CurrentUserContext = createContext<any | undefined>(undefined);
export const useCurrentUser = () => {
    return useContext(CurrentUserContext);
}
export default function CurrentUserProvider({ children }: { children: ReactNode }) {
    const [currentUser, setCurrentUser] = useState<UserCurrent>()
    const { data, isSuccess, isLoading } = useQuery({
        queryKey: ['current-user'],
        queryFn: () => GetCurrentUser()
    })
    useEffect(() => {
        if (isSuccess) {
            setCurrentUser(data.data.metadata)
        }
    }, [isSuccess, data])

    if (isLoading) return <Loading/>
    return (
        <CurrentUserContext.Provider value={{ currentUser }}>
            {children}
        </CurrentUserContext.Provider>
    )
}
