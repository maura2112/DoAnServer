using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using Application.Services;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryService categoryService, ICategoryRepository categoryRepository)
        {
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        [Route(Common.Url.Category.GetAll)]
        [RoleAuthorizeAttribute("Freelancer")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryService.GetAllHomePage();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }

        [HttpGet]
        [Route(Common.Url.Category.GetAllPaginaton)]
        public async Task<IActionResult> Pagination(int pageIndex, int pageSize)
        {
            try
            {
                var categories = await _categoryService.GetAllPagination(pageIndex,  pageSize);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }

        [HttpPost]
        [Route(Common.Url.Category.Add)]
        public async Task<IActionResult> AddAsync(UpdateCategoryDTO DTOs)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var category = await _categoryService.Add(DTOs);

            return Ok(new
            {
                success = true,
                message = "Bạn vừa tạo danh mục mới thành công",
                data = category
            });
        }
        [HttpPut]
        [Route(Common.Url.Category.Update)]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryDTO DTOs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fetchedProject = await _categoryRepository.GetByIdAsync(DTOs.Id);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục phù hợp!" });
            }

            var category = await _categoryService.Update(DTOs);


            return Ok(new
            {
                success = true,
                message = "Bạn vừa cập nhật danh mục thành công",
                data = category
            });
        }
        [HttpDelete]
        [Route(Common.Url.Category.Delete)]
        public async Task<IActionResult> DeleteAsync(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fetchedProject = await _categoryRepository.GetByIdAsync(categoryId);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục phù hợp!" });
            }

            await _categoryService.Delete(categoryId);

            return Ok(new
            {
                success = true,
                message = "Bạn vừa xóa danh mục thành công"
            });
        }
        [HttpGet]
        [Route(Common.Url.Category.GetByStatus)]
        public async Task<IActionResult> GetByStatus([FromQuery] bool? isDeleted, int pageIndex, int pageSize)
        {
            var result = await _categoryService.GetByStatus(isDeleted, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpPut]
        [Route(Common.Url.Category.RestoreDeleted)]
        public async Task<IActionResult> RestoreDeleted(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fetchedProject = await _categoryRepository.GetByIdAsync(id);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục phù hợp!" });
            }

            await _categoryService.RestoreDeleted(id);

            return Ok(new
            {
                success = true,
                message = "Bạn vừa khôi phục  danh mục thành công"
            });
        }


    }
}
