using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.Infrastructure
{
    public class PagedResult<T>
    {
        public class PagingInfo
        {
            public int PageNumber { get; set; }

            public int PageSize { get; set; }

            public int PageCount { get; set; }

            public long TotalCount { get; set; }

        }
        public List<T> Data { get; private set; }

        public PagingInfo Paging { get; private set; }

        public PagedResult(IList<T> items, int pageNo, int pageSize, long totalRecordCount)
        {
            Data = new List<T>(items);
            Paging = new PagingInfo
            {
                PageNumber = pageNo,
                PageSize = pageSize,
                TotalCount = totalRecordCount,
                PageCount = totalRecordCount > 0
                    ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
                    : 0
            };
        }
    }
}
