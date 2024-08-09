using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class BookmarkTests
    {
        [Test]
        public void Bookmark_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bookmark = new Bookmark();
            var expectedUserId = 1;

            // Act
            bookmark.UserId = expectedUserId;
            var actualUserId = bookmark.UserId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [Test]
        public void Bookmark_ProjectId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bookmark = new Bookmark();
            var expectedProjectId = 1;

            // Act
            bookmark.ProjectId = expectedProjectId;
            var actualProjectId = bookmark.ProjectId;

            // Assert
            Assert.AreEqual(expectedProjectId, actualProjectId);
        }

        [Test]
        public void Bookmark_SavedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bookmark = new Bookmark();
            var expectedSavedDate = new DateTime(2024, 1, 1);

            // Act
            bookmark.SavedDate = expectedSavedDate;
            var actualSavedDate = bookmark.SavedDate;

            // Assert
            Assert.AreEqual(expectedSavedDate, actualSavedDate);
        }

        [Test]
        public void Bookmark_AppUser_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bookmark = new Bookmark();
            var expectedAppUser = new AppUser { Name = "Test User" };

            // Act
            bookmark.AppUser = expectedAppUser;
            var actualAppUser = bookmark.AppUser;

            // Assert
            Assert.AreEqual(expectedAppUser, actualAppUser);
        }

        [Test]
        public void Bookmark_Project_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var bookmark = new Bookmark();
            var expectedProject = new Project { Title = "Test Project" };

            // Act
            bookmark.Project = expectedProject;
            var actualProject = bookmark.Project;

            // Assert
            Assert.AreEqual(expectedProject, actualProject);
        }
    }
}
