using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IProductService
    {
        Task<Pagination<ProductDTO>> Get(int pageIndex, int pageSize);

        Task<Pagination<ProductDTO>> GetWithFilter(Expression<Func<Product, bool>> filter, int pageIndex, int pageSize);

        Task<ProductDTO> Get(int id);
        Task<int> Add(ProductDTO request);
        Task<int> Update(ProductDTO request);
        Task<int> Delete(int id);
    }
}
