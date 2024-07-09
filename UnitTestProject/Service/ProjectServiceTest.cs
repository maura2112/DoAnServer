//using Application.DTOs;
//using Application.Extensions;
//using Application.IServices;
//using Application.Services;
//using AutoMapper;
//using Domain.Common;
//using Domain.Entities;
//using Domain.IRepositories;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ProjectStatus = Domain.Entities.ProjectStatus;

//namespace UnitTestProject.Service
//{
//    [TestFixture]
//    public class ProjectServiceTest
//    {
//        private ProjectService _projectService;
//        private Mock<IMapper> _mapperMock;
//        private Mock<IProjectRepository> _projectRepositoryMock;
//        private Mock<IAppUserRepository> _appUserRepositoryMock;
//        private Mock<ICategoryRepository> _categoryRepositoryMock;
//        private Mock<IProjectSkillRepository> _projectSkillRepositoryMock;
//        private Mock<ICurrentUserService> _currentUserServiceMock;
//        private Mock<IAddressRepository> _addressRepositoryMock;
//        private Mock<IStatusRepository> _statusRepositoryMock;
//        private  PaginationService<ProjectDTO> _paginationServiceMock;

//        [SetUp]
//        public void Setup()
//        {
//            _mapperMock = new Mock<IMapper>();
//            _projectRepositoryMock = new Mock<IProjectRepository>();
//            _appUserRepositoryMock = new Mock<IAppUserRepository>();
//            _categoryRepositoryMock = new Mock<ICategoryRepository>();
//            _projectSkillRepositoryMock = new Mock<IProjectSkillRepository>();
//            _currentUserServiceMock = new Mock<ICurrentUserService>();
//            _addressRepositoryMock = new Mock<IAddressRepository>();
//            _statusRepositoryMock = new Mock<IStatusRepository>();
//            _paginationServiceMock = new PaginationService<ProjectDTO>();


//            _projectService = new ProjectService(
//                _mapperMock.Object,
//                _projectRepositoryMock.Object,
//                It.IsAny<IUrlRepository>(), // Mock for IUrlRepository if needed
//                _appUserRepositoryMock.Object,
//                _categoryRepositoryMock.Object,
//                _projectSkillRepositoryMock.Object,
//                _currentUserServiceMock.Object,
//                _addressRepositoryMock.Object,
//                _statusRepositoryMock.Object,
//                _paginationServiceMock
//            );
//        }
//        #region Add Project
//        [Test]
//        public async Task Add_Project_Success()
//        {
//            // Arrange
//            var addProjectDto = new AddProjectDTO
//            {
//                Title = "Test Project",
//                CategoryId = 1,
//                MinBudget = 1000,
//                MaxBudget = 2000,
//                Duration = 30,
//                Description = "Test description",
//                Skill = new List<string> { "Skill 1", "Skill 2" }
//            };

//            var userId = 1; // Simulate current user id
//            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

//            var createdProject = new Project
//            {
//                Title = addProjectDto.Title,
//                CategoryId = addProjectDto.CategoryId,
//                MinBudget = addProjectDto.MinBudget,
//                MaxBudget = addProjectDto.MaxBudget,
//                Duration = addProjectDto.Duration,
//                Description = addProjectDto.Description,
//                CreatedBy = userId,
//                CreatedDate = DateTime.Now,
//                StatusId = 1,
//                IsDeleted = false
//            };

//            var projectDto = new ProjectDTO
//            {
//                Title = createdProject.Title,
//                CategoryId = createdProject.CategoryId,
//                MinBudget = createdProject.MinBudget,
//                MaxBudget = createdProject.MaxBudget,
//                Duration = createdProject.Duration,
//                CreatedBy = createdProject.CreatedBy,
//                CreatedDate = createdProject.CreatedDate,
//                StatusId = createdProject.StatusId
//            };

//            _mapperMock.Setup(x => x.Map<Project>(addProjectDto)).Returns(createdProject);
//            _mapperMock.Setup(x => x.Map<ProjectDTO>(createdProject)).Returns(projectDto);

//            _appUserRepositoryMock.Setup(x => x.GetByIdAsync(userId))
//                                  .ReturnsAsync(new AppUser { Id = userId });

//            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(addProjectDto.CategoryId))
//                                   .ReturnsAsync(new Category { Id = addProjectDto.CategoryId });

