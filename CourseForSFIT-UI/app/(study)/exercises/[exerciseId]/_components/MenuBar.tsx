import React from 'react'
import {
    Menubar,
    MenubarMenu,
    MenubarTrigger,
} from "@/components/ui/menubar"
type Props = {
    onSelectContent: (data: number) => void
}


export default function MenuBar({ onSelectContent }: Props) {
    const handleClick = (data: number) => {
        let menuBar = document.querySelectorAll('.menubar')
        for (let i = 0; i < menuBar.length; i++) {
            menuBar[i].classList.remove('bg-gray-700')
        }
        menuBar[data - 1].classList.add('bg-gray-700')
        onSelectContent(data)
    }
    return (
        <Menubar className="">
            <MenubarMenu>
                <MenubarTrigger onClick={() => handleClick(1)} className="menubar w-1/3 text-center bg-gray-700">Đề bài</MenubarTrigger>
            </MenubarMenu>
            <MenubarMenu>
                <MenubarTrigger onClick={() => handleClick(2)} className="menubar w-1/3 text-center">Bình luận</MenubarTrigger>
            </MenubarMenu>
            <MenubarMenu>
                <MenubarTrigger onClick={() => handleClick(3)} className="menubar w-1/3 text-center">Danh sách</MenubarTrigger>
            </MenubarMenu>
        </Menubar>
    )
}
