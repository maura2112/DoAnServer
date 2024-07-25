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
        public async Task<IActionResult> GetAll([FromQuery]BlogSearch search)
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

        [HttpGet]
        [Route(Common.Url.Blog.Gets)]
        public async Task<IActionResult> Gets([FromQuery] BlogFilter filter)
        {
            try
            {
                var blogs = await _blogService.GetBlogList(filter);
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

        [HttpGet]
        [Route(Common.Url.Blog.Detail)]
        public async Task<IActionResult> Detail([FromQuery] int id)
        {
            try
            {
                var blogDTO = await _blogService.GetBlogDTOAsync(id);
                return Ok(blogDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }

        [HttpPut]
        [Route(Common.Url.Blog.Update)]
        public async Task<IActionResult> Update(BlogCreateDTO dto)
        {
            try
            {
                var id = _currentUserService.UserId;
                var blogs = await _blogService.UpdateBlog(dto);
                if(blogs != null)
                {
                    var blogDTO = await _blogService.GetBlogDTOAsync((int) dto.Id);
                    return Ok(blogDTO);
                }
                return Conflict("Cập nhật bài viết không thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }

        [HttpDelete]
        [Route(Common.Url.Blog.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var uid = _currentUserService.UserId;
                var result = await _blogService.DeleteBlog(id);
                if (result)
                {
                    return Ok("Đã xóa bài viết thành công");
                }
                return Conflict("Xóa bài viết không thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }

        [HttpPut]
        [Route(Common.Url.Blog.Publish)]
        public async Task<IActionResult> Publish(int id)
        {
            try
            {
                var uid = _currentUserService.UserId;
                var result = await _blogService.PublishBlog(id);
                if (result !=  0)
                {
                    return Ok("Bạn đã công khai bài viết thành công");
                }
                return Conflict("Bạn đã không công khai bài viết thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error" });
            }
        }
    }
}
