using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class MediaFileTests
    {
        [Test]
        public void MediaFile_FileName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedFileName = "test.jpg";

            // Act
            mediaFile.FileName = expectedFileName;
            var actualFileName = mediaFile.FileName;

            // Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [Test]
        public void MediaFile_CreateAt_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedCreateAt = new DateTime(2024, 1, 1);

            // Act
            mediaFile.CreateAt = expectedCreateAt;
            var actualCreateAt = mediaFile.CreateAt;

            // Assert
            Assert.AreEqual(expectedCreateAt, actualCreateAt);
        }
    }
}