//            _statusRepositoryMock.Setup(x => x.GetByIdAsync(createdProject.StatusId))
//                                 .ReturnsAsync(new ProjectStatus { Id = createdProject.StatusId });

//            _projectSkillRepositoryMock.Setup(x => x.GetListProjectSkillByProjectId(createdProject.Id))
//                                       .ReturnsAsync(new List<Skill>
//                                       {
//                                           new Skill { SkillName = "Skill 1" },
//                                           new Skill { SkillName = "Skill 2" }
//                                       });

//            _addressRepositoryMock.Setup(x => x.GetAddressByUserId(userId))
//                                  .ReturnsAsync(new Address { UserId = userId });

//            _projectRepositoryMock.Setup(x => x.AddAsync(createdProject)).Returns(Task.CompletedTask);

//            // Act
//            var result = await _projectService.Add(addProjectDto);

//            // Assert
//            Assert.AreEqual(projectDto.Title, result.Title);
//            Assert.AreEqual(projectDto.CategoryId, result.CategoryId);
//            Assert.AreEqual(projectDto.MinBudget, result.MinBudget);
//            Assert.AreEqual(projectDto.MaxBudget, result.MaxBudget);
//            Assert.AreEqual(projectDto.Duration, result.Duration);
//            Assert.AreEqual(projectDto.CreatedBy, result.CreatedBy);
//            Assert.AreEqual(projectDto.CreatedDate, result.CreatedDate);
//            Assert.AreEqual(projectDto.StatusId, result.StatusId);

//            Assert.AreEqual(projectDto.Skill.Count, result.Skill.Count);
//            Assert.AreEqual(projectDto.TimeAgo, result.TimeAgo);
//            Assert.AreEqual(projectDto.CreatedDateString, result.CreatedDateString);
//            Assert.AreEqual(projectDto.UpdatedDateString, result.UpdatedDateString);

//            // Additional assertions if needed
//        }

        

//        [Test]
//        public void Add_Project_Failed_ThrowsException()
//        {
//            // Arrange
//            var addProjectDto = new AddProjectDTO
//            {
//                Title = "Test Project",
//                CategoryId = 1,
//                MinBudget = 1000,
//                MaxBudget = 2000,
//                Duration = 30,
//                Description = "Test description",
//                Skill = new List<string> { "Skill 1", "Skill 2" }
//            };

//            var userId = 1;
//            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

//            var createdCategory = new Category
//            {
//                Id = addProjectDto.CategoryId,
//                CategoryName = "Test Category"
//                // Set other properties as needed
//            };

//            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(addProjectDto.CategoryId))
//                                   .ReturnsAsync(createdCategory);

//            var createdProject = new Project
//            {
//                Title = addProjectDto.Title,
//                CategoryId = addProjectDto.CategoryId,
//                MinBudget = addProjectDto.MinBudget,
//                MaxBudget = addProjectDto.MaxBudget,
//                Duration = addProjectDto.Duration,
//                Description = addProjectDto.Description,
//                CreatedBy = userId,
//                CreatedDate = DateTime.Now,
//                StatusId = 1,
//                IsDeleted = false
//            };

//            _mapperMock.Setup(x => x.Map<Project>(addProjectDto)).Returns(createdProject);
//            _mapperMock.Setup(x => x.Map<ProjectDTO>(createdProject)).Returns(new ProjectDTO());

//            _appUserRepositoryMock.Setup(x => x.GetByIdAsync(userId))
//                                  .ReturnsAsync(new AppUser { Id = userId });

//            _statusRepositoryMock.Setup(x => x.GetByIdAsync(createdProject.StatusId))
//                                 .ReturnsAsync(new ProjectStatus { Id = createdProject.StatusId });

//            _projectSkillRepositoryMock.Setup(x => x.GetListProjectSkillByProjectId(createdProject.Id))
//                                       .ReturnsAsync(new List<Skill>
//                                       {
//                                   new Skill { SkillName = "Skill 1" },
//                                   new Skill { SkillName = "Skill 2" }
//                                       });

//            _addressRepositoryMock.Setup(x => x.GetAddressByUserId(userId))
//                                  .ReturnsAsync(new Address { UserId = userId });

//            _projectRepositoryMock.Setup(x => x.AddAsync(createdProject))
//                                  .ThrowsAsync(new Exception("Tạo dự án mới thất bại"));

