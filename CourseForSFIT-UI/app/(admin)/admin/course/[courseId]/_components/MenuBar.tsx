

import React from 'react'
import {
    Menubar,
    MenubarMenu,
    MenubarTrigger,
} from "@/components/ui/menubar"
export default function MenuBar({onSetShow} : {onSetShow:(data: number) => void}) {
    return (
        <Menubar className="w-2/3">
            <MenubarMenu>
                <MenubarTrigger onClick={() => onSetShow(1)} className='menubar w-1/2 text-center bg-gray-700"'>
                    <div  className='header flex items-center justify-between'>
                        <h2 className='text-[20px] text-slate-50 font-bold'>Sửa thông tin khóa học</h2>
                    </div>
                </MenubarTrigger>
            </MenubarMenu>
            <MenubarMenu>
                <MenubarTrigger onClick={() => onSetShow(2)} className='menubar w-1/2 text-center bg-gray-700"'>
                    <div  className='header flex items-center justify-between'>
                        <h2 className='text-[20px] text-slate-50 font-bold'>Chương trình khóa học</h2>
                    </div>
                </MenubarTrigger>
            </MenubarMenu>
        </Menubar>
    )
}
