
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
import {
    Pagination,
    PaginationContent,
    PaginationEllipsis,
    PaginationItem,
    PaginationLink,
    PaginationNext,
    PaginationPrevious,
} from "@/components/ui/pagination"
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "../../../../../components/ui/select"
import { useState } from "react"
import { Checkbox } from "@/components/ui/checkbox"
import Paginations from "@/components/Pagination"
import { useQuery } from "@tanstack/react-query"
import { GetUserSubmision } from "@/apis/exercises.api"
import { PagedResult } from "@/model/PagedResult"
import { Check } from "lucide-react"
import moment from "moment"


type Props = {
    data: PagedResult,
    onClickPaginate: (page: number) => void,
    onCheck: () => void,
    checked: number
}
moment.locale('vi');
export function ListSubmission({ data, onClickPaginate, onCheck,checked }: Props) {
    return (
        <div>
            <div className="mt-10">
                <h2 className="text-[18px] font-bold text-center">Danh sách giải bài tập</h2>
            </div>
            <div className="flex items-center space-x-2 mt-4 ml-4">
                <Checkbox checked={checked == 1 ? true : false} id="terms" onCheckedChange={() => onCheck()} />
                <label
                    htmlFor="terms"
                    className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                >
                    Chỉ hiển thị bài làm của tôi
                </label>
            </div>
            <Table className="mt-4 border border-gray rounded-md ">
                <TableHeader>
                    <TableRow>
                        <TableHead className="w-[100px]">STT</TableHead>
                        <TableHead></TableHead>
                        <TableHead>Tên</TableHead>
                        <TableHead>Thời gian nộp</TableHead>
                        <TableHead className="text-right">Điểm</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {data.items?.map((item: any, index) => (
                        <TableRow key={index}>
                            <TableCell className="font-medium">{index + 1}</TableCell>
                            <TableCell><img width='50px' src={`https://localhost:7130/Uploads/${item.avatar}`} alt="" /></TableCell>
                            <TableCell>{item.fullName}</TableCell>
                            <TableCell>{moment(item.createdAt).format('dddd, DD-MM-YYYY HH:mm:ss')}</TableCell>
                            <TableCell className={`text-[${item.isSuccess == true ? "#7bc043" : "#faa05e"}] text-right`}>{item.successRate}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
            <Paginations onClickPaginate={onClickPaginate} pagedResult={data} />
        </div >


    )
}
