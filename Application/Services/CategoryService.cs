using Application.DTOs;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.IRepositories;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryDTO>> GetAll()
        {
            var categories = await _categoryRepository.GetAll();
            var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
            return categoryDTOs;
        }
    }
}
