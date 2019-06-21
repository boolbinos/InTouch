using System;
using System.Linq;
using System.Text;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Infrastructure;

namespace Materialise.InTouch.BLL.Mappers
{
    public class PagedResultMapper
    {
        public static PagedResultDTO<TDto> MapToDTO<TEntity, TDto>(PagedResult<TEntity> pagedResult, Func<TEntity, TDto> dataDtoMapper)
        {
            if (pagedResult == null)
            {
                throw new ArgumentNullException(nameof(pagedResult));
            }

            return new PagedResultDTO<TDto>
            {
                Paging = new PagedResultDTO<TDto>.PagingInfo
                {
                    PageCount = pagedResult.Paging.PageCount,
                    PageNumber = pagedResult.Paging.PageNumber,
                    PageSize = pagedResult.Paging.PageSize,
                    TotalCount = pagedResult.Paging.TotalCount
                },

                Data = pagedResult.Data.Select(dataDtoMapper).ToList()
            };
        }
    }
}
