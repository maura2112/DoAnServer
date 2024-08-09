using Domain.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class BaseEntityTests
    {
        [Test]
        public void BaseEntity_Id_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var baseEntity = new Mock<BaseEntity>().Object;
            var expectedId = 1;

            // Act
            baseEntity.Id = expectedId;
            var actualId = baseEntity.Id;

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void BaseEntity_IsDeleted_ShouldBeFalseByDefault()
        {
            // Arrange
            var baseEntity = new Mock<BaseEntity>().Object;

            // Act
            var isDeleted = baseEntity.IsDeleted;

            // Assert
            Assert.IsFalse(isDeleted);
        }
    }
}
