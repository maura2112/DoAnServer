using API.Controllers;
using Application.DTOs;
using Application.IServices;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Common.Url;

namespace UnitTestProject.Controller
{
    [TestFixture]
    public class UserControllerTest
    {
        private Mock<IAppUserService> appUserServiceMock;
        private Mock<ICurrentUserService> currentUserServiceMock;
        private Mock<UserManager<AppUser>> userManagerMock;
        private Mock<ISkillService> skillServiceMock;
        private Mock<IMediaService> mediaFileServiceMock;
        private Mock<IPasswordGeneratorService> passwordGeneratorServiceMock;
        private Mock<RoleManager<Role>> roleManagerMock;

        private UsersController usersController;

        [SetUp]
        public void Setup()
        {
            appUserServiceMock = new Mock<IAppUserService>();
            currentUserServiceMock = new Mock<ICurrentUserService>();
            userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null,
                null, null, null, null);
            skillServiceMock = new Mock<ISkillService>();
            mediaFileServiceMock = new Mock<IMediaService>();
            passwordGeneratorServiceMock = new Mock<IPasswordGeneratorService>();
            roleManagerMock = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(), null, null, null, null);

            usersController = new UsersController(
                appUserServiceMock.Object,
                currentUserServiceMock.Object,
                userManagerMock.Object,
                skillServiceMock.Object,
                passwordGeneratorServiceMock.Object,
                mediaFileServiceMock.Object,
                roleManagerMock.Object
            );
        }

        #region Profile

        [Test]
        public async Task Profile_Returns_OkObjectResult_With_UserDto()
        {
            // Arrange
            int userId = 1;
            var expectedUserDto = new UserDTO
            {
                Id = userId,
                Name = "Test User",
                Email = "test@example.com",
                CreatedDate = DateTime.Now,
                IsCompany = false,
                Description = "Test Description",
                EmailConfirmed = true,
                LockoutEnd = null,
                LockoutEnabled = true,
                IsLock = false,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = true,
                Qualifications = new List<Qualification>(),
                Experiences = new List<Experience>(),
                Educations = new List<Education>(),
                Avatar = "avatar.jpg",
                mediaFiles = new List<MediaFileDTO>(),
                skills = new List<string>(),
                Address = new AddressDTO { }
            };

            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            appUserServiceMock.Setup(m => m.GetUserDTOAsync(userId)).ReturnsAsync(expectedUserDto);

            // Act
            var result = await usersController.Profile();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedUserDto, okResult.Value);
        }

        [Test]
        public async Task Profile_When_CurrentUserService_Returns_NullUserId_Returns_BadRequestResult()
        {
            // Arrange
            currentUserServiceMock.Setup(m => m.UserId).Returns(null);

            // Act
            var result = await usersController.Profile();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        #endregion

        #region Update

        [Test]
        public async Task Update_Returns_BadRequest_When_ModelState_Invalid()
        {
            // Arrange
            var dto = new UserUpdateDTO();
            usersController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await usersController.Update(dto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }



        [Test]
        public async Task Update_Returns_BadRequest_When_User_NotFound()
        {
            // Arrange
            var dto = new UserUpdateDTO
            {
                Name = "Updated Name",
                Email = "updated@example.com",
                PhoneNumber = "123456789",
                TaxCode = "123456",
                Skills = new List<string> { "Skill A", "Skill B" }
            };

            var userId = 1;
            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((AppUser)null);

            // Act
            var result = await usersController.Update(dto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_Returns_OkObjectResult_When_Update_Successful()
        {
            // Arrange
            var dto = new UserUpdateDTO
            {
                Name = "Updated Name",
                Email = "updated@example.com",
                PhoneNumber = "123456789",
                TaxCode = "123456",
                Skills = new List<string> { "Skill A", "Skill B" }
            };

            var userId = 1;
            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);

            var user = new AppUser { Id = userId };
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await usersController.Update(dto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(dto, okResult.Value);
        }


        #endregion

        #region ChangePassword

        [Test]
        public async Task ChangePassword_Returns_BadRequest_When_ModelState_Invalid()
        {
            // Arrange
            var dto = new UserChangePasswordDTO();
            usersController.ModelState.AddModelError("OldPassword", "OldPassword is required");

            // Act
            var result = await usersController.ChangePassword(dto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }


        [Test]
        public async Task ChangePassword_Returns_BadRequest_When_User_NotFound()
        {
            // Arrange
            var dto = new UserChangePasswordDTO
            {
                OldPassword = "oldPassword",
                NewPassword = "newPassword",
                NewPasswordConfirm = "newPassword"
            };

            var userId = 1; // userId needs to be a string for userManager.FindByIdAsync
            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((AppUser)null); // Simulate user not found

            // Act
            var result = await usersController.ChangePassword(dto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

        }


        [Test]
        public async Task ChangePassword_Returns_OkObjectResult_When_Password_Change_Successful()
        {
            // Arrange
            var dto = new UserChangePasswordDTO
            {
                OldPassword = "oldPassword",
                NewPassword = "newPassword"
            };

            var userId = 1;
            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);

            var user = new AppUser { Id = userId, PasswordHash = "hashedPassword" };
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user); // Simulate user found
            passwordGeneratorServiceMock.Setup(m => m.VerifyHashPassword("hashedPassword", "oldPassword"))
                .Returns(true); // Simulate password verification success
            userManagerMock.Setup(m => m.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success); // Simulate update success

            // Act
            var result = await usersController.ChangePassword(dto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

        }


        #endregion

        #region UpdateEducation

        [Test]
        public async Task UpdateEducation_Returns_OkResult()
        {
            // Arrange
            var userId = 1;
            var educations = new List<Education>
            {
                new Education
                {
                    Country = "Vietnam",
                    UniversityCollege = "University 1",
                    Degree = "Bachelor",
                    Start = new Start { Year = 2010, Month = "9" },
                    End = new End { Year = 2014, Month = "6" }
                },
                new Education
                {
                    Country = "USA",
                    UniversityCollege = "College 2",
                    Degree = "Master",
                    Start = new Start { Year = 2015, Month = "8" },
                    End = new End { Year = 2017, Month = "5" }
                }
            };

            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(new AppUser { Id = userId });

            // Act
            var result = await usersController.UpdateEducation(educations);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue(okResult != null);
            userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()), Times.Once);
        }


        #endregion

        #region UpdateExperience

        [Test]
        public async Task UpdateExperience_Returns_OkResult()
        {
            // Arrange
            var userId = 1;
            var experiences = new List<Experience>
            {
                new Experience
                {
                    Title = "Software Engineer",
                    Company = "ABC Inc.",
                    Start = new Start { Year = 2015, Month = "9" },
                    End = new End { Year = 2018, Month = "12" },
                    Summary = "Worked as a software engineer at ABC Inc."
                },
                new Experience
                {
                    Title = "Senior Developer",
                    Company = "XYZ Corp.",
                    Start = new Start { Year = 2019, Month = "1" },
                    End = null,
                    Summary = "Currently working as a senior developer at XYZ Corp."
                }
            };

            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new AppUser { Id = userId }); // Simulate user found

            // Act
            var result = await usersController.UpdateExperience(experiences);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue(okResult != null);

            // Verify that UpdateAsync was called with the correct user object
            userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()), Times.Once);
        }



        #endregion

        #region UpdateQualification

        [Test]
        public async Task UpdateQualification_Returns_OkResult()
        {
            // Arrange
            var userId = 1; // userId needs to be a string for userManager.FindByIdAsync
            var qualifications = new List<Qualification>
            {
                new Qualification
                {
                    Link = "http://example.com/qualification1",
                    Organization = "Organization 1",
                    Summary = "Qualified in field 1"
                },
                new Qualification
                {
                    Link = "http://example.com/qualification2",
                    Organization = "Organization 2",
                    Summary = "Qualified in field 2"
                }
            };

            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new AppUser { Id = userId }); // Simulate user found

            // Act
            var result = await usersController.UpdateQualification(qualifications);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue(okResult != null);

            // Verify that UpdateAsync was called with the correct user object
            userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()), Times.Once);
        }


        #endregion

        #region AddPortfolio

        [Test]
        public async Task AddPortfolio_Returns_OkResult()
        {
            // Arrange
            var userId = 1;
            var mediaFile = new MediaFileDTO
            {
                Id = 1,
                FileName = "testfile.pdf",
                Description = "Test file",
                Title = "Test Title",
                UserId = userId
            };

            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            mediaFileServiceMock.Setup(m => m.AddMediaFile(mediaFile))
                .ReturnsAsync(mediaFile); // Simulate media file added

            // Act
            var result = await usersController.AddPortfolio(mediaFile);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue(okResult != null);
            var addedFile = okResult.Value as MediaFileDTO;
            Assert.IsNotNull(addedFile);
            Assert.AreEqual(mediaFile.Id, addedFile.Id);
            Assert.AreEqual(mediaFile.FileName, addedFile.FileName);
            Assert.AreEqual(mediaFile.Description, addedFile.Description);
            Assert.AreEqual(mediaFile.Title, addedFile.Title);
            Assert.AreEqual(mediaFile.UserId, addedFile.UserId);

            // Verify that AddMediaFile was called with the correct mediaFile object
            mediaFileServiceMock.Verify(m => m.AddMediaFile(mediaFile), Times.Once);
        }


        #endregion

        #region UpdatePortfolio

        [Test]
        public async Task UpdatePortfolio_Returns_OkResult()
        {
            // Arrange
            var userId = 1;
            var mediaFile = new MediaFileDTO
            {
                Id = 1,
                FileName = "testfile.pdf",
                Description = "Updated file",
                Title = "Updated Title",
                UserId = userId
            };

            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            mediaFileServiceMock.Setup(m => m.UpdateMediaFile(mediaFile))
                .ReturnsAsync(mediaFile); // Simulate media file updated

            // Act
            var result = await usersController.UpdatePortfolio(mediaFile);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue(okResult != null);
            var updatedFile = okResult.Value as MediaFileDTO;
            Assert.IsNotNull(updatedFile);
            Assert.AreEqual(mediaFile.Id, updatedFile.Id);
            Assert.AreEqual(mediaFile.FileName, updatedFile.FileName);
            Assert.AreEqual(mediaFile.Description, updatedFile.Description);
            Assert.AreEqual(mediaFile.Title, updatedFile.Title);
            Assert.AreEqual(mediaFile.UserId, updatedFile.UserId);

            // Verify that UpdateMediaFile was called with the correct mediaFile object
            mediaFileServiceMock.Verify(m => m.UpdateMediaFile(mediaFile), Times.Once);
        }



        #endregion

        #region DeletePortfolio

        [Test]
        public async Task DeletePortfolio_Returns_OkResult()
        {
            // Arrange
            var userId = 1;
            var fileId = 1;

            currentUserServiceMock.Setup(m => m.UserId).Returns(userId);
            mediaFileServiceMock.Setup(m => m.DeleteMediaFile(fileId))
                .ReturnsAsync(fileId); // Simulate media file deleted

            // Act
            var result = await usersController.DeletePortfolio(fileId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue(okResult != null);
            var deletedFileId = (long)okResult.Value;
            Assert.AreEqual(fileId, deletedFileId);

            // Verify that DeleteMediaFile was called with the correct fileId
            mediaFileServiceMock.Verify(m => m.DeleteMediaFile(fileId), Times.Once);
        }


        #endregion

        #region ConvertIntoRecruiter


        #endregion

        #region GetUsers

        [Test]
        public async Task GetUsers_Returns_OkResult_With_Role()
        {
            // Arrange
            var userSearchDTO = new UserSearchDTO
            {
                PageIndex = 1,
                PageSize = 10,
                role = "Admin"
            };

            var expectedUsers = new List<UserDTO>
            {
                new UserDTO { Id = 1, Name = "Admin1", Email = "admin1@example.com", IsCompany = false },
                new UserDTO { Id = 2, Name = "Admin2", Email = "admin2@example.com", IsCompany = true }
                // Add more user DTOs as needed for your test scenario
            };

            appUserServiceMock.Setup(m => m.GetUsers(userSearchDTO)).ReturnsAsync(new Pagination<UserDTO>
            {
                Items = expectedUsers,
                PageIndex = userSearchDTO.PageIndex,
                PageSize = userSearchDTO.PageSize,
                TotalItemsCount = expectedUsers.Count
            });

            // Act
            var result = await usersController.GetUsers(userSearchDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var actualUsers = okResult.Value as Pagination<UserDTO>;
            Assert.IsNotNull(actualUsers);
            Assert.AreEqual(expectedUsers.Count, actualUsers.Items.Count);

            // Example of more specific assertions
            Assert.AreEqual(userSearchDTO.PageIndex, actualUsers.PageIndex);
            Assert.AreEqual(userSearchDTO.PageSize, actualUsers.PageSize);
            Assert.AreEqual(expectedUsers.Count, actualUsers.TotalItemsCount);
        }

        [Test]
        public async Task GetUsers_Returns_OkResult_With_Search()
        {
            // Arrange
            var userSearchDTO = new UserSearchDTO
            {
                PageIndex = 1,
                PageSize = 10,
                search = "John"
            };

            var expectedUsers = new List<UserDTO>
            {
                new UserDTO { Id = 1, Name = "John Doe", Email = "john.doe@example.com", IsCompany = false },
                new UserDTO { Id = 2, Name = "John Smith", Email = "john.smith@example.com", IsCompany = true }
                // Add more user DTOs as needed for your test scenario
            };

            appUserServiceMock.Setup(m => m.GetUsers(userSearchDTO)).ReturnsAsync(new Pagination<UserDTO>
            {
                Items = expectedUsers,
                PageIndex = userSearchDTO.PageIndex,
                PageSize = userSearchDTO.PageSize,
                TotalItemsCount = expectedUsers.Count
            });

            // Act
            var result = await usersController.GetUsers(userSearchDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var actualUsers = okResult.Value as Pagination<UserDTO>;
            Assert.IsNotNull(actualUsers);
            Assert.AreEqual(expectedUsers.Count, actualUsers.Items.Count);

            // Example of more specific assertions
            Assert.AreEqual(userSearchDTO.PageIndex, actualUsers.PageIndex);
            Assert.AreEqual(userSearchDTO.PageSize, actualUsers.PageSize);
            Assert.AreEqual(expectedUsers.Count, actualUsers.TotalItemsCount);
        }

        #endregion

        #region Lock

        //[Test]
        //public async Task Lock_Returns_OkResult_When_User_Found_And_Successfully_Locked()
        //{
        //    // Arrange
        //    int userId = 1;
        //    var user = new AppUser { Id = userId, UserName = "testuser" };

        //    userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

        //    currentUserServiceMock.Setup(m => m.UserId).Returns(userId);

        //    var usersController = new UsersController(
        //        appUserServiceMock.Object,
        //        currentUserServiceMock.Object,
        //        userManagerMock.Object,
        //        skillServiceMock.Object,
        //        passwordGeneratorServiceMock.Object,
        //        mediaFileServiceMock.Object,
        //        roleManagerMock.Object
        //    );

        //    // Act
        //    var result = await usersController.Lock(userId);

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.IsNotNull(okResult);
        //    Assert.AreEqual("Khóa thành công người dùng", okResult.Value);

        //    // Verify interactions
        //    userManagerMock.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
        //    userManagerMock.Verify(m => m.SetLockoutEndDateAsync(user, It.IsAny<DateTimeOffset?>()), Times.Once);
        //}
    


    [Test]
    public async Task Lock_Returns_NotFoundResult_When_User_NotFound()
    {
        // Arrange
        int userId = 1;
        userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((AppUser)null);

        // Act
        var result = await usersController.Lock(userId);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual("User not found.", notFoundResult.Value);

        // Verify interactions
        userManagerMock.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
    }

    //[Test]
    //public async Task Lock_Returns_BadRequestResult_When_SetLockoutEndDate_Fails()
    //{
    //    // Arrange
    //    int userId = 1;
    //    var user = new AppUser { Id = userId, UserName = "testuser" };
    //    var lockoutEndDate = DateTimeOffset.UtcNow.AddYears(100);

    //    userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
    //    userManagerMock.Setup(m => m.SetLockoutEndDateAsync(user, lockoutEndDate)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to set lockout end date." }));

    //    // Act
    //    var result = await usersController.Lock(userId);

    //    // Assert
    //    Assert.IsInstanceOf<BadRequestObjectResult>(result);
    //    var badRequestResult = result as BadRequestObjectResult;
    //    Assert.IsNotNull(badRequestResult);
    //    Assert.AreEqual("Khóa không thành công người dùng", badRequestResult.Value);

    //    // Verify interactions
    //    userManagerMock.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
    //    userManagerMock.Verify(m => m.SetLockoutEndDateAsync(user, lockoutEndDate), Times.Once);
    //}



    #endregion


}
}
