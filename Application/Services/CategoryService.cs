using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICurrentUserService _currentUserService;
        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CategoryDTO> Add(CategoryDTO request)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            var category = _mapper.Map<Category>(request);
            category.CategoryName = request.CategoryName;
            category.Image = request.Image;

            await _categoryRepository.AddAsync(category);
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task<CategoryDTO> Delete(int id)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found.");
            }
            category.IsDeleted = true;
            _categoryRepository.Update(category);
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task<List<CategoryDTO>> GetAllHomePage()
        {
            try
            {
                var categories = await _categoryRepository.GetByStatus(null);
                var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
                return categoryDTOs;
            }
            catch (Exception ex)
            {
                return new List<CategoryDTO>();
            }
        }

        public async Task<List<CategoryDTO>> GetByStatus(bool? IsDeleted)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            var categories = await _categoryRepository.GetByStatus(IsDeleted);
            var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
            return categoryDTOs;
        }

        public async Task<CategoryDTO> Update(UpdateCategoryDTO request)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            category.CategoryName = request.CategoryName;
            category.Image = request.Image;

            _categoryRepository.Update(category);

            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }
    }
}
