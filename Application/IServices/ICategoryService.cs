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
        //Task<CategoryDTO> GetDetailCategoryById(int id);
        Task<CategoryDTO> Add(CategoryDTO request);

        Task<CategoryDTO> Update(UpdateCategoryDTO request);

        Task<CategoryDTO> Delete(int id);

        Task<List<CategoryDTO>> GetByStatus(bool? IsDeleted);
    }
}
