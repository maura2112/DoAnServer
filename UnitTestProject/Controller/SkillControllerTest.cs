//using API.Controllers;
//using Application.DTOs;
//using Application.IServices;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Domain.Entities;

//namespace UnitTestProject.Controller
//{
//    [TestFixture]
//    public class SkillControllerTest
//    {
//        private Mock<ISkillService> _mockSkillService;
//        private Mock<ICurrentUserService> _mockCurrentUserService;
//        private SkillController _controller;

//        [SetUp]
//        public void Setup()
//        {
//            _mockSkillService = new Mock<ISkillService>();
//            _mockCurrentUserService = new Mock<ICurrentUserService>();

//            _controller = new SkillController(
//                _mockSkillService.Object,
//                _mockCurrentUserService.Object
//            );
//        }

//        #region AddAsync
//        [Test]
//        public async Task AddAsync_WithValidModel_ReturnsNoContent()
//        {
//            // Arrange
//            var dto = new SkillDTO { CategoryId = 1, SkillName = "Programming", CategoryName = "IT" };
//            var token = CancellationToken.None;

//            // Act
//            var result = await _controller.AddAsync(dto, token) as IActionResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.IsInstanceOf<NoContentResult>(result);
//        }

//        [Test]
//        public async Task AddAsync_WithInvalidModel_ReturnsBadRequest()
//        {
//            // Arrange
//            var dto = new SkillDTO { }; // DTO không hợp lệ để gây ra lỗi ModelState
//            var token = CancellationToken.None;

//            // Manually validate ModelState
//            _controller.ModelState.AddModelError("CategoryName", "CategoryName is required");

//            // Act
//            var result = await _controller.AddAsync(dto, token) as ObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
//        }



//        #endregion

//        #region Search

//        [Test]
//        public async Task Search_WithValidSkills_ReturnsOkObjectResult()
//        {
//            // Arrange
//            var skills = new SkillListByCate { CategoryId = 1 };

//            // Act
//            var result = await _controller.Search(skills) as ObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

//            _mockSkillService.Verify(
//                x => x.GetWithFilter(It.IsAny<Expression<Func<Skill, bool>>>(), 1, int.MaxValue),
//                Times.Once);
//        }

//        [Test]
//        public async Task Search_WithInvalidSkills_ReturnsBadRequest()
//        {
//            // Arrange
//            var skills = new SkillListByCate { };
//            _controller.ModelState.AddModelError("CategoryId", "CategoryId is required");
//            // Act
//            var result = await _controller.Search(skills) as ObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
//        }

//        #endregion

//        #region GetAll
//        [Test]
//        public async Task GetAll_ReturnsOkObjectResult()
//        {
//            // Arrange
//            var mockSkillList = new List<SkillDTO>
//            {
//                new SkillDTO() { Id = 1, CategoryId = 1, SkillName = "Programming" },
//                new SkillDTO() { Id = 2, CategoryId = 2, SkillName = "Design" }
//            };

//            _mockSkillService.Setup(x => x.GetAll()).ReturnsAsync(mockSkillList);

//            // Act
//            var result = await _controller.GetAll() as ObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

//            var resultList = result.Value as List<SkillDTO>;
//            Assert.IsNotNull(resultList);
//            Assert.AreEqual(mockSkillList.Count, resultList.Count);
//        }


//        #endregion
//    }

//}
