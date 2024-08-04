using Application.DTOs;
using Application.DTOs.AuthenticationDTO;
using Application.Extensions;
using Application.IServices;
using Application.Services;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using MailKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Security.Claims;
using System.Text.Encodings.Web;
using static API.Common.Url;
using static Domain.Exceptions.Constant;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace API.Controllers
{
    public class IdentityController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAppUserService _appUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordGeneratorService _passwordGeneratorService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ISkillService _skillService;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ISmsService _smsService;
        private readonly ICurrentUserService _currentUserService;

        public IdentityController(IJwtTokenService jwtTokenService, IMapper mapper, UserManager<AppUser> userManager, IPasswordGeneratorService passwordGeneratorService, ISkillService skillService, SignInManager<AppUser> signInManager, IEmailSender emailSender, RoleManager<Role> roleManager, IConfiguration configuration, ISmsService smsService, ICurrentUserService currentUserService, IAppUserService appUserService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _passwordGeneratorService = passwordGeneratorService;
            _skillService = skillService;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _configuration = configuration;
            _smsService = smsService;
            _currentUserService = currentUserService;
            _appUserService = appUserService;
        }
        [HttpPost]
        [Route(Common.Url.User.Identity.Register)]
        public async Task<ActionResult> Register([FromBody] RegisterDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                AmountBid = 5,
                AmoutProject = 5,
                CreatedDate = DateTime.UtcNow,
            };
            var userResult = await _userManager.CreateAsync(userRegister);
            if (userResult.Succeeded)
            {
                var currentUser = await _userManager.FindByIdAsync(userRegister.Id.ToString());
                var roles = await _roleManager.Roles.ToListAsync();
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

            if (user.LockoutEnd > DateTime.Now)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau."
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
                    new("Name", user.Name),
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
                BidAmount = user.AmountBid,
                ProjectAmount = user.AmoutProject,
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
        public async Task<ActionResult> ResetPasswordInputCode([FromBody]  ResetPasswordCodeDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.PasswordResetToken == null)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Email sai hoặc người dùng không tồn tại"
                });
            }
            if (!_passwordGeneratorService.VerifyHashPassword(user.PasswordResetToken, dto.Code) )
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

           var  ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();

            return Ok(ExternalLogins);
        }
        [HttpPost]
        [Route(Common.Url.User.Identity.External)]
        public async Task<IActionResult> GetExternalLoginAsync([FromBody] string accessToken)
        {
            var credential = GoogleCredential.FromAccessToken(accessToken);
            var oauthService = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
            var userInfo = await oauthService.Userinfo.Get().ExecuteAsync();
            var UserDb = await _userManager.FindByEmailAsync(userInfo.Email);
            if (UserDb != null)
            {
                if(UserDb.EmailConfirmed == false)
                {
                    UserDb.EmailConfirmed = true;
                    var userResult = await _userManager.UpdateAsync(UserDb);
                }
            }
            else
            {
                UserDb = new AppUser()
                {
                    UserName = userInfo.Email,
                    Email = userInfo.Email,
                    TaxCode = "",
                    IsCompany = false,
                    PasswordHash = _passwordGeneratorService.HashPassword(userInfo.Id),
                    Name = userInfo.Name,
                    Avatar = "https://i.pinimg.com/736x/bc/43/98/bc439871417621836a0eeea768d60944.jpg",
                    CreatedDate = DateTime.UtcNow,
                };
                var userResult = await _userManager.CreateAsync(UserDb);
                if (userResult.Succeeded)
                {
                    var currentUser = await _userManager.FindByIdAsync(UserDb.Id.ToString());
                    var roles = await _roleManager.Roles.ToListAsync();
                    await _userManager.AddToRolesAsync(currentUser, ["Freelancer"]);
                }
            }

            // Login
            var rolesUser = await _userManager.GetRolesAsync(UserDb);
            var claims = new List<Claim>
                {
                    new("Id", UserDb.Id.ToString()),
                    new(ClaimTypes.Email, UserDb.Email)
                };
            foreach (var role in rolesUser)
            {
                claims.Add(new Claim("Roles", role));
            }

            var accessTokenLogin = _jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var loginRespone = new LoginRespone()
            {
                UserId = UserDb.Id,
                Role = rolesUser.First(),
                Name = UserDb.Name,
                Avatar = UserDb.Avatar,
                EmailConfirmed = UserDb.EmailConfirmed,
                AccessToken = accessTokenLogin,
                RefreshToken = refreshToken
            };
            return Ok(loginRespone);
        }


        [HttpPost]
        [Route(Common.Url.User.Identity.VerifyPhone)]
        public async Task<IActionResult> SendSmsAsync([FromBody] string phoneNumber )
        {
            try
            {
                var userId = _currentUserService.UserId;
                var userPhone = await _appUserService.FindByPhoneConfirmed(StringExtensions.NormalizePhoneNumber(phoneNumber));
                if(userPhone != null)
                {
                    return BadRequest("Số điện thoại này đã được xác nhận");
                }
                var user = await _userManager.FindByIdAsync(userId.ToString());
                var Code = _passwordGeneratorService.Generate6DigitCode();
                user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(1);
                user.PhoneNumber = StringExtensions.NormalizePhoneNumber(phoneNumber);
                user.PasswordResetToken = _passwordGeneratorService.HashPassword(Code);
                await _userManager.UpdateAsync(user);
                bool result = await _smsService.SendSmsAsync(phoneNumber, "GoodJobs - Mã xác thực số điện thoại: "+ Code);
                if (result)
                {
                    return Ok("Message sent successfully");
                }
                else
                {
                    return BadRequest("Failed to send SMS.");
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "An error occurred while sending SMS.");
            }
        }

        [HttpPost]
        [Route(Common.Url.User.Identity.VerifyPhoneCode)]
        public async Task<IActionResult> VerifyPhoneCode([FromBody] string code)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null || user.PasswordResetToken == null)
                {
                    return Conflict(new
                    {
                        success = false,
                        message = "Người dùng không tồn tại"
                    });
                }
                if (!_passwordGeneratorService.VerifyHashPassword(user.PasswordResetToken, code.ToString()))
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
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Ok(new
                {
                    success = true,
                    message = "Đã xác thực người dùng thành công",
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }   
}
