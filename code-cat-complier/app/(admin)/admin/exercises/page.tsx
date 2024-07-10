import React from 'react'
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
export default function page() {
  return (
    <div className='w-full'>
      <div className='header flex items-center justify-between'>
        <h2 className='text-[20px] text-slate-50 font-bold'>Danh sách bài tập</h2>
      </div>
      <div className='content pb-2'>
        <Table>
          <TableCaption>Danh sách bài tập</TableCaption>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[50px]">STT</TableHead>
              <TableHead>Tên </TableHead>
              <TableHead>Người tạo</TableHead>
              <TableHead>Người sửa</TableHead>
              <TableHead>Nhãn dán</TableHead>
              <TableHead className="text-right">Hành động</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>

          </TableBody>
        </Table>
      </div>
    </div>
  )
}
