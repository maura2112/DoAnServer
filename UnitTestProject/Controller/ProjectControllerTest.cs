using API.Controllers;
using Application.DTOs;
using Application.IServices;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Linq.Expressions;

namespace UnitTestProject.Controller
{
    [TestFixture]
    public class ProjectControllerTest
    {
        private Mock<IProjectService> _mockProjectService;
        private Mock<ICurrentUserService> _mockCurrentUserService;
        private Mock<ISkillService> _mockSkillService;
        private Mock<IProjectRepository> _mockProjectRepository;
        private ProjectsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockProjectService = new Mock<IProjectService>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockSkillService = new Mock<ISkillService>();
            _mockProjectRepository = new Mock<IProjectRepository>();

            _controller = new ProjectsController(
                _mockProjectService.Object,
                _mockCurrentUserService.Object,
                _mockSkillService.Object,
                _mockProjectRepository.Object
            );
        }
        #region GetAll
        [Test]
        public async Task Index_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _controller.Index(1, 10);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result); // Chỉnh sửa đoạn này để mong đợi ObjectResult
            var badRequestResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(badRequestResult.StatusCode, Is.EqualTo(400)); // Kiểm tra StatusCode của ObjectResult
                Assert.That(badRequestResult.Value.GetType(), Is.EqualTo(typeof(SerializableError))); // Kiểm tra kiểu dữ liệu của Value
            });
        }

        [Test]
        public async Task Index_InvalidPageIndexOrPageSize_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.Index(0, 10);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult, "Expected BadRequestObjectResult but was null");

            var errorResponse = badRequestResult.Value as SerializableError;
            Assert.IsNotNull(errorResponse, "Expected SerializableError but was null");

            Assert.IsTrue(errorResponse.ContainsKey(string.Empty));
            var messages = errorResponse[string.Empty] as string[];
            Assert.IsNotNull(messages, "Expected 'message' to be string[] but was null");

            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual("Số trang hoặc kích cỡ trang lớn hơn 1", messages[0]);
        }




        [Test]
        public async Task Index_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var pagination = new Pagination<ProjectDTO>
            {
                Items = new List<ProjectDTO>(),
                PageIndex = 1,
                PageSize = 10
            };

            _mockProjectService.Setup(service => service.Get(1, 10)).ReturnsAsync(pagination);

            // Act
            var result = await _controller.Index(1, 10);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(pagination, okResult.Value);
        }
        #endregion
        #region Search
        [Test]
        public async Task Search_ValidModelStateAndKeyword_ReturnsOk()
        {
            // Arrange
            var projects = new ProjectSearchDTO
            {
                Keyword = "test",
                PageIndex = 1,
                PageSize = 10
            };
            _controller.ModelState.Clear(); // Đảm bảo ModelState rỗng

            var expectedProjects = new Pagination<ProjectDTO>
            {
                Items = new List<ProjectDTO>
            {
                new ProjectDTO { Id = 1, Title = "Test Project 1" },
                new ProjectDTO { Id = 2, Title = "Test Project 2" }
            },
                PageIndex = 1,
                PageSize = 10,
                TotalItemsCount = 2
            };

            _mockProjectService.Setup(x => x.GetWithFilter(It.IsAny<Expression<Func<Domain.Entities.Project, bool>>>(), projects.PageIndex, projects.PageSize))
                               .ReturnsAsync(expectedProjects);

            // Act
            var result = await _controller.Search(projects);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var projectsResult = okResult.Value as Pagination<ProjectDTO>;
            Assert.IsNotNull(projectsResult);
            Assert.IsTrue(projectsResult.Items.All(p => p.Title.ToLower().Contains("test")));
            Assert.AreEqual(2, projectsResult.Items.Count); // Kiểm tra số lượng dự án trả về, tuỳ theo logic của bạn
            Assert.AreEqual(1, projectsResult.PageIndex);
            Assert.AreEqual(10, projectsResult.PageSize);
            Assert.AreEqual(2, projectsResult.TotalItemsCount);
        }

        [Test]
        public async Task Search_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var projects = new ProjectSearchDTO();
            _controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _controller.Search(projects);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }




        [Test]
        public async Task Search_NoKeyword_ReturnsOkWithAllProjects()
        {
            // Arrange
            var projects = new ProjectSearchDTO
            {
                PageIndex = 1,
                PageSize = 10
            };
            _controller.ModelState.Clear(); // Đảm bảo ModelState rỗng

            var expectedProjects = new Pagination<ProjectDTO>
            {
                Items = new List<ProjectDTO>
            {
                new ProjectDTO { Id = 1, Title = "Test Project 1" },
                new ProjectDTO { Id = 2, Title = "Test Project 2" }
            },
                PageIndex = 1,
                PageSize = 10,
                TotalItemsCount = 2
            };

            _mockProjectService.Setup(x => x.GetWithFilter(It.IsAny<Expression<Func<Domain.Entities.Project, bool>>>(), projects.PageIndex, projects.PageSize))
                               .ReturnsAsync(expectedProjects);

            // Act
            var result = await _controller.Search(projects);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var projectsResult = okResult.Value as Pagination<ProjectDTO>;
            Assert.IsNotNull(projectsResult);
            Assert.AreEqual(2, projectsResult.Items.Count); // Kiểm tra số lượng dự án trả về, tuỳ theo logic của bạn
            Assert.AreEqual(1, projectsResult.PageIndex);
            Assert.AreEqual(10, projectsResult.PageSize);
            Assert.AreEqual(2, projectsResult.TotalItemsCount);
        }
        #endregion
        #region Filter
        [Test]
        public async Task Filter_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var projects = new ProjectFilter();
            _controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _controller.Filter(projects);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            // Assert that the value is a SerializableError
            Assert.IsNotNull(objectResult.Value);
            var serializableError = objectResult.Value as SerializableError;
            Assert.IsNotNull(serializableError);

            // Optional: Check if service method was not called
            _mockProjectService.Verify(x => x.GetWithFilter(It.IsAny<Expression<Func<Project, bool>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }





        [Test]
        public async Task Filter_ValidModelState_ReturnsOkWithFilteredData()
        {
            // Arrange
            var projects = new ProjectFilter
            {
                Keyword = "test",
                CategoryId = 1,
                SkillIds = new List<int> { 1, 2 },
                Duration = 5,
                MinBudget = 100,
                MaxBudget = 1000,
                PageIndex = 1,
                PageSize = 10
            };

            var filteredProjects = new Pagination<ProjectDTO>
            {
                Items = new List<ProjectDTO>
        {
            new ProjectDTO { Id = 1, Title = "Project 1" },
            new ProjectDTO { Id = 2, Title = "Project 2" }
        },
                PageIndex = 1,
                PageSize = 10,
                TotalItemsCount = 2
            };

            _mockProjectService.Setup(x => x.GetWithFilter(It.IsAny<Expression<Func<Project, bool>>>(), projects.PageIndex, projects.PageSize))
                               .ReturnsAsync(filteredProjects);

            // Act
            var result = await _controller.Filter(projects);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var model = okResult.Value as Pagination<ProjectDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Items.Count); // Check số lượng items trả về
            Assert.AreEqual(1, model.PageIndex); // Check PageIndex
            Assert.AreEqual(10, model.PageSize); // Check PageSize
            Assert.AreEqual(2, model.TotalItemsCount); // Check TotalItemsCount

            // Optional: Check if service method was called with correct filter
            _mockProjectService.Verify(x => x.GetWithFilter(It.IsAny<Expression<Func<Project, bool>>>(), projects.PageIndex, projects.PageSize), Times.Once);
        }


        #endregion
        #region GetListByUserId
        [Test]
        public async Task GetListByUserId_ValidModelState_ReturnsOkWithFilteredData()
        {
            // Arrange
            var projects = new ProjectListDTO
            {
                UserId = 1,
                PageIndex = 1,
                PageSize = 10
            };

            var filteredProjects = new Pagination<ProjectDTO>
            {
                Items = new List<ProjectDTO>
            {
                new ProjectDTO { Id = 1, Title = "Project 1" },
                new ProjectDTO { Id = 2, Title = "Project 2" }
            },
                PageIndex = 1,
                PageSize = 10,
                TotalItemsCount = 2
            };

            _mockProjectService.Setup(x => x.GetWithFilter(
                    It.IsAny<Expression<Func<Project, bool>>>(),
                    projects.PageIndex,
                    projects.PageSize))
                .ReturnsAsync(filteredProjects);

            // Act
            var result = await _controller.GetListByUserId(projects) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var model = result.Value as Pagination<ProjectDTO>;
            Assert.NotNull(model);
            Assert.AreEqual(2, model.Items.Count); // Check số lượng items trả về
            Assert.AreEqual(1, model.PageIndex); // Check PageIndex
            Assert.AreEqual(10, model.PageSize); // Check PageSize
            Assert.AreEqual(2, model.TotalItemsCount); // Check TotalCount

            // Verify that service method was called with correct filter
            _mockProjectService.Verify(x => x.GetWithFilter(
                    It.IsAny<Expression<Func<Project, bool>>>(),
                    projects.PageIndex,
                    projects.PageSize), Times.Once);
        }

        [Test]
        public async Task GetListByUserId_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var projects = new ProjectListDTO();
            _controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _controller.GetListByUserId(projects);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            // Optional: Check if service method was not called
            _mockProjectService.Verify(x => x.GetWithFilter(It.IsAny<Expression<Func<Project, bool>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
        #endregion
        #region GetProjectDetails
        [Test]
        public async Task GetDetailProject_ValidId_ReturnsOk()
        {
            // Arrange
            int projectId = 1;
            var projectDetail = new ProjectDTO
            {
                Id = projectId,
                Title = "Test Project",
                Description = "Description of test project",
                // Add other properties as needed
            };

            _mockProjectService.Setup(x => x.GetDetailProjectById(projectId))
                               .ReturnsAsync(projectDetail);

            // Act
            var result = await _controller.GetDetailProject(projectId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var model = result.Value as ProjectDTO;
            Assert.NotNull(model);
            Assert.AreEqual(projectId, model.Id); // Check the ID
            Assert.AreEqual("Test Project", model.Title); // Check the title
            Assert.AreEqual("Description of test project", model.Description); // Check the description

            // Verify that service method was called with correct project ID
            _mockProjectService.Verify(x => x.GetDetailProjectById(projectId), Times.Once);
        }

        [Test]
        public async Task GetDetailProject_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            int projectId = 1;
            _controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _controller.GetDetailProject(projectId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            // Verify that service method was not called
            _mockProjectService.Verify(x => x.GetDetailProjectById(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task GetDetailProject_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            int projectId = 1;
            _mockProjectService.Setup(x => x.GetDetailProjectById(projectId))
                               .ReturnsAsync((ProjectDTO)null); // Simulate project not found

            // Act
            var result = await _controller.GetDetailProject(projectId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);

            // Verify that service method was called with correct project ID
            _mockProjectService.Verify(x => x.GetDetailProjectById(projectId), Times.Once);
        }

        #endregion
        #region Create Project
        [Test]
        public async Task AddAsync_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Title is required");

            var addProjectDTO = new AddProjectDTO
            {
                // Missing Title
                Description = "Test Description",
                Skill = new List<string> { "skill1", "skill2", "skill3" }
            };

            // Act
            var result = await _controller.AddAsync(addProjectDTO, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result); // Ensure result is not null

            var badRequestResult = result as ObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Test]
        public async Task AddAsync_ServiceAddReturnsNull_ReturnsInternalServerError()
        {
            // Arrange
            var dto = new AddProjectDTO();
            _mockProjectService.Setup(service => service.Add(dto)).ReturnsAsync((ProjectDTO)null);

            // Act
            var result = await _controller.AddAsync(dto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            var response = objectResult.Value as ProjectResponse;
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Failed to create project.", response.Message);
        }

        [Test]
        public async Task AddAsync_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var dto = new AddProjectDTO
            {
                Title = "Project Game Unity 3D",
                CategoryId = 1,
                MinBudget = 20000000,
                MaxBudget = 50000000,
                Duration = 2,
                Description = "",
                Skill = new List<string> { ".Net", "Java" }

            };

            var projectDto = new ProjectDTO
            {
                Id = 1,
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                MinBudget = dto.MinBudget,
                MaxBudget = dto.MaxBudget,
                Duration = dto.Duration,
                Description = dto.Description,
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                StatusId = 1,
                Skill = dto.Skill
            };

            _mockProjectService.Setup(service => service.Add(dto)).ReturnsAsync(projectDto);
            _mockCurrentUserService.Setup(x => x.UserId).Returns(1);

            // Act
            var result = await _controller.AddAsync(dto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var response = okResult.Value as ProjectResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Bạn vừa tạo dự án thành công", response.Message);
            Assert.AreEqual(projectDto, response.Data);

            // Verify that the skill service's AddSkillForProject method was called
            _mockSkillService.Verify(service => service.AddSkillForProject(dto.Skill, projectDto.Id), Times.Once);
        }



        #endregion
        #region Update Project
        [Test]
        public async Task UpdateAsync_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");
            var dto = new UpdateProjectDTO();

            // Act
            var result = await _controller.UpdateAsync(dto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateAsync_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new UpdateProjectDTO { Id = 1 };
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(dto.Id)).ReturnsAsync((Project)null);

            // Act
            var result = await _controller.UpdateAsync(dto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            // Assert the content of NotFoundObjectResult
            Assert.IsNotNull(notFoundResult.Value);
            Assert.IsTrue(notFoundResult.Value.ToString().Contains("Không tìm thấy dự án phù hợp!"));

            // Verify that Update method in _mockProjectService was not called
            _mockProjectService.Verify(service => service.Update(It.IsAny<UpdateProjectDTO>()), Times.Never);
        }



        [Test]
        public async Task UpdateAsync_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var dto = new UpdateProjectDTO
            {
                Id = 1,
                Title = "Updated Project",
                CategoryId = 1,
                MinBudget = 1000,
                MaxBudget = 5000,
                Duration = 30,
                Description = "Updated Description",
                Skill = new List<string> { "Skill1", "Skill2" }
            };

            var existingProject = new Project
            {
                Id = dto.Id,
                Title = "Original Project",
                CategoryId = 1,
                MinBudget = 1000,
                MaxBudget = 5000,
                Duration = 30,
                Description = "Original Description"
            };

            var updatedProjectDto = new ProjectDTO
            {
                Id = dto.Id,
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                MinBudget = dto.MinBudget,
                MaxBudget = dto.MaxBudget,
                Duration = dto.Duration,
                Description = dto.Description,
                UpdatedDate = DateTime.Now,
                Skill = dto.Skill
            };

            var skills = new List<Skill>
            {
                new Skill { SkillName = "Skill1" },
                new Skill { SkillName = "Skill2" }
            };

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(dto.Id)).ReturnsAsync(existingProject);
            _mockProjectService.Setup(service => service.Update(dto)).ReturnsAsync(updatedProjectDto);
            _mockSkillService.Setup(service => service.AddSkillForProject(It.IsAny<List<string>>(), It.IsAny<int>()))
                .ReturnsAsync(skills);

            // Act
            var result = await _controller.UpdateAsync(dto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var response = okResult.Value as ProjectResponse;
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("Bạn vừa cập nhật dự án thành công", response.Message);
            Assert.AreEqual(updatedProjectDto, response.Data);

            // Verify that the skill service's AddSkillForProject method was called
            _mockSkillService.Verify(service => service.AddSkillForProject(dto.Skill, updatedProjectDto.Id), Times.Once);
        }
        #endregion
        #region Delete Project
        [Test]
        public async Task DeleteAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = await _controller.DeleteAsync(1, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            // Verify that Delete method in _mockProjectService was not called
            _mockProjectService.Verify(service => service.Delete(It.IsAny<int>()), Times.Never);
        }
        [Test]
        public async Task DeleteAsync_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Project)null);

            // Act
            var result = await _controller.DeleteAsync(1, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            // Assert the content of NotFoundObjectResult
            Assert.IsNotNull(notFoundResult.Value);
            Assert.IsTrue(notFoundResult.Value.ToString().Contains("Không tìm thấy dự án phù hợp!"));

            // Verify that Delete method in _mockProjectService was not called
            _mockProjectService.Verify(service => service.Delete(It.IsAny<int>()), Times.Never);
        }
        [Test]
        public async Task DeleteAsync_ValidProjectId_ReturnsOk()
        {
            // Arrange
            int projectId = 1;
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync(new Project { Id = projectId }); // Simulate project found

            // Act
            var result = await _controller.DeleteAsync(projectId, CancellationToken.None) as ObjectResult;
            var responseData = result.Value as ProjectResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsNotNull(responseData);
            Assert.IsTrue(responseData.Success);
            Assert.AreEqual("Bạn vừa xóa dự án thành công", responseData.Message);
            //Assert.AreEqual(projectId, responseData.Data.Id); // Example: assuming Data has an Id property
        }

        #endregion
        #region Update Project's Status
        [Test]
        public async Task UpdateStatus_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("StatusId", "Required");

            // Act
            var result = await _controller.UpdateStatus(new Application.DTOs.ProjectStatus { Id = 1 }, CancellationToken.None) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Test]
        public async Task UpdateStatus_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            int projectId = 1;
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync((Project)null); // Simulate project not found

            // Act
            var result = await _controller.UpdateStatus(new Application.DTOs.ProjectStatus { Id = projectId }, CancellationToken.None) as ObjectResult;
            var responseData = result.Value as ProjectResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.IsNotNull(responseData);
            Assert.IsFalse(responseData.Success);
            Assert.AreEqual("Không tìm thấy dự án phù hợp!", responseData.Message);
            Assert.IsNull(responseData.Data);
        }

        [Test]
        public async Task UpdateStatus_ValidModel_ReturnsOk()
        {
            // Arrange
            int projectId = 1;
            int statusId = 2;
            var projectDto = new ProjectDTO { Id = projectId, StatusId = statusId };

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync(new Project { Id = projectId }); // Simulate project found
            _mockProjectService.Setup(service => service.UpdateStatus(projectId, statusId))
                .ReturnsAsync(projectDto); // Simulate successful update

            // Act
            var result = await _controller.UpdateStatus(new Application.DTOs.ProjectStatus { Id = projectId, StatusId = statusId }, CancellationToken.None) as ObjectResult;
            var responseData = result.Value as ProjectResponse;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsNotNull(responseData);
            Assert.IsTrue(responseData.Success);
            Assert.AreEqual("Bạn vừa thay đổi trạng thái dự án thành công", responseData.Message);
            Assert.AreEqual(projectId, responseData.Data.Id);
        }
        #endregion
    }
}
