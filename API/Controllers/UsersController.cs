using Application.DTOs;
using Application.IServices;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{

    public class UsersController : ApiControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly ICurrentUserService _currentUserService;
        public UsersController(IAppUserService appUserService, ICurrentUserService currentUserService)
        {
            _appUserService = appUserService;
            _currentUserService = currentUserService;
        }
        [HttpGet]
        [Route(Common.Url.User.Profile)]
        public async Task<IActionResult> Profile()
        {
            var userId = _currentUserService.UserId;
            //var userId = 9;
            var userDtos = await _appUserService.GetUserDTOAsync(userId);
            return (Ok(userDtos));
        }
    }
}
