using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class ProjectStatusTests
    {
        [Test]
        public void ProjectStatus_StatusName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var projectStatus = new ProjectStatus();
            var expectedStatusName = "InProgress";

            // Act
            projectStatus.StatusName = expectedStatusName;
            var actualStatusName = projectStatus.StatusName;

            // Assert
            Assert.AreEqual(expectedStatusName, actualStatusName);
        }

        [Test]
        public void ProjectStatus_StatusColor_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var projectStatus = new ProjectStatus();
            var expectedStatusColor = "#FF5733";

            // Act
            projectStatus.StatusColor = expectedStatusColor;
            var actualStatusColor = projectStatus.StatusColor;

            // Assert
            Assert.AreEqual(expectedStatusColor, actualStatusColor);
        }

        [Test]
        public void ProjectStatus_ShouldStartWithEmptyProjects()
        {
            // Arrange
            var projectStatus = new ProjectStatus();

            // Act
            var projects = projectStatus.Projects;

            // Assert
            Assert.IsNotNull(projects);
            Assert.IsEmpty(projects);
        }

        [Test]
        public void ProjectStatus_ShouldAddProject()
        {
            // Arrange
            var projectStatus = new ProjectStatus();
            var project = new Project { Title = "New Project" };

            // Act
            projectStatus.Projects.Add(project);

            // Assert
            Assert.Contains(project, (System.Collections.ICollection)projectStatus.Projects);
        }

        [Test]
        public void ProjectStatus_ShouldRemoveProject()
        {
            // Arrange
            var projectStatus = new ProjectStatus();
            var project = new Project { Title = "New Project" };
            projectStatus.Projects.Add(project);

            // Act
            projectStatus.Projects.Remove(project);

            // Assert
            Assert.IsFalse(projectStatus.Projects.Contains(project));
        }
    }
}
