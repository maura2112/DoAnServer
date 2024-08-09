using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class FavoriteProjectTests
    {
        [Test]
        public void FavoriteProject_AppUserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var favoriteProject = new FavoriteProject();
            var expectedAppUserId = 1;

            // Act
            favoriteProject.AppUserId = expectedAppUserId;
            var actualAppUserId = favoriteProject.AppUserId;

            // Assert
            Assert.AreEqual(expectedAppUserId, actualAppUserId);
        }

        [Test]
        public void FavoriteProject_AppUser_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var favoriteProject = new FavoriteProject();
            var expectedAppUser = new AppUser { Name = "Test User" };

            // Act
            favoriteProject.AppUser = expectedAppUser;
            var actualAppUser = favoriteProject.AppUser;

            // Assert
            Assert.AreEqual(expectedAppUser, actualAppUser);
        }

        [Test]
        public void FavoriteProject_ProjectId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var favoriteProject = new FavoriteProject();
            var expectedProjectId = 1;

            // Act
            favoriteProject.ProjectId = expectedProjectId;
            var actualProjectId = favoriteProject.ProjectId;

            // Assert
            Assert.AreEqual(expectedProjectId, actualProjectId);
        }

        [Test]
        public void FavoriteProject_Project_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var favoriteProject = new FavoriteProject();
            var expectedProject = new Project { Title = "Test Project" };

            // Act
            favoriteProject.Project = expectedProject;
            var actualProject = favoriteProject.Project;

            // Assert
            Assert.AreEqual(expectedProject, actualProject);
        }

        [Test]
        public void FavoriteProject_SavedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var favoriteProject = new FavoriteProject();
            var expectedSavedDate = new DateTime(2024, 1, 1);

            // Act
            favoriteProject.SavedDate = expectedSavedDate;
            var actualSavedDate = favoriteProject.SavedDate;

            // Assert
            Assert.AreEqual(expectedSavedDate, actualSavedDate);
        }
    }
}
