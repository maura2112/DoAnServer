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

        [SetUp]
        public void Setup()
        {
            _mockBidService = new Mock<IBidService>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockBidRepository = new Mock<IBidRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();

            _bidController = new BidController(
                _mockBidService.Object,
                _mockBidRepository.Object,
                _mockCurrentUserService.Object,
                _mockProjectRepository.Object
            );
        }
        #region Get List Bid By UserId
        [Test]
        public async Task GetListByUserId_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _bidController.ModelState.AddModelError("key", "error message");

            // Act
            var result = await _bidController.GetListByUserId(new BidSearchDTO());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }




        [Test]
        public async Task GetListByUserId_ValidRequest_ReturnsOkWithBids()
        {
            // Arrange
            var userId = 1;
            var bids = new List<Bid>
        {
            new Bid { Id = 1, UserId = userId, ProjectId = 1, Proposal = "Proposal 1", Duration = 10, Budget = 1000, CreatedDate = DateTime.UtcNow }
            // Add more sample bids as needed
        };

            _mockBidService.Setup(service => service.GetWithFilter(It.IsAny<Expression<Func<Bid, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new Pagination<BidDTO> { Items = bids.Select(b => new BidDTO { Id = b.Id }).ToList() });

            // Act
            var result = await _bidController.GetListByUserId(new BidSearchDTO { UserId = userId, PageIndex = 1, PageSize = 10 });

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var bidsDto = okResult.Value as Pagination<BidDTO>;
            Assert.IsNotNull(bidsDto);
            Assert.AreEqual(bids.Count, bidsDto.Items.Count);
        }






        [Test]
        public async Task GetListByUserId_ServiceError_ReturnsInternalServerError()
        {
            // Arrange
            _mockBidService.Setup(service => service.GetWithFilter(It.IsAny<Expression<Func<Domain.Entities.Bid, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ThrowsAsync(new Exception("Service exception"));

            // Act
            var result = await _bidController.GetListByUserId(new BidSearchDTO());

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, (result as ObjectResult).StatusCode);
        }
        #endregion
        #region Get List Bid By Project Id
        [Test]
        public async Task GetListByProjectId_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _bidController.ModelState.AddModelError("key", "error message");

            // Act
            var result = await _bidController.GetListByProjectId(new BidListDTO());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetListByProjectId_ValidRequest_ReturnsOkWithBids()
        {
            // Arrange
            var projectId = 1;
            var bids = new List<Bid>
        {
            new Bid { Id = 1, ProjectId = projectId, UserId = 1, Proposal = "Proposal 1", Duration = 10, Budget = 1000, CreatedDate = DateTime.UtcNow }
            // Add more sample bids as needed
        };

            _mockBidService.Setup(service => service.GetWithFilter(It.IsAny<Expression<Func<Bid, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new Pagination<BidDTO> { Items = bids.Select(b => new BidDTO { Id = b.Id }).ToList() });

            // Act
            var result = await _bidController.GetListByProjectId(new BidListDTO { ProjectId = projectId, PageIndex = 1, PageSize = 10 });

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var bidsDto = okResult.Value as Pagination<BidDTO>;
            Assert.IsNotNull(bidsDto);
            Assert.AreEqual(bids.Count, bidsDto.Items.Count);
        }

        [Test]
        public async Task GetListByProjectId_ValidRequest_NoBids_ReturnsEmptyList()
        {
            // Arrange
            var projectId = 2;
            var emptyBids = new List<Bid>();

            _mockBidService.Setup(service => service.GetWithFilter(It.IsAny<Expression<Func<Bid, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new Pagination<BidDTO> { Items = emptyBids.Select(b => new BidDTO { Id = b.Id }).ToList() });

            // Act
            var result = await _bidController.GetListByProjectId(new BidListDTO { ProjectId = projectId, PageIndex = 1, PageSize = 10 });

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var bidsDto = okResult.Value as Pagination<BidDTO>;
            Assert.IsNotNull(bidsDto);
            Assert.AreEqual(0, bidsDto.Items.Count);
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
