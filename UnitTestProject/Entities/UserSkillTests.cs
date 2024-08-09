using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class UserSkillTests
    {
        [Test]
        public void UserSkill_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userSkill = new UserSkill();
            var expectedUserId = 1;

            // Act
            userSkill.UserId = expectedUserId;
            var actualUserId = userSkill.UserId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [Test]
        public void UserSkill_SkillId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userSkill = new UserSkill();
            var expectedSkillId = 2;

            // Act
            userSkill.SkillId = expectedSkillId;
            var actualSkillId = userSkill.SkillId;

            // Assert
            Assert.AreEqual(expectedSkillId, actualSkillId);
        }

        [Test]
        public void UserSkill_Skill_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userSkill = new UserSkill();
            var expectedSkill = new Skill { SkillName = "Test Skill" };

            // Act
            userSkill.Skill = expectedSkill;
            var actualSkill = userSkill.Skill;

            // Assert
            Assert.AreEqual(expectedSkill, actualSkill);
        }

        [Test]
        public void UserSkill_AppUser_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var userSkill = new UserSkill();
            var expectedAppUser = new AppUser { Name = "Test User" };

            // Act
            userSkill.AppUser = expectedAppUser;
            var actualAppUser = userSkill.AppUser;

            // Assert
            Assert.AreEqual(expectedAppUser, actualAppUser);
        }

        [Test]
        public void UserSkill_Skill_ShouldAllowNull()
        {
            // Arrange
            var userSkill = new UserSkill();

            // Act
            userSkill.Skill = null;
            var actualSkill = userSkill.Skill;

            // Assert
            Assert.IsNull(actualSkill);
        }

        [Test]
        public void UserSkill_AppUser_ShouldAllowNull()
        {
            // Arrange
            var userSkill = new UserSkill();

            // Act
            userSkill.AppUser = null;
            var actualAppUser = userSkill.AppUser;

            // Assert
            Assert.IsNull(actualAppUser);
        }
    }
}
