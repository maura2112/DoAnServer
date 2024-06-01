using Application.DTOs;
using Application.DTOs.AuthenticationDTO;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Domain.Exceptions.Constant;

namespace API.Controllers
{
    public class IdentityController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordGeneratorService _passwordGeneratorService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ISkillService _skillService;
        public IdentityController(IJwtTokenService jwtTokenService, IMapper mapper, UserManager<AppUser> userManager, IPasswordGeneratorService passwordGeneratorService, ISkillService skillService, SignInManager<AppUser> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _passwordGeneratorService = passwordGeneratorService;
            _skillService = skillService;
            _signInManager = signInManager;
        }
        [HttpPost]
        [Route(Common.Url.User.Identity.Register)]
        public async Task<ActionResult> Register([FromBody] RegisterDTO user)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            if (existUser != null)
            {
                throw new Exception("This email đã được sử dụng");
            }
            var userRegister = new AppUser()
            {
                UserName = user.Email,
                Email = user.Email,
                TaxCode = user.TaxCode,
                IsCompany = user.IsCompany,
                PasswordHash = _passwordGeneratorService.HashPassword(user.Password),
                Name = user.Name,
            };
            var userResult = await _userManager.CreateAsync(userRegister);
            if (userResult.Succeeded)
            {
                var currentUser = await _userManager.FindByIdAsync(userRegister.Id.ToString());
                await _userManager.AddToRolesAsync(currentUser, user.Roles);
            }
            else
            {
                throw new Exception(string.Join(",", userResult.Errors.Select(x => x.Description)));
            }
            if (!user.IsCompany)
            {
                await _skillService.AddSkillForUser(user.Skill, userRegister.Id);
            }
            return Ok(new
            {
                success = true,
                message  = "Bạn vừa đăng kí thành công",
                data = userResult
            });
        }

        [HttpPost]
        [Route(Common.Url.User.Identity.Login)]
        public async Task<ActionResult> Login([FromBody] LoginDTO userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message= "Email hoặc mật khẩu sai!"
                });
            }
            if (!_passwordGeneratorService.VerifyHashPassword(user.PasswordHash, userDto.Password))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Email hoặc mật khẩu sai!"
                });
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
                {
                    new("Id", user.Id.ToString()),
                    new(ClaimTypes.Email, userDto.Email)
                };
            foreach (var role in roles)
            {
                claims.Add(new Claim("Roles", role));
            }
            var accessToken = _jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var loginRespone = new LoginRespone()
            {
                UserId = user.Id,
                Roles = roles.ToList(),
                Name =  user.Name,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return Ok(loginRespone);
        }
        [HttpPost]
        [Route(Common.Url.User.Identity.Logout)]
        public async Task<ActionResult> Logout()
        {

            return Ok(new
            {
                success= true,
                message ="Bạn đã đăng xuất thành công!"
            });
        }
        [HttpGet]
        [Route(Common.Url.User.Identity.External)]
        public async Task<ActionResult> GetExternalLogin()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            IList<AuthenticationScheme>  ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Ok(ExternalLogins.First().Name);
        }
        [HttpPost]
        [Route(Common.Url.User.Identity.External)]
        public IActionResult GetExternalLogin(string provider)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("Callback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
    }   
}
