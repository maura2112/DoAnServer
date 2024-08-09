using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class UrlRecordTests
    {
        [Test]
        public void UrlRecord_Id_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var urlRecord = new UrlRecord();
            var expectedId = 12345L;

            // Act
            urlRecord.Id = expectedId;
            var actualId = urlRecord.Id;

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void UrlRecord_EntityId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var urlRecord = new UrlRecord();
            var expectedEntityId = 1;

            // Act
            urlRecord.EntityId = expectedEntityId;
            var actualEntityId = urlRecord.EntityId;

            // Assert
            Assert.AreEqual(expectedEntityId, actualEntityId);
        }

        [Test]
        public void UrlRecord_EntityName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var urlRecord = new UrlRecord();
            var expectedEntityName = "TestEntity";

            // Act
            urlRecord.EntityName = expectedEntityName;
            var actualEntityName = urlRecord.EntityName;

            // Assert
            Assert.AreEqual(expectedEntityName, actualEntityName);
        }

        [Test]
        public void UrlRecord_Slug_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var urlRecord = new UrlRecord();
            var expectedSlug = "test-slug";

            // Act
            urlRecord.Slug = expectedSlug;
            var actualSlug = urlRecord.Slug;

            // Assert
            Assert.AreEqual(expectedSlug, actualSlug);
        }

        [Test]
        public void UrlRecord_IsActive_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var urlRecord = new UrlRecord();
            var expectedIsActive = true;

            // Act
            urlRecord.IsActive = expectedIsActive;
            var actualIsActive = urlRecord.IsActive;

            // Assert
            Assert.AreEqual(expectedIsActive, actualIsActive);
        }

        [Test]
        public void UrlRecord_EntityName_ShouldAllowNull()
        {
            // Arrange
            var urlRecord = new UrlRecord();

            // Act
            urlRecord.EntityName = null;
            var actualEntityName = urlRecord.EntityName;

            // Assert
            Assert.IsNull(actualEntityName);
        }

        [Test]
        public void UrlRecord_Slug_ShouldAllowNull()
        {
            // Arrange
            var urlRecord = new UrlRecord();

            // Act
            urlRecord.Slug = null;
            var actualSlug = urlRecord.Slug;

            // Assert
            Assert.IsNull(actualSlug);
        }
    }
}
