using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using Application.DTOs;
using Application.IServices;
using Application.Services;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTestProject.Controller
{
    [TestFixture]
    public class CategoryControllerTest
    {
        private Mock<ICategoryService> _mockCategoryService;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private CategoriesController _controller;


        [SetUp]
        public void Setup()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockCategoryService.Object, _categoryRepositoryMock.Object);
        }

        #region GetAllCategories
        [Test]
        public async Task Index_ReturnsOkResult_WithCategories()
        {
            // Arrange
            int pageIndex = 1;
            int pageSize = 10;
            var mockCategories = new Pagination<CategoryDTO>
            {
                Items = new List<CategoryDTO>
                {
                    new CategoryDTO
                    {
                        Id = 1,
                        CategoryName = "Category1",
                        Image = "image1.png",
                        TotalProjects = 5,
                        IsDeleted = false
                    }
                },
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = 1
            };

            _mockCategoryService.Setup(service => service.GetAllHomePage(pageIndex, pageSize))
                .ReturnsAsync(mockCategories);

            // Act
            var result = await _controller.Index(pageIndex, pageSize) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(mockCategories, result.Value);
        }

        [Test]
        public async Task Index_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int pageIndex = 1;
            int pageSize = 10;

            _mockCategoryService.Setup(service => service.GetAllHomePage(pageIndex, pageSize))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Index(pageIndex, pageSize) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual("Internal server error", (result.Value as dynamic).message);
        }



        #endregion

    }
}
