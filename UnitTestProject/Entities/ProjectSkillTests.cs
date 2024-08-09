using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class ProjectSkillTests
    {
        [Test]
        public void ProjectSkill_ProjectId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var projectSkill = new ProjectSkill();
            var expectedProjectId = 1;

            // Act
            projectSkill.ProjectId = expectedProjectId;
            var actualProjectId = projectSkill.ProjectId;

            // Assert
            Assert.AreEqual(expectedProjectId, actualProjectId);
        }

        [Test]
        public void ProjectSkill_SkillId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var projectSkill = new ProjectSkill();
            var expectedSkillId = 2;

            // Act
            projectSkill.SkillId = expectedSkillId;
            var actualSkillId = projectSkill.SkillId;

            // Assert
            Assert.AreEqual(expectedSkillId, actualSkillId);
        }

        [Test]
        public void ProjectSkill_Skill_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var projectSkill = new ProjectSkill();
            var expectedSkill = new Skill { SkillName = "Test Skill" };

            // Act
            projectSkill.Skill = expectedSkill;
            var actualSkill = projectSkill.Skill;

            // Assert
            Assert.AreEqual(expectedSkill, actualSkill);
        }

        [Test]
        public void ProjectSkill_Project_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var projectSkill = new ProjectSkill();
            var expectedProject = new Project { Title = "Test Project" };

            // Act
            projectSkill.Project = expectedProject;
            var actualProject = projectSkill.Project;

            // Assert
            Assert.AreEqual(expectedProject, actualProject);
        }

        [Test]
        public void ProjectSkill_Skill_ShouldAllowNull()
        {
            // Arrange
            var projectSkill = new ProjectSkill();

            // Act
            projectSkill.Skill = null;
            var actualSkill = projectSkill.Skill;

            // Assert
            Assert.IsNull(actualSkill);
        }

        [Test]
        public void ProjectSkill_Project_ShouldAllowNull()
        {
            // Arrange
            var projectSkill = new ProjectSkill();

            // Act
            projectSkill.Project = null;
            var actualProject = projectSkill.Project;

            // Assert
            Assert.IsNull(actualProject);
        }
    }
}
