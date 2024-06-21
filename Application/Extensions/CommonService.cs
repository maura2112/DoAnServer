using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public class PaginationService<T>
    {
        public async Task<Pagination<T>> ToPagination(List<T> itemDTO , int pageIndex, int pageSize)
        {
            var items =  itemDTO.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize).ToList();
            var result = new Pagination<T>()
            {
                TotalItemsCount = items.Count,
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return result;
        }
    }
}
