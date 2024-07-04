using Dtos.Results;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class HandlePagination<T> where T : class
    {
        public static PagedResult<T> PageList(int pageNumber, int totalItemCount, int pageSize, IEnumerable<T> metadata)
        {
            int pageCount = (int)Math.Ceiling((double)totalItemCount / pageSize);
            return new PagedResult<T>
            {
                PageCount = pageCount,
                TotalItemCount = totalItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                HasPreviousPage = pageNumber == 1 ? false : true,
                HasNextPage = pageNumber == pageCount ? false : true,
                IsFirstPage = pageNumber == 1 ? true : false,
                IsLastPage = pageNumber == pageCount ? true : false,
                Items = metadata
            };
        }
    }
}
