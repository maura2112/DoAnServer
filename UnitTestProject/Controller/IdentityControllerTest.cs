using API.Controllers;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API.Common.Url;
using Skill = Domain.Entities.Skill;
using Application.DTOs.AuthenticationDTO;
using System.Security.Claims;
using Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;

namespace UnitTestProject.Controller
{
    [TestFixture]
    public class IdentityControllerTest 
    {
        private Mock<IMapper> _mapperMock;
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<IPasswordGeneratorService> _passwordGeneratorServiceMock;
        private Mock<IJwtTokenService> _jwtTokenServiceMock;
        private Mock<SignInManager<AppUser>> _signInManagerMock;
        private Mock<ISkillService> _skillServiceMock;
        private Mock<IEmailSender> _emailSenderMock;
        private Mock<RoleManager<Role>> _roleManagerMock;
        private Mock<IConfiguration> _configurationMock;
        private IdentityController _controller;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();

            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(),
                null, null, null, null, null, null, null, null);

            _passwordGeneratorServiceMock = new Mock<IPasswordGeneratorService>();

            _jwtTokenServiceMock = new Mock<IJwtTokenService>();

            _signInManagerMock = new Mock<SignInManager<AppUser>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null, null, null, null);

            _skillServiceMock = new Mock<ISkillService>();

            _emailSenderMock = new Mock<IEmailSender>();

            _roleManagerMock = new Mock<RoleManager<Role>>(
                Mock.Of<IRoleStore<Role>>(),
                null, null, null, null);

            _configurationMock = new Mock<IConfiguration>();

            _controller = new IdentityController(
                _jwtTokenServiceMock.Object,
                _mapperMock.Object,
                _userManagerMock.Object,
                _passwordGeneratorServiceMock.Object,
                _skillServiceMock.Object,
                _signInManagerMock.Object,
                _emailSenderMock.Object,
                _roleManagerMock.Object,
                _configurationMock.Object);
        }

        #region Register
        [Test]
        public async Task Register_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model state");
            var registerDto = new RegisterDTO();

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Register_EmailAlreadyInUse_ReturnsConflict()
        {
            // Arrange
            var registerDto = new RegisterDTO { Email = "test@example.com" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(registerDto.Email)).ReturnsAsync(new AppUser());

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.AreEqual("Email đã được sử dụng.", conflictResult.Value?.GetType().GetProperty("message")?.GetValue(conflictResult.Value)?.ToString());
        }

        [Test]
        public async Task Register_UserCreationFails_ThrowsException()
        {
            // Arrange
            var registerDto = new RegisterDTO { Email = "test@example.com", Password = "password123", Roles = new List<string> { "User" } };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Test error" });

            _userManagerMock.Setup(u => u.FindByEmailAsync(registerDto.Email)).ReturnsAsync((AppUser)null);
            _passwordGeneratorServiceMock.Setup(p => p.HashPassword(registerDto.Password)).Returns("hashedPassword");
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<AppUser>())).ReturnsAsync(identityResult);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _controller.Register(registerDto));
            Assert.AreEqual("Test error", ex.Message);
        }

        [Test]
        public async Task Register_CreateUserSuccess_AddsRolesAndSkills_ReturnsOk()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Email = "test@example.com",
                Password = "Password123",
                Roles = new List<string> { "User" },
                Skill = new List<string> { "Skill1" },
                IsCompany = false
            };

            var userId = 1; // Giả sử ID của người dùng sau khi tạo là 1
            var appUser = new AppUser { Id = userId, Email = registerDto.Email };

            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(appUser);
            _userManagerMock.Setup(u => u.AddToRolesAsync(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
            _skillServiceMock.Setup(s => s.AddSkillForUser(It.IsAny<List<string>>(), It.IsAny<int>())).ReturnsAsync(new List<Skill>());
            _passwordGeneratorServiceMock.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("hashedPassword");

            // Act
            var actionResult = await _controller.Register(registerDto);

            // Assert
            Assert.IsAssignableFrom<ActionResult>(actionResult); // Kiểm tra actionResult có thể gán được cho ActionResult

            if (actionResult is OkObjectResult okResult)
            {
                Assert.IsTrue((bool)okResult.Value.GetType().GetProperty("success")?.GetValue(okResult.Value));
                Assert.AreEqual("Bạn vừa đăng kí thành công", okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value)?.ToString());

                var data = okResult.Value.GetType().GetProperty("data")?.GetValue(okResult.Value);
                Assert.IsNotNull(data);
                Assert.AreEqual(userId, int.Parse(data.GetType().GetProperty("UserId")?.GetValue(data)?.ToString()));

                // Verify các hoạt động với UserManager và SkillService
                _userManagerMock.Verify(u => u.CreateAsync(It.IsAny<AppUser>()), Times.Once);
                _userManagerMock.Verify(u => u.FindByEmailAsync(registerDto.Email), Times.Once);
                _userManagerMock.Verify(u => u.FindByIdAsync(userId.ToString()), Times.Once);
                _userManagerMock.Verify(u => u.AddToRolesAsync(appUser, It.Is<IEnumerable<string>>(r => r.Contains("User"))), Times.Once);
                _skillServiceMock.Verify(s => s.AddSkillForUser(registerDto.Skill, userId), Times.Once);
            }
            else
            {
                Assert.Fail("Expected OkObjectResult but received a different ActionResult.");
            }
        }



        #endregion

        #region Login
        [Test]
        public async Task Login_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "Password123" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync((AppUser)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Email hoặc mật khẩu sai!", badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value)?.ToString());
        }

        [Test]
        public async Task Login_IncorrectPassword_ReturnsBadRequest()
        {
            // Arrange
            var user = new AppUser { Email = "test@example.com", PasswordHash = "hashedPassword" };
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "WrongPassword" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _passwordGeneratorServiceMock.Setup(p => p.VerifyHashPassword(user.PasswordHash, loginDto.Password)).Returns(false);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Email hoặc mật khẩu sai!", badRequestResult.Value.GetType().GetProperty("message")?.GetValue(badRequestResult.Value)?.ToString());
        }

        [Test]
        public async Task Login_Success_ReturnsOkObjectResult()
        {
            // Arrange
            var user = new AppUser
            {
                Id = 1,
                Email = "test@example.com",
                Name = "Test User",
                Avatar = "avatar-url",
                EmailConfirmed = true
            };
            var roles = new List<string> { "User" };
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "Password123" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _passwordGeneratorServiceMock.Setup(p => p.VerifyHashPassword(user.PasswordHash, loginDto.Password)).Returns(true);
            _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(roles);
            _jwtTokenServiceMock.Setup(j => j.GenerateAccessToken(It.IsAny<List<Claim>>())).Returns("accessToken");
            _jwtTokenServiceMock.Setup(j => j.GenerateRefreshToken()).Returns("refreshToken");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var loginResponse = okResult.Value as LoginRespone;
            Assert.IsNotNull(loginResponse);
            Assert.AreEqual(user.Id, loginResponse.UserId);
            Assert.AreEqual(user.Name, loginResponse.Name);
            Assert.AreEqual(user.Avatar, loginResponse.Avatar);
            Assert.AreEqual(user.EmailConfirmed, loginResponse.EmailConfirmed);
            Assert.AreEqual("accessToken", loginResponse.AccessToken);
            Assert.AreEqual("refreshToken", loginResponse.RefreshToken);
        }


        #endregion

        #region Logout

        [Test]
        public async Task Logout_ReturnsOkObjectResult()
        {
            // Act
            var result = await _controller.Logout();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var success = (bool)okResult.Value.GetType().GetProperty("success")?.GetValue(okResult.Value);
            var message = okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value)?.ToString();

            Assert.IsTrue(success);
            Assert.AreEqual("Bạn đã đăng xuất thành công!", message);
        }

        #endregion

        #region ResetPassword
        [Test]
        public async Task ResetPassword_ValidEmail_ReturnsOk()
        {
            // Arrange
            string email = "test@example.com";
            var user = new AppUser { Email = email, Name = "Test User" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(email)).ReturnsAsync(user);
            _passwordGeneratorServiceMock.Setup(p => p.Generate6DigitCode()).Returns("123456");
            _passwordGeneratorServiceMock.Setup(p => p.HashPassword("123456")).Returns("hashedResetCode");
            _userManagerMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            _emailSenderMock.Setup(e => e.SendEmailAsync(
                email,
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ResetPassword(email);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue((bool)okResult.Value.GetType().GetProperty("success")?.GetValue(okResult.Value));
            Assert.AreEqual("Hãy kiểm tra email để lấy mã xác nhận", okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(email), Times.Once);
            _passwordGeneratorServiceMock.Verify(p => p.Generate6DigitCode(), Times.Once);
            _passwordGeneratorServiceMock.Verify(p => p.HashPassword("123456"), Times.Once);
            _userManagerMock.Verify(u => u.UpdateAsync(user), Times.Once);
            _emailSenderMock.Verify(e => e.SendEmailAsync(
                email,
                "Mã xác nhận thiết lập lại mật khẩu ",
                It.IsAny<string>()
            ), Times.Once);
        }

        [Test]
        public async Task ResetPassword_InvalidEmail_ReturnsConflict()
        {
            // Arrange
            string email = "nonexistent@example.com";

            _userManagerMock.Setup(u => u.FindByEmailAsync(email)).ReturnsAsync((AppUser)null);

            // Act
            var result = await _controller.ResetPassword(email);

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.IsFalse((bool)conflictResult.Value.GetType().GetProperty("success")?.GetValue(conflictResult.Value));
            Assert.AreEqual("Email sai hoặc người dùng không tồn tại", conflictResult.Value.GetType().GetProperty("message")?.GetValue(conflictResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(email), Times.Once);
            _passwordGeneratorServiceMock.Verify(p => p.Generate6DigitCode(), Times.Never);
            _passwordGeneratorServiceMock.Verify(p => p.HashPassword(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(u => u.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
            _emailSenderMock.Verify(e => e.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never);
        }


        #endregion

        #region ResetPasswordInputCode


        [Test]
        public async Task ResetPasswordInputCode_UserNotFound_ReturnsConflict()
        {
            // Arrange
            var dto = new ResetPasswordCodeDTO { Email = "nonexistent@example.com", Code = "123456" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((AppUser)null);

            // Act
            var result = await _controller.ResetPasswordInputCode(dto);

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.IsFalse((bool)conflictResult.Value.GetType().GetProperty("success")?.GetValue(conflictResult.Value));
            Assert.AreEqual("Email sai hoặc người dùng không tồn tại", conflictResult.Value.GetType().GetProperty("message")?.GetValue(conflictResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(dto.Email), Times.Once);
            _passwordGeneratorServiceMock.Verify(p => p.VerifyHashPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task ResetPasswordInputCode_InvalidCode_ReturnsConflict()
        {
            // Arrange
            var dto = new ResetPasswordCodeDTO { Email = "test@example.com", Code = "invalidcode" };
            var user = new AppUser { Email = dto.Email, PasswordResetToken = "hashedResetCode" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _passwordGeneratorServiceMock.Setup(p => p.VerifyHashPassword(user.PasswordResetToken, dto.Code)).Returns(false);

            // Act
            var result = await _controller.ResetPasswordInputCode(dto);

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.IsFalse((bool)conflictResult.Value.GetType().GetProperty("success")?.GetValue(conflictResult.Value));
            Assert.AreEqual("Mã không hợp lệ !", conflictResult.Value.GetType().GetProperty("message")?.GetValue(conflictResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(dto.Email), Times.Once);
            _passwordGeneratorServiceMock.Verify(p => p.VerifyHashPassword(user.PasswordResetToken, dto.Code), Times.Once);
        }

        [Test]
        public async Task ResetPasswordInputCode_ExpiredToken_ReturnsConflict()
        {
            // Arrange
            var dto = new ResetPasswordCodeDTO { Email = "test@example.com", Code = "123456" };
            var user = new AppUser { Email = dto.Email, PasswordResetToken = "hashedResetCode", ResetTokenExpires = DateTime.UtcNow.AddMinutes(-1) };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _passwordGeneratorServiceMock.Setup(p => p.VerifyHashPassword(user.PasswordResetToken, dto.Code)).Returns(true);

            // Act
            var result = await _controller.ResetPasswordInputCode(dto);

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.IsFalse((bool)conflictResult.Value.GetType().GetProperty("success")?.GetValue(conflictResult.Value));
            Assert.AreEqual("Mã không hợp lệ !", conflictResult.Value.GetType().GetProperty("message")?.GetValue(conflictResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(dto.Email), Times.Once);
            _passwordGeneratorServiceMock.Verify(p => p.VerifyHashPassword(user.PasswordResetToken, dto.Code), Times.Once);
        }

        [Test]
        public async Task ResetPasswordInputCode_ValidCode_ReturnsOk()
        {
            // Arrange
            var dto = new ResetPasswordCodeDTO { Email = "test@example.com", Code = "123456" };
            var user = new AppUser { Email = dto.Email, PasswordResetToken = "hashedResetCode", ResetTokenExpires = DateTime.UtcNow.AddMinutes(10) };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _passwordGeneratorServiceMock.Setup(p => p.VerifyHashPassword(user.PasswordResetToken, dto.Code)).Returns(true);

            // Act
            var result = await _controller.ResetPasswordInputCode(dto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue((bool)okResult.Value.GetType().GetProperty("success")?.GetValue(okResult.Value));
            Assert.AreEqual("Mã chính xác", okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value)?.ToString());
            Assert.AreEqual(user.PasswordResetToken, okResult.Value.GetType().GetProperty("secureToken")?.GetValue(okResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(dto.Email), Times.Once);
            _passwordGeneratorServiceMock.Verify(p => p.VerifyHashPassword(user.PasswordResetToken, dto.Code), Times.Once);
        }
        #endregion

        #region ResetNewPassword

        [Test]
        public async Task ResetNewPassword_InvalidDto_ReturnsBadRequest()
        {
            // Arrange
            var dto = new ResetPasswordDTO { Email = "test@example.com", SecureToken = "123456", NewPassword = "newpassword" };
            _controller.ModelState.AddModelError("Password", "Password is required");

            // Act
            var result = await _controller.ResetNewPassword(dto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result); // Kiểm tra result là kiểu ObjectResult
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.AreSame(_controller.ModelState, objectResult.Value);

            _userManagerMock.Verify(u => u.FindByEmailAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(u => u.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        }


        [Test]
        public async Task ResetNewPassword_UserNotFound_ReturnsConflict()
        {
            // Arrange
            var dto = new ResetPasswordDTO { Email = "nonexistent@example.com", SecureToken = "123456", NewPassword = "newpassword" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((AppUser)null);

            // Act
            var result = await _controller.ResetNewPassword(dto);

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.IsFalse((bool)conflictResult.Value.GetType().GetProperty("success")?.GetValue(conflictResult.Value));
            Assert.AreEqual("Email sai hoặc người dùng không tồn tại", conflictResult.Value.GetType().GetProperty("message")?.GetValue(conflictResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(dto.Email), Times.Once);
            _userManagerMock.Verify(u => u.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        }

        [Test]
        public async Task ResetNewPassword_InvalidSecureToken_ReturnsConflict()
        {
            // Arrange
            var dto = new ResetPasswordDTO { Email = "test@example.com", SecureToken = "invalidtoken", NewPassword = "newpassword" };
            var user = new AppUser { Email = dto.Email, PasswordResetToken = "hashedResetCode" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);

            // Act
            var result = await _controller.ResetNewPassword(dto);

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.IsFalse((bool)conflictResult.Value.GetType().GetProperty("success")?.GetValue(conflictResult.Value));
            Assert.AreEqual("Mã không hợp lệ !", conflictResult.Value.GetType().GetProperty("message")?.GetValue(conflictResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(dto.Email), Times.Once);
            _userManagerMock.Verify(u => u.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        }

        [Test]
        public async Task ResetNewPassword_ExpiredToken_ReturnsConflict()
        {
            // Arrange
            var dto = new ResetPasswordDTO { Email = "test@example.com", SecureToken = "123456", NewPassword = "newpassword" };
            var user = new AppUser { Email = dto.Email, PasswordResetToken = "hashedResetCode", ResetTokenExpires = DateTime.UtcNow.AddMinutes(-1) };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);

            // Act
            var result = await _controller.ResetNewPassword(dto);

            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.IsFalse((bool)conflictResult.Value.GetType().GetProperty("success")?.GetValue(conflictResult.Value));
            Assert.AreEqual("Mã không hợp lệ !", conflictResult.Value.GetType().GetProperty("message")?.GetValue(conflictResult.Value)?.ToString());

            _userManagerMock.Verify(u => u.FindByEmailAsync(dto.Email), Times.Once);
            _userManagerMock.Verify(u => u.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        }

        [Test]
        public async Task ResetNewPassword_ValidReset_ReturnsOk()
        {
            // Arrange
            var dto = new ResetPasswordDTO { Email = "test@example.com", SecureToken = "123456", NewPassword = "newpassword" };

            // Mock user có trong cơ sở dữ liệu
            var user = new AppUser { Id = 1, Email = dto.Email, PasswordResetToken = dto.SecureToken, ResetTokenExpires = DateTime.UtcNow.AddMinutes(5) };
            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.UpdateAsync(user)).Returns(Task.FromResult(IdentityResult.Success));

            // Act
            var result = await _controller.ResetNewPassword(dto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result); // Kiểm tra result là kiểu OkObjectResult
            var okResult = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode); // Kiểm tra StatusCode của OkObjectResult
            Assert.IsTrue((bool)okResult.Value.GetType().GetProperty("success")?.GetValue(okResult.Value)); // Kiểm tra giá trị success
            Assert.AreEqual("Reset mật khẩu thành công", okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value)?.ToString()); // Kiểm tra message trả về

            _userManagerMock.Verify(u => u.UpdateAsync(user), Times.Once); // Đảm bảo phương thức UpdateAsync được gọi đúng 1 lần
        }





        #endregion

        #region GetExternalLogin

        [Test]
        public async Task GetExternalLogin_ReturnsOk()
        {
            // Act
            var result = await _controller.GetExternalLogin();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result); // Kiểm tra kết quả trả về là OkObjectResult
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            // Kiểm tra các điều kiện khác tùy theo logic của phương thức GetExternalLogin()
        }





        #endregion

        #region GetExternalLoginAsync

        



        #endregion
    }
}
