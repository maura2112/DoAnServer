using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class CategoryTests
    {
        [Test]
        public void Category_Id_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var category = new Category();
            var expectedId = 1;

            // Act
            category.Id = expectedId;
            var actualId = category.Id;

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void Category_CategoryName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var category = new Category();
            var expectedCategoryName = "Web Development";

            // Act
            category.CategoryName = expectedCategoryName;
            var actualCategoryName = category.CategoryName;

            // Assert
            Assert.AreEqual(expectedCategoryName, actualCategoryName);
        }

        [Test]
        public void Category_Image_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var category = new Category();
            var expectedImage = "image.png";

            // Act
            category.Image = expectedImage;
            var actualImage = category.Image;

            // Assert
            Assert.AreEqual(expectedImage, actualImage);
        }

        [Test]
        public void Category_ShouldStartWithEmptyProjects()
        {
            // Arrange
            var category = new Category();

            // Act
            var projects = category.Projects;

            // Assert
            Assert.IsNotNull(projects);
            Assert.IsEmpty(projects);
        }

        [Test]
        public void Category_ShouldStartWithEmptySkills()
        {
            // Arrange
            var category = new Category();

            // Act
            var skills = category.Skills;

            // Assert
            Assert.IsNotNull(skills);
            Assert.IsEmpty(skills);
        }

        [Test]
        public void Category_ShouldAddProject()
        {
            // Arrange
            var category = new Category
            {
                Projects = new List<Project>()
            };
            var project = new Project { Title = "New Project" };

            // Act
            category.Projects.Add(project);

            // Assert
            Assert.Contains(project, (System.Collections.ICollection)category.Projects);
        }

        [Test]
        public void Category_ShouldAddSkill()
        {
            // Arrange
            var category = new Category
            {
                Skills = new List<Skill>()
            };
            var skill = new Skill { SkillName = "New Skill" };

            // Act
            category.Skills.Add(skill);

            // Assert
            Assert.Contains(skill, (System.Collections.ICollection)category.Skills);
        }
    }
}