//            // Act & Assert
//            Assert.ThrowsAsync<Exception>(async () => await _projectService.Add(addProjectDto));
//        }
//        [Test]
//        public async Task Add_Project_CategoryNotFound_ThrowsException()
//        {
//            // Arrange
//            var addProjectDto = new AddProjectDTO
//            {
//                Title = "Test Project",
//                CategoryId = 1,
//                MinBudget = 1000,
//                MaxBudget = 2000,
//                Duration = 30,
//                Description = "Test description",
//                Skill = new List<string> { "Skill 1", "Skill 2" }
//            };

//            var userId = 1; // Simulate current user id
//            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

//            // Setup mock to return null for category
//            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(addProjectDto.CategoryId))
//                .ReturnsAsync((Category)null);

//            // Act
//            var result = await _projectService.Add(addProjectDto);

//            // Assert
//            Assert.IsNull(result); // Check if result is null when category is not found
//        }

//        [Test]
//        public async Task Add_Project_UserNotLoggedIn_ThrowsException()
//        {
//            // Arrange
//            var addProjectDto = new AddProjectDTO
//            {
//                Title = "Test Project",
//                CategoryId = 1,
//                MinBudget = 1000,
//                MaxBudget = 2000,
//                Duration = 30,
//                Description = "Test description",
//                Skill = new List<string> { "Skill 1", "Skill 2" }
//            };
//            _currentUserServiceMock.Setup(x => x.UserId).Returns(null);


//            // Act
//            var result = await _projectService.Add(addProjectDto);

//            // Assert
//            Assert.IsNull(result); 
//        }
//        #endregion

//        #region Delete Project

//        [Test]
//        public async Task Delete_Project_Success()
//        {
//            // Arrange
//            var projectId = 1;
//            var existingProject = new Project
//            {
//                Id = projectId,
//                Title = "Test Project",
//                IsDeleted = false
//            };
//            var projectDto = new ProjectDTO
//            {
//                Id = projectId,
//                Title = "Test Project",
//                IsDeleted = true
//            };

//            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId))
//                .ReturnsAsync(existingProject);
//            _projectRepositoryMock.Setup(x => x.Update(existingProject));
//            _mapperMock.Setup(x => x.Map<ProjectDTO>(existingProject))
//                .Returns(projectDto);

//            // Act
//            var result = await _projectService.Delete(projectId);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(projectId, result.Id);
//            Assert.IsTrue(result.IsDeleted);
//            _projectRepositoryMock.Verify(x => x.GetByIdAsync(projectId), Times.Once);
//            _projectRepositoryMock.Verify(x => x.Update(existingProject), Times.Once);
//            _mapperMock.Verify(x => x.Map<ProjectDTO>(existingProject), Times.Once);
//        }

//        [Test]
//        public void Delete_Project_NotFound_ThrowsException()
//        {
//            // Arrange
//            var projectId = 1;
//            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId))
//                .ReturnsAsync((Project)null);

//            // Act & Assert
//            var exception = Assert.ThrowsAsync<Exception>(async () => await _projectService.Delete(projectId));
//            Assert.AreEqual($"Project with ID {projectId} not found.", exception.Message);
//        }


//        #endregion

//        #region GetAll
//        //[Test]
//        //public async Task Get_Projects_Success()
//        //{
//        //    // Arrange
//        //    var pageIndex = 1;
//        //    var pageSize = 10;

//        //    var projects = new Pagination<Project>
//        //    {
//        //        Items = new List<Project>
//        //{
//        //    new Project
//        //    {
//        //        Id = 1,
//        //        Title = "Test Project",
//        //        CreatedBy = 1,
//        //        CategoryId = 1,
//        //        StatusId = 1,
//        //        CreatedDate = DateTime.Now
//        //    }
//        //},
//        //        PageIndex = pageIndex,
//        //        PageSize = pageSize,
//        //        TotalItemsCount = 1
//        //    };

//        //    var projectDTOs = new Pagination<ProjectDTO>
//        //    {
//        //        Items = new List<ProjectDTO>(),
//        //        PageIndex = pageIndex,
//        //        PageSize = pageSize,
//        //        TotalItemsCount = 1
//        //    };

//        //    _projectRepositoryMock.Setup(x => x.ToPagination(pageIndex, pageSize))
//        //                          .ReturnsAsync(projects);

//        //    _mapperMock.Setup(x => x.Map<Pagination<ProjectDTO>>(projects))
//        //               .Returns(projectDTOs);

