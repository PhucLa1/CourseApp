import { PagedResult } from '@/model/PagedResult'
import React from 'react'
import {
    Pagination,
    PaginationContent,
    PaginationEllipsis,
    PaginationItem,
    PaginationLink,
    PaginationNext,
    PaginationPrevious,
} from "@/components/ui/pagination"

export default function Paginations ({ pagedResult, onClickPaginate }: { pagedResult: PagedResult | undefined, onClickPaginate:(page:number)=> void }) {
    if(!pagedResult) return <></>
    if(pagedResult.totalItemCount == 0) return <></>
    let numberPagesArray = [];

    for (let i = 0; i < pagedResult.pageCount; i++) {
        numberPagesArray.push(i + 1);
    }
    let firstPage = pagedResult.pageNumber < pagedResult.pageCount - 3 ? pagedResult.pageNumber - 1 : pagedResult.pageCount - 5
    let thirdPage = pagedResult.pageNumber < pagedResult.pageCount - 3 ? pagedResult.pageNumber + 2 : pagedResult.pageCount - 2

    return (
        <Pagination className="rounded-md w-2/3 ml-28 mt-2">
                <PaginationContent>
                    {
                        pagedResult.hasPreviousPage  &&  < PaginationItem onClick={() => onClickPaginate(pagedResult.pageNumber - 1)}>
                            <PaginationPrevious href="#" />
                        </PaginationItem>
                    }
                    {
                        numberPagesArray.slice(firstPage, thirdPage).map((page) => {
                            if (pagedResult.pageNumber == page) {
                                return <PaginationItem onClick={() => onClickPaginate(page)} key={page}>
                                    <PaginationLink isActive href="#">{page}</PaginationLink>
                                </PaginationItem>
                            }
                            else {
                                return <PaginationItem onClick={() => onClickPaginate(page)} key={page}>
                                    <PaginationLink href="#">{page}</PaginationLink>
                                </PaginationItem>
                            }
                        })

                    }

                    {
                        pagedResult.pageCount > 3 && pagedResult.pageNumber < pagedResult.pageCount - 4 && <PaginationItem>
                            <PaginationEllipsis />
                        </PaginationItem>
                    }
                    {
                        numberPagesArray.slice(pagedResult.pageCount - 2, pagedResult.pageCount + 1).map((page) => {
                            if (pagedResult.pageNumber == page) {
                                return <PaginationItem onClick={() => onClickPaginate(page)} key={page}>
                                    <PaginationLink isActive href="#">{page}</PaginationLink>
                                </PaginationItem>
                            }
                            else {
                                return <PaginationItem onClick={() => onClickPaginate(page)} key={page}>
                                    <PaginationLink href="#">{page}</PaginationLink>
                                </PaginationItem>
                            }
                        })
                    }
                    {
                        pagedResult.hasNextPage  && <PaginationItem onClick={() => onClickPaginate(pagedResult.pageNumber + 1)}>
                            <PaginationNext href="#" />
                        </PaginationItem>
                    }
                </PaginationContent>
            </Pagination>
    )
}
