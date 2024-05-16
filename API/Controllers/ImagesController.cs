using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ImagesController : ApiControllerBase
    {
        private readonly IMediaService _mediaService;
        public ImagesController(IMediaService mediaService) {
            _mediaService = mediaService;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UpLoadAsync(IFormFile request, CancellationToken token) {
            var fileName =await _mediaService.UploadAsync(request, token);
            return Ok(fileName);
        }
    }
}
