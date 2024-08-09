using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class UserReportTests
    {
        [Test]
        public void UserReport_CreatedBy_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedCreatedBy = 1;

            // Act
            userReport.CreatedBy = expectedCreatedBy;
            var actualCreatedBy = userReport.CreatedBy;

            // Assert
            Assert.AreEqual(expectedCreatedBy, actualCreatedBy);
        }

        [Test]
        public void UserReport_ReportCategoryId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedReportCategoryId = 2;

            // Act
            userReport.ReportCategoryId = expectedReportCategoryId;
            var actualReportCategoryId = userReport.ReportCategoryId;

            // Assert
            Assert.AreEqual(expectedReportCategoryId, actualReportCategoryId);
        }

        [Test]
        public void UserReport_ReportToUrl_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedReportToUrl = "http://example.com";

            // Act
            userReport.ReportToUrl = expectedReportToUrl;
            var actualReportToUrl = userReport.ReportToUrl;

            // Assert
            Assert.AreEqual(expectedReportToUrl, actualReportToUrl);
        }

        [Test]
        public void UserReport_ProjectId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedProjectId = 3;

            // Act
            userReport.ProjectId = expectedProjectId;
            var actualProjectId = userReport.ProjectId;

            // Assert
            Assert.AreEqual(expectedProjectId, actualProjectId);
        }

        [Test]
        public void UserReport_BidId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedBidId = 4L;

            // Act
            userReport.BidId = expectedBidId;
            var actualBidId = userReport.BidId;

            // Assert
            Assert.AreEqual(expectedBidId, actualBidId);
        }

        [Test]
        public void UserReport_UserReportedId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedUserReportedId = 5;

            // Act
            userReport.UserReportedId = expectedUserReportedId;
            var actualUserReportedId = userReport.UserReportedId;

            // Assert
            Assert.AreEqual(expectedUserReportedId, actualUserReportedId);
        }

        [Test]
        public void UserReport_IsApproved_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedIsApproved = true;

            // Act
            userReport.IsApproved = expectedIsApproved;
            var actualIsApproved = userReport.IsApproved;

            // Assert
            Assert.AreEqual(expectedIsApproved, actualIsApproved);
        }

        [Test]
        public void UserReport_IsRejected_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedIsRejected = true;

            // Act
            userReport.IsRejected = expectedIsRejected;
            var actualIsRejected = userReport.IsRejected;

            // Assert
            Assert.AreEqual(expectedIsRejected, actualIsRejected);
        }

        [Test]
        public void UserReport_Description_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedDescription = "Test Description";

            // Act
            userReport.Description = expectedDescription;
            var actualDescription = userReport.Description;

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void UserReport_CreatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedCreatedDate = new DateTime(2024, 1, 1);

            // Act
            userReport.CreatedDate = expectedCreatedDate;
            var actualCreatedDate = userReport.CreatedDate;

            // Assert
            Assert.AreEqual(expectedCreatedDate, actualCreatedDate);
        }

        [Test]
        public void UserReport_UpdatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedUpdatedDate = new DateTime(2024, 2, 1);

            // Act
            userReport.UpdatedDate = expectedUpdatedDate;
            var actualUpdatedDate = userReport.UpdatedDate;

            // Assert
            Assert.AreEqual(expectedUpdatedDate, actualUpdatedDate);
        }

        [Test]
        public void UserReport_UpdatedDate_ShouldAllowNull()
        {
            // Arrange
            var userReport = new UserReport();

            // Act
            userReport.UpdatedDate = null;
            var actualUpdatedDate = userReport.UpdatedDate;

            // Assert
            Assert.IsNull(actualUpdatedDate);
        }

        [Test]
        public void UserReport_User_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedUser = new AppUser { Name = "Test User" };

            // Act
            userReport.User = expectedUser;
            var actualUser = userReport.User;

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }

        [Test]
        public void UserReport_ReportCategory_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedReportCategory = new ReportCategory { Name = "Test Category" };

            // Act
            userReport.ReportCategory = expectedReportCategory;
            var actualReportCategory = userReport.ReportCategory;

            // Assert
            Assert.AreEqual(expectedReportCategory, actualReportCategory);
        }

        [Test]
        public void UserReport_UserReported_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userReport = new UserReport();
            var expectedUserReported = new AppUser { Name = "Reported User" };

            // Act
            userReport.UserReported = expectedUserReported;
            var actualUserReported = userReport.UserReported;

            // Assert
            Assert.AreEqual(expectedUserReported, actualUserReported);
        }

        [Test]
        public void UserReport_ReportToUrl_ShouldAllowNull()
        {
            // Arrange
            var userReport = new UserReport();

            // Act
            userReport.ReportToUrl = null;
            var actualReportToUrl = userReport.ReportToUrl;

            // Assert
            Assert.IsNull(actualReportToUrl);
        }

        [Test]
        public void UserReport_ProjectId_ShouldAllowNull()
        {
            // Arrange
            var userReport = new UserReport();

            // Act
            userReport.ProjectId = null;
            var actualProjectId = userReport.ProjectId;

            // Assert
            Assert.IsNull(actualProjectId);
        }

        [Test]
        public void UserReport_BidId_ShouldAllowNull()
        {
            // Arrange
            var userReport = new UserReport();

            // Act
            userReport.BidId = null;
            var actualBidId = userReport.BidId;

            // Assert
            Assert.IsNull(actualBidId);
        }

        [Test]
        public void UserReport_UserReportedId_ShouldAllowNull()
        {
            // Arrange
            var userReport = new UserReport();

            // Act
            userReport.UserReportedId = null;
            var actualUserReportedId = userReport.UserReportedId;

            // Assert
            Assert.IsNull(actualUserReportedId);
        }
    }
}
