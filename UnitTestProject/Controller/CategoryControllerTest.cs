using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using Application.DTOs;
using Application.IServices;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTestProject.Controller
{
    [TestFixture]
    public class CategoryControllerTest
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private CategoriesController _controller;

        [SetUp]
        public void Setup()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoriesController(_categoryServiceMock.Object);
        }

        #region GetAllCategories

        [Test]
        public async Task Index_ReturnsOkWithCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, CategoryName = "Category 1" },
                new Category { Id = 2, CategoryName = "Category 2" },
                new Category { Id = 3, CategoryName = "Category 3" }
            };

            var categoryDTOs = categories.Select(c => new CategoryDTO { Id = c.Id, CategoryName = c.CategoryName }).ToList();

            _categoryServiceMock.Setup(c => c.GetAll()).ReturnsAsync(categoryDTOs);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var model = okResult.Value as List<CategoryDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(categoryDTOs.Count, model.Count);
        }

        [Test]
        public async Task Index_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _categoryServiceMock.Setup(service => service.GetAll()).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }


        #endregion

    }
}
