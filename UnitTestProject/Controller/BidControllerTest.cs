using API.Controllers;
using Application.DTOs;
using Application.IServices;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private BidController _bidController;
        private Mock<IBidService> _mockBidService;
        private Mock<ICurrentUserService> _mockCurrentUserService;
        private Mock<IBidRepository> _mockBidRepository;
        private Mock<IProjectRepository> _mockProjectRepository;
        private Mock<IAppUserRepository> _mockAppUserRepository;

        [SetUp]
        public void Setup()
        {
            _mockBidService = new Mock<IBidService>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockBidRepository = new Mock<IBidRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockAppUserRepository = new Mock<IAppUserRepository>();

            _bidController = new BidController(
                _mockBidService.Object,
                _mockBidRepository.Object,
                _mockCurrentUserService.Object,
                _mockProjectRepository.Object,
                _mockAppUserRepository.Object
            );
        }
        #region Get List Bid By UserId
        [Test]
        public async Task GetListByUserId_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var bidSearchDto = new BidSearchDTO { UserId = 1, PageIndex = 1, PageSize = 10 };
            _mockAppUserRepository.Setup(repo => repo.GetByIdAsync(bidSearchDto.UserId)).ReturnsAsync((AppUser)null);

            // Act
            var result = await _bidController.GetListByUserId(bidSearchDto);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Người dùng không tồn tại!", notFoundResult.Value?.GetType().GetProperty("message")?.GetValue(notFoundResult.Value)?.ToString());
        }

        [Test]
        public async Task GetListByUserId_NoBidsForUser_ReturnsOkWithMessage()
        {
            // Arrange
            var bidSearchDto = new BidSearchDTO { UserId = 1, PageIndex = 1, PageSize = 10 };
            var user = new AppUser { Id = bidSearchDto.UserId };
            var pagedResult = new Pagination<BidDTO> { TotalItemsCount = 0, Items = new List<BidDTO>() };

            _mockAppUserRepository.Setup(repo => repo.GetByIdAsync(bidSearchDto.UserId)).ReturnsAsync(user);
            _mockBidService.Setup(service => service.GetWithFilter(It.IsAny<Expression<Func<Bid, bool>>>(), bidSearchDto.PageIndex, bidSearchDto.PageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _bidController.GetListByUserId(bidSearchDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(true, okResult.Value?.GetType().GetProperty("success")?.GetValue(okResult.Value));
            Assert.AreEqual("Bạn chưa có đấu thầu nào!", okResult.Value?.GetType().GetProperty("message")?.GetValue(okResult.Value)?.ToString());
        }

        [Test]
        public async Task GetListByUserId_BidsForUser_ReturnsOkWithData()
        {
            // Arrange
            var bidSearchDto = new BidSearchDTO { UserId = 1, PageIndex = 1, PageSize = 10 };
            var user = new AppUser { Id = bidSearchDto.UserId };
            var bids = new List<BidDTO> { new BidDTO { UserId = bidSearchDto.UserId } };
            var pagedResult = new Pagination<BidDTO> { TotalItemsCount = 1, Items = bids };

            _mockAppUserRepository.Setup(repo => repo.GetByIdAsync(bidSearchDto.UserId)).ReturnsAsync(user);
            _mockBidService.Setup(service => service.GetWithFilter(It.IsAny<Expression<Func<Bid, bool>>>(), bidSearchDto.PageIndex, bidSearchDto.PageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _bidController.GetListByUserId(bidSearchDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(true, okResult.Value?.GetType().GetProperty("success")?.GetValue(okResult.Value));
            Assert.AreEqual(null, okResult.Value?.GetType().GetProperty("message")?.GetValue(okResult.Value));
            Assert.AreEqual(pagedResult, okResult.Value?.GetType().GetProperty("data")?.GetValue(okResult.Value));
        }

        [Test]
        public async Task GetListByUserId_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var bidSearchDto = new BidSearchDTO { UserId = 1, PageIndex = 1, PageSize = 10 };
            var user = new AppUser { Id = bidSearchDto.UserId };

            _mockAppUserRepository.Setup(repo => repo.GetByIdAsync(bidSearchDto.UserId)).ReturnsAsync(user);
            _mockBidService.Setup(service => service.GetWithFilter(It.IsAny<Expression<Func<Bid, bool>>>(), bidSearchDto.PageIndex, bidSearchDto.PageSize)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _bidController.GetListByUserId(bidSearchDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.AreEqual("Internal server error", objectResult.Value?.GetType().GetProperty("message")?.GetValue(objectResult.Value)?.ToString());
        }


        #endregion
        #region Get List Bid By Project Id
        [Test]
        public async Task GetListByProjectId_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            var bidListDto = new BidListDTO { ProjectId = 1, PageIndex = 1, PageSize = 10 };
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(bidListDto.ProjectId)).ReturnsAsync((Project)null);

            // Act
            var result = await _bidController.GetListByProjectId(bidListDto);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Không tìm thấy dự án!", notFoundResult.Value?.GetType().GetProperty("message")?.GetValue(notFoundResult.Value)?.ToString());
        }

        [Test]
        public async Task GetListByProjectId_NoBidsForProject_ReturnsOkWithMessage()
        {
            // Arrange
            var bidListDto = new BidListDTO { ProjectId = 1, PageIndex = 1, PageSize = 10 };
            var project = new Project { Id = 1 };
            var pagedResult = new Pagination<BidDTO> { TotalItemsCount = 0, Items = new List<BidDTO>() };

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(bidListDto.ProjectId)).ReturnsAsync(project);
            _mockBidService.Setup(service => service.GetWithFilter(It.IsAny<Expression<Func<Bid, bool>>>(), bidListDto.PageIndex, bidListDto.PageSize)).ReturnsAsync(pagedResult);

            // Act
            var result = await _bidController.GetListByProjectId(bidListDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(true, okResult.Value?.GetType().GetProperty("success")?.GetValue(okResult.Value));
            Assert.AreEqual("Chưa có đấu thầu nào cho dự án này!", okResult.Value?.GetType().GetProperty("message")?.GetValue(okResult.Value)?.ToString());
        }

        [Test]
        public async Task GetListByProjectId_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var bidListDto = new BidListDTO { ProjectId = 1, PageIndex = 1, PageSize = 10 };
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(bidListDto.ProjectId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _bidController.GetListByProjectId(bidListDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.AreEqual("Internal server error", objectResult.Value?.GetType().GetProperty("message")?.GetValue(objectResult.Value)?.ToString());
        }

        #endregion
        #region Bidding
        [Test]
        public async Task Bidding_NotLoggedIn_ReturnsUnauthorized()
        {
            // Arrange
            _mockCurrentUserService.Setup(service => service.UserId).Returns(0); // Simulate user not logged in

            var biddingDTO = new BiddingDTO
            {
                ProjectId = 1,
                Proposal = "Test proposal",
                Duration = 10,
                Budget = 1000
            };

            // Act
            var result = await _bidController.Bidding(biddingDTO, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }



        [Test]
        public async Task Bidding_AlreadyBidding_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var existingProject = new Project
            {
                Id = 1,
                Title = "Original Project",
                CategoryId = 1,
                MinBudget = 1000,
                MaxBudget = 5000,
                Duration = 30,
                Description = "Original Description"
            };

            // Mock repository to return the existing project
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(existingProject.Id))
                                  .ReturnsAsync(existingProject);

            _mockCurrentUserService.Setup(service => service.UserId).Returns(userId); // Simulate logged in user

            // Mock repository to return true for CheckBidding
            _mockBidRepository.Setup(repo => repo.CheckBidding(userId, It.IsAny<int>()))
                              .ReturnsAsync(true);

            var biddingDTO = new BiddingDTO
            {
                ProjectId = 1,
                Proposal = "Test proposal",
                Duration = 10,
                Budget = 1000
            };

            // Act
            var result = await _bidController.Bidding(biddingDTO, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result); // Ensure result is BadRequestObjectResult

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.IsTrue(badRequestResult.Value.ToString().Contains("Bạn đã đấu thầu dự án này"));
        }



        [Test]
        public async Task Bidding_ValidRequest_ReturnsOkWithBid()
        {
            // Arrange
            var userId = 1;
            var existingProject = new Project
            {
                Id = 1,
                Title = "Original Project",
                CategoryId = 1,
                MinBudget = 1000,
                MaxBudget = 5000,
                Duration = 30,
                Description = "Original Description"
            };

            // Mock repository to return the existing project
            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(existingProject.Id))
                                  .ReturnsAsync(existingProject);
            _mockCurrentUserService.Setup(service => service.UserId).Returns(userId); // Simulate logged in user

            var biddingDTO = new BiddingDTO
            {
                ProjectId = 1,
                Proposal = "Test proposal",
                Duration = 10,
                Budget = 1000
            };

            var addedBid = new BidDTO // Simulate the added bid
            {
                Id = 1,
                UserId = userId,
                ProjectId = biddingDTO.ProjectId,
                Proposal = biddingDTO.Proposal,
                Duration = biddingDTO.Duration,
                Budget = biddingDTO.Budget
            };

            _mockBidService.Setup(service => service.Add(It.IsAny<BiddingDTO>()))
                           .ReturnsAsync(addedBid);

            // Act
            var result = await _bidController.Bidding(biddingDTO, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            
        }




        [Test]
        public async Task Bidding_NonExistingProject_ReturnsNotFound()
        {
            // Arrange
            var userId = 1;
            _mockCurrentUserService.Setup(service => service.UserId).Returns(userId); // Simulate logged in user

            var biddingDTO = new BiddingDTO
            {
                ProjectId = 999, // Assume ProjectId 999 does not exist
                Proposal = "Test proposal",
                Duration = 10,
                Budget = 1000
            };

            _mockProjectRepository.Setup(repo => repo.GetByIdAsync(biddingDTO.ProjectId))
                                  .ReturnsAsync((Project)null); // Simulate non-existing project

            // Act
            var result = await _bidController.Bidding(biddingDTO, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }





        #endregion
        #region Update Bidding
        [Test]
        public async Task UpdateBidding_ValidRequest_ReturnsOkWithUpdatedBid()
        {
            // Arrange
            var updateBidDTO = new UpdateBidDTO
            {
                Id = 1, // Assuming this Id exists in the repository
                Proposal = "Updated proposal",
                Duration = 15,
                Budget = 2000
            };

            var existingBid = new Bid
            {
                Id = 1,
                UserId = 1,
                ProjectId = 1,
                Proposal = "Original proposal",
                Duration = 10,
                Budget = 1000
            };

            // Mock repository to return the existing bid
            _mockBidRepository.Setup(repo => repo.GetByIdAsync(updateBidDTO.Id))
                  .ReturnsAsync(existingBid);

            //_mockBidService.Setup(service => service.Update(updateBidDTO))
            //               .ReturnsAsync(existingBid);
            // Simulate ModelState.IsValid as true
            _bidController.ModelState.Clear(); // Clear any previous ModelState errors

            // Act
            var result = await _bidController.UpdateBidding(updateBidDTO, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateBidding_BidNotFound_ReturnsNotFound()
        {
            // Arrange
            var updateBidDTO = new UpdateBidDTO
            {
                Id = 999, // Assuming this Id does not exist in the repository
                Proposal = "Updated proposal",
                Duration = 15,
                Budget = 2000
            };

            // Mock repository to return null for non-existing bid
            //_mockBidRepository.Setup(repo => repo.GetByIdAsync(updateBidDTO.Id))
            //                  .ReturnsAsync((BidDTO)null);

            // Simulate ModelState.IsValid as true
            _bidController.ModelState.Clear(); // Clear any previous ModelState errors

            // Act
            var result = await _bidController.UpdateBidding(updateBidDTO, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task UpdateBidding_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var updateBidDTO = new UpdateBidDTO(); // Invalid model state as Id is required

            // Simulate ModelState.IsValid as false
            _bidController.ModelState.AddModelError("Id", "Id is required");

            // Act
            var result = await _bidController.UpdateBidding(updateBidDTO, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        #endregion
        #region Accept Bid
        [Test]
        public async Task AcceptBidding_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var invalidDTO = new BidAccepted(); // Assume DTO is invalid

            _bidController.ModelState.AddModelError("Proposal", "Proposal is required"); // Simulate ModelState error

            // Act
            var result = await _bidController.AcceptBidding(invalidDTO, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task AcceptBidding_NonExistingBid_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 999; // Assume BidId 999 does not exist

            _mockBidRepository.Setup(repo => repo.GetByIdAsync(nonExistingId))
                              .ReturnsAsync((Bid)null); // Simulate non-existing bid

            var dto = new BidAccepted
            {
                Id = nonExistingId
            };

            // Act
            var result = await _bidController.AcceptBidding(dto, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }
        [Test]
        public async Task AcceptBidding_ValidRequest_ReturnsOkWithAcceptedBid()
        {
            // Arrange
            var existingBid = new Bid
            {
                Id = 1,
                UserId = 1,
                ProjectId = 1,
                Proposal = "Test proposal",
                Duration = 10,
                Budget = 1000
            };

            _mockBidRepository.Setup(repo => repo.GetByIdAsync(existingBid.Id))
                              .ReturnsAsync(existingBid); // Simulate existing bid

            

            var dto = new BidDTO
            {
                Id = 1,
                UserId = 1,
                ProjectId = 1,
                Proposal = "Test proposal",
                Duration = 10,
                Budget = 1000
            };

            var acc = new BidAccepted
            {
                Id = 1
            };

            _mockBidService.Setup(service => service.AcceptBidding(dto.Id))
                           .ReturnsAsync(dto); // Simulate acceptance of bid

            // Act
            var result = await _bidController.AcceptBidding(acc, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        #endregion

    }
}
