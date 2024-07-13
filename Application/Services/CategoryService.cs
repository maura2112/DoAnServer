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
using System.Drawing.Printing;
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

        public async Task<Pagination<CategoryDTO>> GetAllPagination(int pageIndex, int pageSize)
        {
            var categories = await _categoryRepository.GetByStatus(false, pageIndex, pageSize);
            var categoryDTOs = _mapper.Map<Pagination<CategoryDTO>>(categories);
            foreach (var cate in categoryDTOs.Items)
            {
                cate.TotalProjects = await _categoryRepository.GetTotalProjectByCategoryId(cate.Id);
            }
            return categoryDTOs;
        }

        public async Task<CategoryDTO> Add(UpdateCategoryDTO request)
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

        public async Task<CategoryDTO> RestoreDeleted(int id)
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
            category.IsDeleted = false;
            _categoryRepository.Update(category);
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task<List<CategoryDTO>> GetAllHomePage()
        {
            var categories = await _categoryRepository.GetAllNotDeleted();
            var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
            foreach (var cate in categoryDTOs)
            {
                cate.TotalProjects = await _categoryRepository.GetTotalProjectByCategoryId(cate.Id);
            }
            return categoryDTOs;

        }

        public async Task<Pagination<CategoryDTO>> GetByStatus(bool? IsDeleted, int pageIndex, int pageSize)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            var categories = await _categoryRepository.GetByStatus(IsDeleted, pageIndex, pageSize);
            var categoryDTOs = _mapper.Map<Pagination<CategoryDTO>>(categories);
            foreach (var cate in categoryDTOs.Items)
            {
                cate.TotalProjects = await _categoryRepository.GetTotalProjectByCategoryId(cate.Id);
            }
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
