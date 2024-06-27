using API.Controllers;
using Application.DTOs;
using Application.IServices;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Controller
{
    [TestFixture]
    public class ReportControllerTest
    {
        private Mock<IReportCategoryService> _reportCategoryServiceMock;
        private Mock<IUserReportService> _userReportServiceMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;
        private ReportsController _controller;

        [SetUp]
        public void SetUp()
        {
            _reportCategoryServiceMock = new Mock<IReportCategoryService>();
            _userReportServiceMock = new Mock<IUserReportService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _controller = new ReportsController(
                _reportCategoryServiceMock.Object,
                _userReportServiceMock.Object,
                _currentUserServiceMock.Object
            );
        }

        #region ReportCategories
        [Test]
        public async Task ReportCategories_ReturnsOkResult_WithReportCategories()
        {
            // Arrange
            var type = "someType";
            var reportCategories = new List<ReportCategory>
            {
                new ReportCategory { Name = "Category1", Description = "Description1" },
                new ReportCategory { Name = "Category2", Description = "Description2" }
            };

            _reportCategoryServiceMock.Setup(x => x.Categories(It.IsAny<string>()))
                .ReturnsAsync(reportCategories);

            // Act
            var result = await _controller.ReportCategories(type) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(reportCategories, result.Value);
        }


        #endregion

        #region CreateReport

        [Test]
        public async Task CreateReport_ReturnsOkResult_WithReportDTO()
        {
            // Arrange
            var userId = 123; 
            var dto = new ReportDTO
            {
                NameCreatedBy = "TruongBQ",
                ReportToUrl = "https://User/User1",
                ProjectId = 1,
                BidId = 2,
                CreatedBy = userId,
                ReportCategoryId = 3,
                Description = "Test report",
                ReportType = "TypeA",
                ReportName = "ReportA",
                ProjectName = "ProjectA",
                ProjectUser = "UserA",
                BidName = "BidA",
                BidUser = "UserB",
                IsApproved = true
            };

            _currentUserServiceMock.SetupGet(x => x.UserId).Returns(userId);
            _userReportServiceMock.Setup(x => x.CreateReport(It.IsAny<ReportDTO>()))
                .Returns(Task.CompletedTask); 

            // Act
            var result = await _controller.CreateReport(dto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(dto, result.Value);
        }

        #endregion

        #region GetReports

        [Test]
        public async Task GetReports_ReturnsOkResult_WithReportSearchDTO()
        {
            // Arrange
            var dto = new ReportSearchDTO
            {
                PageIndex = 1,
                PageSize = 10,
                typeDes = "someType",
                approved = true
            };

            var pagination = new Pagination<ReportDTO>
            {
                Items = new List<ReportDTO>
                {
                    new ReportDTO { Id = 1, ReportName = "Report1" },
                    new ReportDTO { Id = 2, ReportName = "Report2" }
                },
                TotalItemsCount = 2,
                PageIndex = dto.PageIndex,
                PageSize = dto.PageSize
            };

            _userReportServiceMock.Setup(x => x.GetReports(dto))
                .ReturnsAsync(pagination);

            // Act
            var result = await _controller.GetReports(dto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var resultValue = result.Value as Pagination<ReportDTO>;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(pagination.TotalItemsCount, resultValue.TotalItemsCount);
            Assert.AreEqual(pagination.PageIndex, resultValue.PageIndex);
            Assert.AreEqual(pagination.PageSize, resultValue.PageSize);
            Assert.AreEqual(pagination.Items, resultValue.Items);
        }
        #endregion

        #region Approve

        [Test]
        public async Task Approve_ReturnsOkResult_WhenApproved()
        {
            // Arrange
            int reportId = 1;
            _userReportServiceMock.Setup(x => x.ApproveReport(reportId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Approve(reportId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

        }

        [Test]
        public async Task Approve_ReturnsBadRequest_WhenNotApproved()
        {
            // Arrange
            int reportId = 2;
            _userReportServiceMock.Setup(x => x.ApproveReport(reportId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Approve(reportId) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }

        #endregion

    }
}
