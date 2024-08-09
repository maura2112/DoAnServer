using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class SkillTests
    {
        [Test]
        public void Skill_SkillName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var skill = new Skill();
            var expectedSkillName = "C#";

            // Act
            skill.SkillName = expectedSkillName;
            var actualSkillName = skill.SkillName;

            // Assert
            Assert.AreEqual(expectedSkillName, actualSkillName);
        }

        [Test]
        public void Skill_CategoryId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var skill = new Skill();
            var expectedCategoryId = 1;

            // Act
            skill.CategoryId = expectedCategoryId;
            var actualCategoryId = skill.CategoryId;

            // Assert
            Assert.AreEqual(expectedCategoryId, actualCategoryId);
        }

        [Test]
        public void Skill_CreatedBy_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var skill = new Skill();
            var expectedCreatedBy = 2;

            // Act
            skill.CreatedBy = expectedCreatedBy;
            var actualCreatedBy = skill.CreatedBy;

            // Assert
            Assert.AreEqual(expectedCreatedBy, actualCreatedBy);
        }

        [Test]
        public void Skill_CreatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var skill = new Skill();
            var expectedCreatedDate = new DateTime(2024, 1, 1);

            // Act
            skill.CreatedDate = expectedCreatedDate;
            var actualCreatedDate = skill.CreatedDate;

            // Assert
            Assert.AreEqual(expectedCreatedDate, actualCreatedDate);
        }

        [Test]
        public void Skill_UpdatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var skill = new Skill();
            var expectedUpdatedDate = new DateTime(2024, 2, 1);

            // Act
            skill.UpdatedDate = expectedUpdatedDate;
            var actualUpdatedDate = skill.UpdatedDate;

            // Assert
            Assert.AreEqual(expectedUpdatedDate, actualUpdatedDate);
        }

        [Test]
        public void Skill_Category_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var skill = new Skill();
            var expectedCategory = new Category { CategoryName = "Programming" };

            // Act
            skill.Category = expectedCategory;
            var actualCategory = skill.Category;

            // Assert
            Assert.AreEqual(expectedCategory, actualCategory);
        }

        [Test]
        public void Skill_User_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var skill = new Skill();
            var expectedUser = new AppUser { Name = "TruongBQ" };

            // Act
            skill.User = expectedUser;
            var actualUser = skill.User;

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }

        [Test]
        public void Skill_ShouldStartWithEmptyProjectSkills()
        {
            // Arrange
            var skill = new Skill();

            // Act
            var projectSkills = skill.ProjectSkills;

            // Assert
            Assert.IsNotNull(projectSkills);
            Assert.IsEmpty(projectSkills);
        }

        [Test]
        public void Skill_ShouldStartWithEmptyUserSkills()
        {
            // Arrange
            var skill = new Skill();

            // Act
            var userSkills = skill.UserSkills;

            // Assert
            Assert.IsNotNull(userSkills);
            Assert.IsEmpty(userSkills);
        }

        [Test]
        public void Skill_ShouldAddProjectSkill()
        {
            // Arrange
            var skill = new Skill
            {
                ProjectSkills = new List<ProjectSkill>()
            };
            var projectSkill = new ProjectSkill { ProjectId = 1, SkillId = 1 };

            // Act
            skill.ProjectSkills.Add(projectSkill);

            // Assert
            Assert.Contains(projectSkill, (System.Collections.ICollection)skill.ProjectSkills);
        }

        [Test]
        public void Skill_ShouldAddUserSkill()
        {
            // Arrange
            var skill = new Skill
            {
                UserSkills = new List<UserSkill>()
            };
            var userSkill = new UserSkill { UserId = 1, SkillId = 1 };

            // Act
            skill.UserSkills.Add(userSkill);

            // Assert
            Assert.Contains(userSkill, (System.Collections.ICollection)skill.UserSkills);
        }

        [Test]
        public void Skill_CreatedBy_ShouldAllowNull()
        {
            // Arrange
            var skill = new Skill();

            // Act
            skill.CreatedBy = null;
            var actualCreatedBy = skill.CreatedBy;

            // Assert
            Assert.IsNull(actualCreatedBy);
        }

        [Test]
        public void Skill_CreatedDate_ShouldAllowNull()
        {
            // Arrange
            var skill = new Skill();

            // Act
            skill.CreatedDate = null;
            var actualCreatedDate = skill.CreatedDate;

            // Assert
            Assert.IsNull(actualCreatedDate);
        }

        [Test]
        public void Skill_UpdatedDate_ShouldAllowNull()
        {
            // Arrange
            var skill = new Skill();

            // Act
            skill.UpdatedDate = null;
            var actualUpdatedDate = skill.UpdatedDate;

            // Assert
            Assert.IsNull(actualUpdatedDate);
        }

        [Test]
        public void Skill_Category_ShouldAllowNull()
        {
            // Arrange
            var skill = new Skill();

            // Act
            skill.Category = null;
            var actualCategory = skill.Category;

            // Assert
            Assert.IsNull(actualCategory);
        }

        [Test]
        public void Skill_User_ShouldAllowNull()
        {
            // Arrange
            var skill = new Skill();

            // Act
            skill.User = null;
            var actualUser = skill.User;

            // Assert
            Assert.IsNull(actualUser);
        }
    }
}
