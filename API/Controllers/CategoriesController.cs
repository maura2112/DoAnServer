using Application.DTOs;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route(Common.Url.Category.GetAll)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _categoryService.GetAll());
        }
    }
}
