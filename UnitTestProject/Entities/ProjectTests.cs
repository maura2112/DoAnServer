using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public void Project_Title_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedTitle = "Test Project";

            // Act
            project.Title = expectedTitle;
            var actualTitle = project.Title;

            // Assert
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [Test]
        public void Project_CategoryId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedCategoryId = 1;

            // Act
            project.CategoryId = expectedCategoryId;
            var actualCategoryId = project.CategoryId;

            // Assert
            Assert.AreEqual(expectedCategoryId, actualCategoryId);
        }

        [Test]
        public void Project_MinBudget_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedMinBudget = 1000;

            // Act
            project.MinBudget = expectedMinBudget;
            var actualMinBudget = project.MinBudget;

            // Assert
            Assert.AreEqual(expectedMinBudget, actualMinBudget);
        }

        [Test]
        public void Project_MaxBudget_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedMaxBudget = 5000;

            // Act
            project.MaxBudget = expectedMaxBudget;
            var actualMaxBudget = project.MaxBudget;

            // Assert
            Assert.AreEqual(expectedMaxBudget, actualMaxBudget);
        }

        [Test]
        public void Project_IsCompleted_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedIsCompleted = true;

            // Act
            project.IsCompleted = expectedIsCompleted;
            var actualIsCompleted = project.IsCompleted;

            // Assert
            Assert.AreEqual(expectedIsCompleted, actualIsCompleted);
        }

        [Test]
        public void Project_Duration_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedDuration = 30;

            // Act
            project.Duration = expectedDuration;
            var actualDuration = project.Duration;

            // Assert
            Assert.AreEqual(expectedDuration, actualDuration);
        }

        [Test]
        public void Project_Description_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedDescription = "Project Description";

            // Act
            project.Description = expectedDescription;
            var actualDescription = project.Description;

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void Project_CreatedBy_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedCreatedBy = 1;

            // Act
            project.CreatedBy = expectedCreatedBy;
            var actualCreatedBy = project.CreatedBy;

            // Assert
            Assert.AreEqual(expectedCreatedBy, actualCreatedBy);
        }

        [Test]
        public void Project_CreatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedCreatedDate = new DateTime(2024, 1, 1);

            // Act
            project.CreatedDate = expectedCreatedDate;
            var actualCreatedDate = project.CreatedDate;

            // Assert
            Assert.AreEqual(expectedCreatedDate, actualCreatedDate);
        }

        [Test]
        public void Project_UpdatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedUpdatedDate = new DateTime(2024, 2, 1);

            // Act
            project.UpdatedDate = expectedUpdatedDate;
            var actualUpdatedDate = project.UpdatedDate;

            // Assert
            Assert.AreEqual(expectedUpdatedDate, actualUpdatedDate);
        }

        [Test]
        public void Project_EstimateStartDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedEstimateStartDate = new DateTime(2024, 3, 1);

            // Act
            project.EstimateStartDate = expectedEstimateStartDate;
            var actualEstimateStartDate = project.EstimateStartDate;

            // Assert
            Assert.AreEqual(expectedEstimateStartDate, actualEstimateStartDate);
        }

        [Test]
        public void Project_StatusId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedStatusId = 1;

            // Act
            project.StatusId = expectedStatusId;
            var actualStatusId = project.StatusId;

            // Assert
            Assert.AreEqual(expectedStatusId, actualStatusId);
        }

        [Test]
        public void Project_MediaFileId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedMediaFileId = 1L;

            // Act
            project.MediaFileId = expectedMediaFileId;
            var actualMediaFileId = project.MediaFileId;

            // Assert
            Assert.AreEqual(expectedMediaFileId, actualMediaFileId);
        }

        [Test]
        public void Project_RejectReason_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedRejectReason = "Some reason";

            // Act
            project.RejectReason = expectedRejectReason;
            var actualRejectReason = project.RejectReason;

            // Assert
            Assert.AreEqual(expectedRejectReason, actualRejectReason);
        }

        [Test]
        public void Project_RejectTimes_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedRejectTimes = 2;

            // Act
            project.RejectTimes = expectedRejectTimes;
            var actualRejectTimes = project.RejectTimes;

            // Assert
            Assert.AreEqual(expectedRejectTimes, actualRejectTimes);
        }

        [Test]
        public void Project_AppUser_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedAppUser = new AppUser { Name = "TruongBQ" };

            // Act
            project.AppUser = expectedAppUser;
            var actualAppUser = project.AppUser;

            // Assert
            Assert.AreEqual(expectedAppUser, actualAppUser);
        }

        [Test]
        public void Project_Category_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedCategory = new Category { CategoryName = "Programming" };

            // Act
            project.Category = expectedCategory;
            var actualCategory = project.Category;

            // Assert
            Assert.AreEqual(expectedCategory, actualCategory);
        }

        [Test]
        public void Project_MediaFile_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedMediaFile = new MediaFile { FileName = "file.png" };

            // Act
            project.MediaFile = expectedMediaFile;
            var actualMediaFile = project.MediaFile;

            // Assert
            Assert.AreEqual(expectedMediaFile, actualMediaFile);
        }

        [Test]
        public void Project_ProjectStatus_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var project = new Project();
            var expectedProjectStatus = new ProjectStatus { StatusName = "InProgress" };

            // Act
            project.ProjectStatus = expectedProjectStatus;
            var actualProjectStatus = project.ProjectStatus;

            // Assert
            Assert.AreEqual(expectedProjectStatus, actualProjectStatus);
        }

        [Test]
        public void Project_ShouldStartWithEmptyBids()
        {
            // Arrange
            var project = new Project();

            // Act
            var bids = project.Bids;

            // Assert
            Assert.IsNotNull(bids);
            Assert.IsEmpty(bids);
        }

        [Test]
        public void Project_ShouldStartWithEmptyBookmarks()
        {
            // Arrange
            var project = new Project();

            // Act
            var bookmarks = project.Bookmarks;

            // Assert
            Assert.IsNotNull(bookmarks);
            Assert.IsEmpty(bookmarks);
        }

        [Test]
        public void Project_ShouldStartWithEmptyUserProjects()
        {
            // Arrange
            var project = new Project();

            // Act
            var userProjects = project.UserProjects;

            // Assert
            Assert.IsNotNull(userProjects);
            Assert.IsEmpty(userProjects);
        }

        [Test]
        public void Project_ShouldStartWithEmptyProjectSkills()
        {
            // Arrange
            var project = new Project();

            // Act
            var projectSkills = project.ProjectSkills;

            // Assert
            Assert.IsNotNull(projectSkills);
            Assert.IsEmpty(projectSkills);
        }

        [Test]
        public void Project_ShouldStartWithEmptyUserReports()
        {
            // Arrange
            var project = new Project();

            // Act
            var userReports = project.UserReports;

            // Assert
            Assert.IsNotNull(userReports);
            Assert.IsEmpty(userReports);
        }

        [Test]
        public void Project_ShouldStartWithEmptyFavoriteProjects()
        {
            // Arrange
            var project = new Project();

            // Act
            var favoriteProjects = project.FavoriteProjects;

            // Assert
            Assert.IsNotNull(favoriteProjects);
            Assert.IsEmpty(favoriteProjects);
        }

        [Test]
        public void Project_IsCompleted_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.IsCompleted = null;
            var actualIsCompleted = project.IsCompleted;

            // Assert
            Assert.IsNull(actualIsCompleted);
        }

        [Test]
        public void Project_CreatedBy_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.CreatedBy = null;
            var actualCreatedBy = project.CreatedBy;

            // Assert
            Assert.IsNull(actualCreatedBy);
        }

        [Test]
        public void Project_UpdatedDate_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.UpdatedDate = null;
            var actualUpdatedDate = project.UpdatedDate;

            // Assert
            Assert.IsNull(actualUpdatedDate);
        }

        [Test]
        public void Project_EstimateStartDate_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.EstimateStartDate = null;
            var actualEstimateStartDate = project.EstimateStartDate;

            // Assert
            Assert.IsNull(actualEstimateStartDate);
        }

        [Test]
        public void Project_MediaFileId_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.MediaFileId = null;
            var actualMediaFileId = project.MediaFileId;

            // Assert
            Assert.IsNull(actualMediaFileId);
        }

        [Test]
        public void Project_RejectReason_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.RejectReason = null;
            var actualRejectReason = project.RejectReason;

            // Assert
            Assert.IsNull(actualRejectReason);
        }

        [Test]
        public void Project_AppUser_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.AppUser = null;
            var actualAppUser = project.AppUser;

            // Assert
            Assert.IsNull(actualAppUser);
        }

        [Test]
        public void Project_Category_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.Category = null;
            var actualCategory = project.Category;

            // Assert
            Assert.IsNull(actualCategory);
        }

        [Test]
        public void Project_MediaFile_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.MediaFile = null;
            var actualMediaFile = project.MediaFile;

            // Assert
            Assert.IsNull(actualMediaFile);
        }

        [Test]
        public void Project_ProjectStatus_ShouldAllowNull()
        {
            // Arrange
            var project = new Project();

            // Act
            project.ProjectStatus = null;
            var actualProjectStatus = project.ProjectStatus;

            // Assert
            Assert.IsNull(actualProjectStatus);
        }
    }
}
