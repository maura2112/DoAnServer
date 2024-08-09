using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class BidTests
    {
        [Test]
        public void Bid_Id_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedId = 1L;

            // Act
            bid.Id = expectedId;
            var actualId = bid.Id;

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void Bid_ProjectId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedProjectId = 1;

            // Act
            bid.ProjectId = expectedProjectId;
            var actualProjectId = bid.ProjectId;

            // Assert
            Assert.AreEqual(expectedProjectId, actualProjectId);
        }

        [Test]
        public void Bid_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedUserId = 1;

            // Act
            bid.UserId = expectedUserId;
            var actualUserId = bid.UserId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [Test]
        public void Bid_Proposal_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedProposal = "This is a proposal.";

            // Act
            bid.Proposal = expectedProposal;
            var actualProposal = bid.Proposal;

            // Assert
            Assert.AreEqual(expectedProposal, actualProposal);
        }

        [Test]
        public void Bid_Duration_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedDuration = 30;

            // Act
            bid.Duration = expectedDuration;
            var actualDuration = bid.Duration;

            // Assert
            Assert.AreEqual(expectedDuration, actualDuration);
        }

        [Test]
        public void Bid_Budget_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedBudget = 5000;

            // Act
            bid.Budget = expectedBudget;
            var actualBudget = bid.Budget;

            // Assert
            Assert.AreEqual(expectedBudget, actualBudget);
        }

        [Test]
        public void Bid_IsCompleted_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedIsCompleted = true;

            // Act
            bid.IsCompleted = expectedIsCompleted;
            var actualIsCompleted = bid.IsCompleted;

            // Assert
            Assert.AreEqual(expectedIsCompleted, actualIsCompleted);
        }

        [Test]
        public void Bid_CreatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedCreatedDate = new DateTime(2024, 1, 1);

            // Act
            bid.CreatedDate = expectedCreatedDate;
            var actualCreatedDate = bid.CreatedDate;

            // Assert
            Assert.AreEqual(expectedCreatedDate, actualCreatedDate);
        }

        [Test]
        public void Bid_UpdatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedUpdatedDate = new DateTime(2024, 2, 1);

            // Act
            bid.UpdatedDate = expectedUpdatedDate;
            var actualUpdatedDate = bid.UpdatedDate;

            // Assert
            Assert.AreEqual(expectedUpdatedDate, actualUpdatedDate);
        }

        [Test]
        public void Bid_AcceptedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedAcceptedDate = new DateTime(2024, 3, 1);

            // Act
            bid.AcceptedDate = expectedAcceptedDate;
            var actualAcceptedDate = bid.AcceptedDate;

            // Assert
            Assert.AreEqual(expectedAcceptedDate, actualAcceptedDate);
        }

        [Test]
        public void Bid_TotalOfStage_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedTotalOfStage = 5;

            // Act
            bid.TotalOfStage = expectedTotalOfStage;
            var actualTotalOfStage = bid.TotalOfStage;

            // Assert
            Assert.AreEqual(expectedTotalOfStage, actualTotalOfStage);
        }

        

        [Test]
        public void Bid_Project_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedProject = new Project { Title = "Test Project" };

            // Act
            bid.Project = expectedProject;
            var actualProject = bid.Project;

            // Assert
            Assert.AreEqual(expectedProject, actualProject);
        }

        [Test]
        public void Bid_AppUser_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bid = new Bid();
            var expectedAppUser = new AppUser { Name = "TruongBQ" };

            // Act
            bid.AppUser = expectedAppUser;
            var actualAppUser = bid.AppUser;

            // Assert
            Assert.AreEqual(expectedAppUser, actualAppUser);
        }

        [Test]
        public void Bid_UserId_ShouldAllowNull()
        {
            // Arrange
            var bid = new Bid();

            // Act
            bid.UserId = null;
            var actualUserId = bid.UserId;

            // Assert
            Assert.IsNull(actualUserId);
        }

        [Test]
        public void Bid_IsCompleted_ShouldAllowNull()
        {
            // Arrange
            var bid = new Bid();

            // Act
            bid.IsCompleted = null;
            var actualIsCompleted = bid.IsCompleted;

            // Assert
            Assert.IsNull(actualIsCompleted);
        }

        [Test]
        public void Bid_UpdatedDate_ShouldAllowNull()
        {
            // Arrange
            var bid = new Bid();

            // Act
            bid.UpdatedDate = null;
            var actualUpdatedDate = bid.UpdatedDate;

            // Assert
            Assert.IsNull(actualUpdatedDate);
        }

        [Test]
        public void Bid_AcceptedDate_ShouldAllowNull()
        {
            // Arrange
            var bid = new Bid();

            // Act
            bid.AcceptedDate = null;
            var actualAcceptedDate = bid.AcceptedDate;

            // Assert
            Assert.IsNull(actualAcceptedDate);
        }

        [Test]
        public void Bid_TotalOfStage_ShouldAllowNull()
        {
            // Arrange
            var bid = new Bid();

            // Act
            bid.TotalOfStage = null;
            var actualTotalOfStage = bid.TotalOfStage;

            // Assert
            Assert.IsNull(actualTotalOfStage);
        }

        [Test]
        public void Bid_BidStages_ShouldAllowNull()
        {
            // Arrange
            var bid = new Bid();

            // Act
            bid.BidStages = null;
            var actualBidStages = bid.BidStages;

            // Assert
            Assert.IsNull(actualBidStages);
        }

        [Test]
        public void Bid_Project_ShouldAllowNull()
        {
            // Arrange
            var bid = new Bid();

            // Act
            bid.Project = null;
            var actualProject = bid.Project;

            // Assert
            Assert.IsNull(actualProject);
        }

        [Test]
        public void Bid_AppUser_ShouldAllowNull()
        {
            // Arrange
            var bid = new Bid();

            // Act
            bid.AppUser = null;
            var actualAppUser = bid.AppUser;

            // Assert
            Assert.IsNull(actualAppUser);
        }
    }
}
