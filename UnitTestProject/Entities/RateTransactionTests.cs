using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class RateTransactionTests
    {
        [Test]
        public void RateTransaction_ProjectUserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedProjectUserId = 1;

            // Act
            rateTransaction.ProjectUserId = expectedProjectUserId;
            var actualProjectUserId = rateTransaction.ProjectUserId;

            // Assert
            Assert.AreEqual(expectedProjectUserId, actualProjectUserId);
        }

        [Test]
        public void RateTransaction_ProjectId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedProjectId = 2;

            // Act
            rateTransaction.ProjectId = expectedProjectId;
            var actualProjectId = rateTransaction.ProjectId;

            // Assert
            Assert.AreEqual(expectedProjectId, actualProjectId);
        }

        [Test]
        public void RateTransaction_ProjectAcceptedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedProjectAcceptedDate = new DateTime(2024, 1, 1);

            // Act
            rateTransaction.ProjectAcceptedDate = expectedProjectAcceptedDate;
            var actualProjectAcceptedDate = rateTransaction.ProjectAcceptedDate;

            // Assert
            Assert.AreEqual(expectedProjectAcceptedDate, actualProjectAcceptedDate);
        }

        [Test]
        public void RateTransaction_BidUserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedBidUserId = 3;

            // Act
            rateTransaction.BidUserId = expectedBidUserId;
            var actualBidUserId = rateTransaction.BidUserId;

            // Assert
            Assert.AreEqual(expectedBidUserId, actualBidUserId);
        }

        [Test]
        public void RateTransaction_BidCompletedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedBidCompletedDate = new DateTime(2024, 2, 1);

            // Act
            rateTransaction.BidCompletedDate = expectedBidCompletedDate;
            var actualBidCompletedDate = rateTransaction.BidCompletedDate;

            // Assert
            Assert.AreEqual(expectedBidCompletedDate, actualBidCompletedDate);
        }

        [Test]
        public void RateTransaction_User1IdRated_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedUser1IdRated = 4;

            // Act
            rateTransaction.User1IdRated = expectedUser1IdRated;
            var actualUser1IdRated = rateTransaction.User1IdRated;

            // Assert
            Assert.AreEqual(expectedUser1IdRated, actualUser1IdRated);
        }

        [Test]
        public void RateTransaction_User2IdRated_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedUser2IdRated = 5;

            // Act
            rateTransaction.User2IdRated = expectedUser2IdRated;
            var actualUser2IdRated = rateTransaction.User2IdRated;

            // Assert
            Assert.AreEqual(expectedUser2IdRated, actualUser2IdRated);
        }

        [Test]
        public void RateTransaction_UserProject_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedUserProject = new AppUser { Name = "Test User Project" };

            // Act
            rateTransaction.UserProject = expectedUserProject;
            var actualUserProject = rateTransaction.UserProject;

            // Assert
            Assert.AreEqual(expectedUserProject, actualUserProject);
        }

        [Test]
        public void RateTransaction_UserBid_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rateTransaction = new RateTransaction();
            var expectedUserBid = new AppUser { Name = "Test User Bid" };

            // Act
            rateTransaction.UserBid = expectedUserBid;
            var actualUserBid = rateTransaction.UserBid;

            // Assert
            Assert.AreEqual(expectedUserBid, actualUserBid);
        }

        [Test]
        public void RateTransaction_ProjectUserId_ShouldAllowNull()
        {
            // Arrange
            var rateTransaction = new RateTransaction();

            // Act
            rateTransaction.ProjectUserId = null;
            var actualProjectUserId = rateTransaction.ProjectUserId;

            // Assert
            Assert.IsNull(actualProjectUserId);
        }

        [Test]
        public void RateTransaction_ProjectId_ShouldAllowNull()
        {
            // Arrange
            var rateTransaction = new RateTransaction();

            // Act
            rateTransaction.ProjectId = null;
            var actualProjectId = rateTransaction.ProjectId;

            // Assert
            Assert.IsNull(actualProjectId);
        }

        [Test]
        public void RateTransaction_ProjectAcceptedDate_ShouldAllowNull()
        {
            // Arrange
            var rateTransaction = new RateTransaction();

            // Act
            rateTransaction.ProjectAcceptedDate = null;
            var actualProjectAcceptedDate = rateTransaction.ProjectAcceptedDate;

            // Assert
            Assert.IsNull(actualProjectAcceptedDate);
        }

        [Test]
        public void RateTransaction_BidUserId_ShouldAllowNull()
        {
            // Arrange
            var rateTransaction = new RateTransaction();

            // Act
            rateTransaction.BidUserId = null;
            var actualBidUserId = rateTransaction.BidUserId;

            // Assert
            Assert.IsNull(actualBidUserId);
        }

        [Test]
        public void RateTransaction_BidCompletedDate_ShouldAllowNull()
        {
            // Arrange
            var rateTransaction = new RateTransaction();

            // Act
            rateTransaction.BidCompletedDate = null;
            var actualBidCompletedDate = rateTransaction.BidCompletedDate;

            // Assert
            Assert.IsNull(actualBidCompletedDate);
        }

        [Test]
        public void RateTransaction_UserProject_ShouldAllowNull()
        {
            // Arrange
            var rateTransaction = new RateTransaction();

            // Act
            rateTransaction.UserProject = null;
            var actualUserProject = rateTransaction.UserProject;

            // Assert
            Assert.IsNull(actualUserProject);
        }

        [Test]
        public void RateTransaction_UserBid_ShouldAllowNull()
        {
            // Arrange
            var rateTransaction = new RateTransaction();

            // Act
            rateTransaction.UserBid = null;
            var actualUserBid = rateTransaction.UserBid;

            // Assert
            Assert.IsNull(actualUserBid);
        }
    }
}
