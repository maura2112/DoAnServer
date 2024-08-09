using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class UserProjectTests
    {
        [Test]
        public void UserProject_Id_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userProject = new UserProject();
            var expectedId = 123L;

            // Act
            userProject.Id = expectedId;
            var actualId = userProject.Id;

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void UserProject_ProjectId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userProject = new UserProject();
            var expectedProjectId = 1;

            // Act
            userProject.ProjectId = expectedProjectId;
            var actualProjectId = userProject.ProjectId;

            // Assert
            Assert.AreEqual(expectedProjectId, actualProjectId);
        }

        [Test]
        public void UserProject_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userProject = new UserProject();
            var expectedUserId = 2;

            // Act
            userProject.UserId = expectedUserId;
            var actualUserId = userProject.UserId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [Test]
        public void UserProject_Project_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userProject = new UserProject();
            var expectedProject = new Project { Title = "Test Project" };

            // Act
            userProject.Project = expectedProject;
            var actualProject = userProject.Project;

            // Assert
            Assert.AreEqual(expectedProject, actualProject);
        }

        [Test]
        public void UserProject_AppUser_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userProject = new UserProject();
            var expectedAppUser = new AppUser { Name = "Test User" };

            // Act
            userProject.AppUser = expectedAppUser;
            var actualAppUser = userProject.AppUser;

            // Assert
            Assert.AreEqual(expectedAppUser, actualAppUser);
        }

        [Test]
        public void UserProject_Project_ShouldAllowNull()
        {
            // Arrange
            var userProject = new UserProject();

            // Act
            userProject.Project = null;
            var actualProject = userProject.Project;

            // Assert
            Assert.IsNull(actualProject);
        }

        [Test]
        public void UserProject_AppUser_ShouldAllowNull()
        {
            // Arrange
            var userProject = new UserProject();

            // Act
            userProject.AppUser = null;
            var actualAppUser = userProject.AppUser;

            // Assert
            Assert.IsNull(actualAppUser);
        }
    }
}
