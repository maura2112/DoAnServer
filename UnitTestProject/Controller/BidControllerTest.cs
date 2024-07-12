using API.Controllers;
using API.Hubs;
using Application.DTOs;
using Application.IServices;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Controller
{
    [TestFixture]
    public class BidControllerTest
    {
        private Mock<IBidService> _mockBidService;
        private Mock<IBidRepository> _mockBidRepository;
        private Mock<ICurrentUserService> _mockCurrentUserService;
        private Mock<IProjectRepository> _mockProjectRepository;
        private Mock<IAppUserRepository> _mockAppUserRepository;
        private Mock<INotificationService> _mockNotificationService;
        private Mock<IHubContext<ChatHub>> _mockChatHubContext;
        private Mock<INotificationRepository> _mockNotificationRepository;
        private BidController _controller;

        [SetUp]
        public void Setup()
        {
            _mockBidService = new Mock<IBidService>();
            _mockBidRepository = new Mock<IBidRepository>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockAppUserRepository = new Mock<IAppUserRepository>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockChatHubContext = new Mock<IHubContext<ChatHub>>();
            _mockNotificationRepository = new Mock<INotificationRepository>();

            _controller = new BidController(
                _mockBidService.Object,
                _mockBidRepository.Object,
                _mockCurrentUserService.Object,
                _mockProjectRepository.Object,
                _mockAppUserRepository.Object,
                _mockNotificationService.Object,
                _mockChatHubContext.Object,
                _mockNotificationRepository.Object
            );
        }
        #region Get List Bid By UserId
        [Test]
        public async Task GetListByUserId_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var bidSearchDTO = new BidSearchDTO { UserId = 1 };
            _mockAppUserRepository.Setup(repo => repo.GetByIdAsync(bidSearchDTO.UserId))
                .ReturnsAsync((AppUser)null);

            // Act
            var result = await _controller.GetListByUserId(bidSearchDTO) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            
        }

        [Test]
        public async Task GetListByUserId_ValidUser_ReturnsOkResultWithExpectedData()
        {
            // Arrange
            var bidSearchDTO = new BidSearchDTO { UserId = 1, PageIndex = 1, PageSize = 10 };
            var fetchedUser = new AppUser { Id = 1 };
            var expectedBids = new Pagination<BidDTO>
            {
                Items = new List<BidDTO> { new BidDTO { /* ... */ } },
                PageIndex = 1,
                PageSize = 10,
                TotalItemsCount = 1
            };

            _mockAppUserRepository.Setup(repo => repo.GetByIdAsync(bidSearchDTO.UserId))
                .ReturnsAsync(fetchedUser);

            _mockBidService.Setup(s => s.GetWithFilter(It.IsAny<Expression<Func<Domain.Entities.Bid, bool>>>(), bidSearchDTO.PageIndex, bidSearchDTO.PageSize))
                .ReturnsAsync(expectedBids);

            // Act
            var result = await _controller.GetListByUserId(bidSearchDTO) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [Test]
        public async Task GetListByUserId_ValidUser_NoBidsFound_ReturnsOkResultWithMessage()
        {
            // Arrange
            var bidSearchDTO = new BidSearchDTO { UserId = 1, PageIndex = 1, PageSize = 10 };
            var fetchedUser = new AppUser { Id = 1 };
            var expectedBids = new Pagination<BidDTO>
            {
                Items = new List<BidDTO>(),
                PageIndex = 1,
                PageSize = 10,
                TotalItemsCount = 0
            };

            _mockAppUserRepository.Setup(repo => repo.GetByIdAsync(bidSearchDTO.UserId))
                .ReturnsAsync(fetchedUser);

            _mockBidService.Setup(s => s.GetWithFilter(It.IsAny<Expression<Func<Domain.Entities.Bid, bool>>>(), bidSearchDTO.PageIndex, bidSearchDTO.PageSize))
                .ReturnsAsync(expectedBids);

            // Act
            var result = await _controller.GetListByUserId(bidSearchDTO) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        }

        [Test]
        public async Task GetListByUserId_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var bidSearchDTO = new BidSearchDTO { UserId = 1 };

            _mockAppUserRepository.Setup(repo => repo.GetByIdAsync(bidSearchDTO.UserId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetListByUserId(bidSearchDTO) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
           
        }


        #endregion
        #region Get List Bid By Project Id
        [Test]
        public async Task GetListByProjectId_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            var bidListDTO = new BidListDTO { ProjectId = 1 };
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(bidListDTO.ProjectId))
                .ReturnsAsync((Project)null);

            // Act
            var result = await _controller.GetListByProjectId(bidListDTO) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            
        }

        [Test]
        public async Task GetListByProjectId_ValidProject_ReturnsOkResultWithExpectedData()
        {
            // Arrange
            var bidListDTO = new BidListDTO { ProjectId = 1, PageIndex = 1, PageSize = 10 };
            var fetchedProject = new Project { Id = 1 };
            var expectedBids = new Pagination<BidDTO>
            {
                Items = new List<BidDTO> { new BidDTO { /* ... */ } },
                PageIndex = 1,
                PageSize = 10,
                TotalItemsCount = 1
            };

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(bidListDTO.ProjectId))
                .ReturnsAsync(fetchedProject);

            _mockBidService.Setup(s => s.GetWithFilter(It.IsAny<Expression<Func<Domain.Entities.Bid, bool>>>(), bidListDTO.PageIndex, bidListDTO.PageSize))
                .ReturnsAsync(expectedBids);

            // Act
            var result = await _controller.GetListByProjectId(bidListDTO) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [Test]
        public async Task GetListByProjectId_ValidProject_NoBidsFound_ReturnsOkResultWithMessage()
        {
            // Arrange
            var bidListDTO = new BidListDTO { ProjectId = 1, PageIndex = 1, PageSize = 10 };
            var fetchedProject = new Project { Id = 1 };
            var expectedBids = new Pagination<BidDTO>
            {
                Items = new List<BidDTO>(),
                PageIndex = 1,
                PageSize = 10,
                TotalItemsCount = 0
            };

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(bidListDTO.ProjectId))
                .ReturnsAsync(fetchedProject);

            _mockBidService.Setup(s => s.GetWithFilter(It.IsAny<Expression<Func<Domain.Entities.Bid, bool>>>(), bidListDTO.PageIndex, bidListDTO.PageSize))
                .ReturnsAsync(expectedBids);

            // Act
            var result = await _controller.GetListByProjectId(bidListDTO) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [Test]
        public async Task GetListByProjectId_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var bidListDTO = new BidListDTO { ProjectId = 1 };

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(bidListDTO.ProjectId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetListByProjectId(bidListDTO) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        #endregion
        #region Bidding
        [Test]
        public async Task Bidding_UserNotLoggedIn_ReturnsUnauthorized()
        {
            // Arrange
            _mockCurrentUserService.Setup(s => s.UserId).Returns(0);
            var biddingDTO = new BiddingDTO();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _controller.Bidding(biddingDTO, cancellationToken) as UnauthorizedResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);
        }

        [Test]
        public async Task Bidding_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockCurrentUserService.Setup(s => s.UserId).Returns(1);
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Project)null);
            var biddingDTO = new BiddingDTO { ProjectId = 1 };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _controller.Bidding(biddingDTO, cancellationToken) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public async Task Bidding_AlreadyBidding_ReturnsBadRequest()
        {
            // Arrange
            _mockCurrentUserService.Setup(s => s.UserId).Returns(1);
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Project());
            _mockBidRepository.Setup(repo => repo.CheckBidding(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            var biddingDTO = new BiddingDTO { ProjectId = 1 };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _controller.Bidding(biddingDTO, cancellationToken) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
           
        }

        [Test]
        public async Task Bidding_ValidBid_ReturnsOkResult()
        {
            // Arrange
            var userId = 1;
            var projectId = 1;
            var biddingDTO = new BiddingDTO { ProjectId = projectId };
            var bid = new BidDTO { ProjectId = projectId, AppUser = new AppUserDTO { Name = "User" }, Project = new ProjectDTO { Title = "Project", CreatedBy = 2 } };
            var cancellationToken = CancellationToken.None;

            _mockCurrentUserService.Setup(s => s.UserId).Returns(userId);
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(new Project { Id = projectId });
            _mockBidRepository.Setup(repo => repo.CheckBidding(userId, projectId)).ReturnsAsync(false);
            _mockBidService.Setup(s => s.Add(biddingDTO)).ReturnsAsync(bid);
            _mockNotificationRepository.Setup(repo => repo.GetNotificationMax()).ReturnsAsync(0);
            _mockNotificationService.Setup(service => service.AddNotification(It.IsAny<NotificationDto>())).ReturnsAsync(true);
            _mockChatHubContext.Setup(hub => hub.Clients.Client(It.IsAny<string>()).SendAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Bidding(biddingDTO, cancellationToken) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
           
        }

        [Test]
        public async Task Bidding_AddNotificationFailed_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var projectId = 1;
            var biddingDTO = new BiddingDTO { ProjectId = projectId };
            var bid = new BidDTO { ProjectId = projectId, AppUser = new AppUserDTO { Name = "User" }, Project = new ProjectDTO { Title = "Project", CreatedBy = 2 } };
            var cancellationToken = CancellationToken.None;

            _mockCurrentUserService.Setup(s => s.UserId).Returns(userId);
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(new Project { Id = projectId });
            _mockBidRepository.Setup(repo => repo.CheckBidding(userId, projectId)).ReturnsAsync(false);
            _mockBidService.Setup(s => s.Add(biddingDTO)).ReturnsAsync(bid);
            _mockNotificationRepository.Setup(repo => repo.GetNotificationMax()).ReturnsAsync(0);
            _mockNotificationService.Setup(service => service.AddNotification(It.IsAny<NotificationDto>())).ReturnsAsync(false);

            // Act
            var result = await _controller.Bidding(biddingDTO, cancellationToken) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Test]
        public async Task Bidding_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var userId = 1;
            var projectId = 1;
            var biddingDTO = new BiddingDTO { ProjectId = projectId };
            var cancellationToken = CancellationToken.None;

            _mockCurrentUserService.Setup(s => s.UserId).Returns(userId);
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(new Project { Id = projectId });
            _mockBidRepository.Setup(repo => repo.CheckBidding(userId, projectId)).ReturnsAsync(false);
            _mockBidService.Setup(s => s.Add(biddingDTO)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Bidding(biddingDTO, cancellationToken) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
           
        }
        #endregion
        #region Update Bidding
        [Test]
        public async Task UpdateBidding_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model state");
            var updateBidDTO = new UpdateBidDTO();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _controller.UpdateBidding(updateBidDTO, cancellationToken) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);

        }

        [Test]
        public async Task UpdateBidding_BidNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockBidRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Func<Bid>)null);
            var updateBidDTO = new UpdateBidDTO { Id = 1 };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _controller.UpdateBidding(updateBidDTO, cancellationToken) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            
        }

        [Test]
        public async Task UpdateBidding_ValidBid_ReturnsOkResult()
        {
            // Arrange
            var updateBidDTO = new UpdateBidDTO { Id = 1 };
            var bid = new Domain.Entities.Bid();
            var cancellationToken = CancellationToken.None;

            _mockBidRepository.Setup(repo => repo.GetByIdAsync(updateBidDTO.Id)).ReturnsAsync(bid);
            _mockBidService.Setup(service => service.Update(updateBidDTO)).ReturnsAsync(new BidDTO());

            // Act
            var result = await _controller.UpdateBidding(updateBidDTO, cancellationToken) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
           
            // Bạn có thể thêm Assert để kiểm tra dữ liệu cụ thể nếu cần
        }


        #endregion
        #region Accept Bid
        [Test]
        public async Task AcceptBidding_ValidBid_ReturnsOkResult()
        {
            // Arrange
            var bidId = 1;
            var bidAcceptedDto = new BidAccepted { Id = bidId };
            var cancellationToken = CancellationToken.None;

            var mockBidDto = new Bid();
            _mockBidRepository.Setup(repo => repo.GetByIdAsync(bidAcceptedDto.Id)).ReturnsAsync(mockBidDto);

            var acceptedBid = new BidDTO(); 
            _mockBidService.Setup(service => service.AcceptBidding(bidAcceptedDto.Id)).ReturnsAsync(acceptedBid);

            // Act
            var result = await _controller.AcceptBidding(bidAcceptedDto, cancellationToken) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            
        }


        [Test]
        public async Task AcceptBidding_BidNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var bidId = 1;
            var bidAcceptedDto = new BidAccepted { Id = bidId };
            var cancellationToken = CancellationToken.None;

            _mockBidRepository.Setup(repo => repo.GetByIdAsync(bidId)).ReturnsAsync((Bid)null);

            // Act
            var result = await _controller.AcceptBidding(bidAcceptedDto, cancellationToken) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        
        #endregion

    }
}
