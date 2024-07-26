using Application.DTOs;
using Application.IServices;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text.Json;

namespace API.Controllers
{

    public class UsersController : ApiControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISkillService _skillService;
        private readonly IMediaService _mediaFileService;
        private readonly IPasswordGeneratorService _passwordGeneratorService;
        private readonly RoleManager<Role> _roleManager;
        public UsersController(IAppUserService appUserService, ICurrentUserService currentUserService, UserManager<AppUser> userManager, ISkillService skillService, IPasswordGeneratorService passwordGeneratorService, IMediaService mediaFileService, RoleManager<Role> roleManager)
        {
            _appUserService = appUserService;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _skillService = skillService;
            _passwordGeneratorService = passwordGeneratorService;
            _mediaFileService = mediaFileService;
            _roleManager = roleManager;
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
        [HttpGet]
        [Route(Common.Url.User.GetUser)]
        public async Task<IActionResult> GetUser(int uid)
        {
            //var userId = _currentUserService.UserId;
            var userDtos = await _appUserService.GetUserDTOAsync(uid);
            return (Ok(userDtos));
        }
        [HttpPost]
        [Route(Common.Url.User.Update)]
        public async Task<IActionResult> Update([FromBody] UserUpdateDTO dto)
        {
            var userId = _currentUserService.UserId;
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return BadRequest("Bạn hãy đăng nhập");
            }
            user.PhoneNumber = String.IsNullOrEmpty(dto.PhoneNumber) ? "" : dto.PhoneNumber;
            user.Email = dto.Email;
            user.Name = dto.Name;
            user.Description = dto.Description;
            user.TaxCode = String.IsNullOrEmpty(dto.TaxCode) ? "": dto.TaxCode;
            var userResult = await _userManager.UpdateAsync(user);
            if (userResult.Succeeded)
            {
                await _skillService.UpdateSkillForUser(dto.Skills, userId);
            }
            return Ok(dto);  
        }

        [HttpPut]
        [Route(Common.Url.User.ChangePassword)]
        public async Task<IActionResult> ChangePassword(UserChangePasswordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var userId = _currentUserService.UserId;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Bạn hãy đăng nhập"
                });
            }

            if (!_passwordGeneratorService.VerifyHashPassword(user.PasswordHash, dto.OldPassword))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Mật khẩu cũ sai !"
                });
            }

            user.PasswordHash = _passwordGeneratorService.HashPassword(dto.NewPassword);
            var userResult = await _userManager.UpdateAsync(user);

            if (!userResult.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Lỗi khi cập nhật mật khẩu"
                });
            }

            return Ok(new
            {
                success = true
            });
        }


        [HttpPut]
        [Route(Common.Url.User.UpdateEducation)]
        public async Task<IActionResult> UpdateEducation(List<Education> educations)
        {
            var userId = _currentUserService.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            user.Education=  JsonSerializer.Serialize(educations, options);
            var userResult = await _userManager.UpdateAsync(user);
            return Ok(new
            {
                educations=  educations,
                success = true,
            });
        }
        [HttpPut]
        [Route(Common.Url.User.UpdateExperience)]
        public async Task<IActionResult> UpdateExperience(List<Experience> experiences)
        {
            var userId = _currentUserService.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            user.Experience = JsonSerializer.Serialize(experiences, options);
            var userResult = await _userManager.UpdateAsync(user);
            return Ok(new
            {
                experiences = experiences,
                success = true,
            });
        }
        [HttpPut]
        [Route(Common.Url.User.UpdateQualification)]
        public async Task<IActionResult> UpdateQualification(List<Qualification> qualifications)
        {
            var userId = _currentUserService.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            user.Qualifications = JsonSerializer.Serialize(qualifications, options);
            var userResult = await _userManager.UpdateAsync(user);
            return Ok(new
            {
                qualifications = qualifications,
                success = true,
            });
        }

        [HttpPost]
        [Route(Common.Url.User.AddPortfolio)]
        public async Task<IActionResult> AddPortfolio(MediaFileDTO mediaFile)
        {
            var userId = _currentUserService.UserId;
            var file =await _mediaFileService.AddMediaFile(mediaFile);
            return Ok(file);
        }

        [HttpPut]
        [Route(Common.Url.User.UpdatePortfolio)]
        public async Task<IActionResult> UpdatePortfolio(MediaFileDTO mediaFile)
        {
            var userId = _currentUserService.UserId;
            var file = await _mediaFileService.UpdateMediaFile(mediaFile);
            return Ok(file);
        }

        [HttpDelete]
        [Route(Common.Url.User.DeletePortfolio)]
        public async Task<IActionResult> DeletePortfolio(long id)
        {
            var userId = _currentUserService.UserId;
            var fileId = await _mediaFileService.DeleteMediaFile(id);
            return Ok(fileId);
        }

        [HttpPut]
        [Route(Common.Url.User.ConvertIntoRecruiter)]
        public async Task<IActionResult> ConvertIntoRecruiter(long id)
        {
            return Ok();
        }

        [HttpGet]
        [Route(Common.Url.User.GetUsers)]
        public async Task<IActionResult> GetUsers([FromQuery] UserSearchDTO userSearchDTO)
        {
            var users = await _appUserService.GetUsers(userSearchDTO);
            return Ok(users);
        }

        [HttpGet]
        [Route(Common.Url.User.Roles)]
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost]
        [Route(Common.Url.User.Lock)]
        public async Task<IActionResult> Lock([FromBody] List<int> userIds)
        {
            var users = await _userManager.Users.Where(x=>userIds.Contains(x.Id)).ToListAsync();
            if (!users.Any())
            {
                return NotFound("Không tìm thấy người dùng.");
            }
            var userLock = users.Where(x=>x.LockoutEnabled || x.LockoutEnd  == null).ToList();

            if (!userLock.Any())
            {
                return BadRequest("Tất cả người dùng đều đang bị khóa");
            }else
            {
                foreach (var user in userLock) {
                    var lockoutEndDate = DateTimeOffset.UtcNow.AddYears(100);
                    var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);
                    var lockDisabledTask = await _userManager.SetLockoutEnabledAsync(user, false);
                }
                return Ok("Khóa thành công "+userLock.Count +" người dùng");
            } 
        }

        [HttpPost]
        [Route(Common.Url.User.Unlock)]
        public async Task<IActionResult> Unlock([FromBody] List<int> userIds)
        {
            var users = await _userManager.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();
            if (!users.Any())
            {
                return NotFound("Không tìm thấy người dùng.");
            }
            var userUnlock = users.Where(x => !x.LockoutEnabled || x.LockoutEnd != null).ToList();

            if (!userUnlock.Any())
            {
                return BadRequest("Tất cả người dùng đều đang mở khóa");
            }
            else
            {
                foreach (var user in userUnlock)
                {
                    var lockDisabledTask = await _userManager.SetLockoutEnabledAsync(user, true);

                    var setLockoutEndDateTask = await _userManager.SetLockoutEndDateAsync(user, null);
                }
                return Ok("Mở khóa thành công "+ userUnlock.Count+" người dùng");
            }
        }
    }
}
