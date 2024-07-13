using Application.DTOs;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllHomePage();
        Task<Pagination<CategoryDTO>> GetAllPagination(int pageIndex, int pageSize);
        //Task<CategoryDTO> GetDetailCategoryById(int id);
        Task<CategoryDTO> Add(UpdateCategoryDTO request);

        Task<CategoryDTO> Update(UpdateCategoryDTO request);

        Task<CategoryDTO> Delete(int id);
        Task<CategoryDTO> RestoreDeleted(int id);

        Task<Pagination<CategoryDTO>> GetByStatus(bool? IsDeleted, int pageIndex, int pageSize);
    }
}
