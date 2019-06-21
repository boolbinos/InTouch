using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class PagedResultDTO<T>
    {
        public class PagingInfo
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }

            public int PageCount { get; set; }

            public long TotalCount { get; set; }

        }
        public List<T> Data { get; set; }

        public PagingInfo Paging { get; set; }
    }
}