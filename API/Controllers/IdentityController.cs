using Application.DTOs;
using Application.DTOs.AuthenticationDTO;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using MailKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using static Domain.Exceptions.Constant;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
        private readonly IEmailSender _emailSender;
        public IdentityController(IJwtTokenService jwtTokenService, IMapper mapper, UserManager<AppUser> userManager, IPasswordGeneratorService passwordGeneratorService, ISkillService skillService, SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _passwordGeneratorService = passwordGeneratorService;
            _skillService = skillService;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
                return Conflict(new { message = "Email đã được sử dụng." });
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
                Role = roles.First(),
                Name =  user.Name,
                Avatar = user.Avatar,
                EmailConfirmed = user.EmailConfirmed,
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

        [HttpPost]
        [Route(Common.Url.User.Identity.ResetPassword)]
        public async Task<ActionResult> ResetPassword( string email)
        {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Email sai hoặc người dùng không tồn tại"
                });
            }
            var resetCode = _passwordGeneratorService.Generate6DigitCode();
            user.PasswordResetToken = _passwordGeneratorService.HashPassword(resetCode);
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(3);
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(email, "Mã xác nhận thiết lập lại mật khẩu ",
                        $"<p>Dear {user.Name},</p>" +
                        $"<p>Chúng tôi đã nhận được yêu cầu thiết lập lại mật khẩu. Hãy sữ dụng mã dưới đây:</p>" +
                        $"<h3 style=\"background-color: #f9f9f9; padding: 10px; border: 1px solid #ccc; display: inline-block;\">{resetCode}</h3>" +
                        $"<p>Mã sẽ hợp lệ trong vòng 3 phút.</p>" +
                        $"<p>Nếu bạn không yêu cầu thiết lập lại mật khẩu, hãy bỏ qua email này.</p>" +
                        $"<p>Cảm ơn,</p>" +
                        $"<p>GoodJobs</p>");
            return Ok(new
            {
                success = true,
                message = "Hãy kiểm tra email để lấy mã xác nhận"
            }); ;
        }

        [HttpPost]
        [Route(Common.Url.User.Identity.ResetPasswordInputCode)]
        public async Task<ActionResult> ResetPasswordInputCode(string email , string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.PasswordResetToken == null)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Email sai hoặc người dùng không tồn tại"
                });
            }
            if (!_passwordGeneratorService.VerifyHashPassword(user.PasswordResetToken, code) )
            {
                return Conflict(new
                {
                    success = false,
                    message = "Mã không hợp lệ !"
                });
            }
            if(user.ResetTokenExpires < DateTime.UtcNow)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Mã không hợp lệ !"
                });
            }
            return Ok(new
            {
                success = true,
                message = "Mã chính xác",
                secureToken  = user.PasswordResetToken
            }) ;
        }

        [HttpPost]
        [Route(Common.Url.User.Identity.ResetNewPassword)]
        public async Task<ActionResult> ResetNewPassword([FromBody] ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.PasswordResetToken == null)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Email sai hoặc người dùng không tồn tại"
                });
            }
            if (!user.PasswordResetToken.Equals(dto.SecureToken))
            {
                return Conflict(new
                {
                    success = false,
                    message = "Mã không hợp lệ !"
                });
            }
            if (user.ResetTokenExpires < DateTime.UtcNow)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Mã không hợp lệ !"
                });
            }
            user.PasswordHash = _passwordGeneratorService.HashPassword(dto.NewPassword);
             await _userManager.UpdateAsync(user);
            return Ok(new
            {
                success = true,
                message = "Reset mật khẩu thành công",
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