//        //    foreach (var project in projects.Items)
//        //    {
//        //        var projectDTO = new ProjectDTO
//        //        {
//        //            Id = project.Id,
//        //            CreatedBy = project.CreatedBy,
//        //            AppUser = new AppUserDTO { Id = (int)project.CreatedBy, Address = new AddressDTO() },
//        //            Skill = new List<string>()
//        //        };

//        //        projectDTOs.Items.Add(projectDTO);

//        //        _mapperMock.Setup(x => x.Map<ProjectDTO>(project))
//        //                   .Returns(projectDTO);

//        //        var user = new AppUser { Id = (int)project.CreatedBy };
//        //        var userDTO = new AppUserDTO { Id = (int)project.CreatedBy, Address = new AddressDTO() };
//        //        _appUserRepositoryMock.Setup(x => x.GetByIdAsync(project.CreatedBy))
//        //                              .ReturnsAsync(user);
//        //        _mapperMock.Setup(x => x.Map<AppUserDTO>(user))
//        //                   .Returns(userDTO);

//        //        var address = new Address { UserId = (int)project.CreatedBy };
//        //        var addressDTO = new AddressDTO { UserId = (int)project.CreatedBy };
//        //        _addressRepositoryMock.Setup(x => x.GetAddressByUserId((int)project.CreatedBy))
//        //                              .ReturnsAsync(address);
//        //        _mapperMock.Setup(x => x.Map<AddressDTO>(address))
//        //                   .Returns(addressDTO);

//        //        var category = new Category { Id = project.CategoryId };
//        //        var categoryDTO = new CategoryDTO { Id = project.CategoryId };
//        //        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(project.CategoryId))
//        //                               .ReturnsAsync(category);
//        //        _mapperMock.Setup(x => x.Map<CategoryDTO>(category))
//        //                   .Returns(categoryDTO);

//        //        var status = new ProjectStatus { Id = project.StatusId };
//        //        var statusDTO = new ProjectStatusDTO { Id = project.StatusId };
//        //        _statusRepositoryMock.Setup(x => x.GetByIdAsync(project.StatusId))
//        //                             .ReturnsAsync(status);
//        //        _mapperMock.Setup(x => x.Map<ProjectStatusDTO>(status))
//        //                   .Returns(statusDTO);

//        //        var skills = new List<Skill> { new Skill { SkillName = "Skill 1" }, new Skill { SkillName = "Skill 2" } };
//        //        _projectSkillRepositoryMock.Setup(x => x.GetListProjectSkillByProjectId(project.Id))
//        //                                   .ReturnsAsync(skills);

//        //        _projectRepositoryMock.Setup(x => x.GetAverageBudget(project.Id))
//        //                              .ReturnsAsync(1500);
//        //        _projectRepositoryMock.Setup(x => x.GetTotalBids(project.Id))
//        //                              .ReturnsAsync(5);
//        //    }

//        //    // Act
//        //    var result = await _projectService.Get(pageIndex, pageSize);

//        //    // Assert
//        //    Assert.IsNotNull(result);
//        //    Assert.AreEqual(1, result.Items.Count);

//        //    var resultItem = result.Items.First();
//        //    Assert.AreEqual(1, resultItem.Id);
//        //    Assert.AreEqual(1, resultItem.CreatedBy);

//        //    var resultUser = resultItem.AppUser;
//        //    Assert.IsNotNull(resultUser);
//        //    Assert.AreEqual(1, resultUser.Id);

//        //    var resultAddress = resultUser.Address;
//        //    Assert.IsNotNull(resultAddress);
//        //    Assert.AreEqual(1, resultAddress.UserId);

//        //    var resultCategory = resultItem.Category;
//        //    Assert.IsNotNull(resultCategory);
//        //    Assert.AreEqual(1, resultCategory.Id);

//        //    var resultStatus = resultItem.ProjectStatus;
//        //    Assert.IsNotNull(resultStatus);
//        //    Assert.AreEqual(1, resultStatus.Id);

//        //    Assert.AreEqual(2, resultItem.Skill.Count);
//        //    Assert.AreEqual("Skill 1", resultItem.Skill[0]);
//        //    Assert.AreEqual("Skill 2", resultItem.Skill[1]);
//        //    Assert.AreEqual(1500, resultItem.AverageBudget);
//        //    Assert.AreEqual(5, resultItem.TotalBids);
//        //}




//        #endregion
//    }
//}
