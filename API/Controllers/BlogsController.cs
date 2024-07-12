using Application.DTOs;
using Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class BlogsController : ApiControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly ICurrentUserService _currentUserService;
        public BlogsController(IBlogService blogService, ICurrentUserService currentUserService)
        {
            _blogService = blogService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Route(Common.Url.Blog.GetAll)]
        public async Task<IActionResult> GetAll(BlogSearch search)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var blogs = await _blogService.GetBlogs(search);
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }

        [HttpPost]
        [Route(Common.Url.Blog.Create)]
        public async Task<IActionResult> Create(BlogCreateDTO dto)
        {
            try
            {
                dto.CreatedBy = _currentUserService.UserId;
                var blogs = await _blogService.CreateBlog(dto);
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }
    }
}
