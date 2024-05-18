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
        Task<List<CategoryDTO>> GetAll();
    }
}
