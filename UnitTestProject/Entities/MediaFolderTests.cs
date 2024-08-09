using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class MediaFolderTests
    {
        [Test]
        public void MediaFile_Id_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedId = 1L;

            // Act
            mediaFile.Id = expectedId;
            var actualId = mediaFile.Id;

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void MediaFile_FileName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedFileName = "file.png";

            // Act
            mediaFile.FileName = expectedFileName;
            var actualFileName = mediaFile.FileName;

            // Assert
            Assert.AreEqual(expectedFileName, actualFileName);
        }

        [Test]
        public void MediaFile_Description_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedDescription = "This is a description.";

            // Act
            mediaFile.Description = expectedDescription;
            var actualDescription = mediaFile.Description;

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void MediaFile_Title_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedTitle = "File Title";

            // Act
            mediaFile.Title = expectedTitle;
            var actualTitle = mediaFile.Title;

            // Assert
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [Test]
        public void MediaFile_FolderId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedFolderId = 1;

            // Act
            mediaFile.FolderId = expectedFolderId;
            var actualFolderId = mediaFile.FolderId;

            // Assert
            Assert.AreEqual(expectedFolderId, actualFolderId);
        }

        [Test]
        public void MediaFile_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedUserId = 1;

            // Act
            mediaFile.UserId = expectedUserId;
            var actualUserId = mediaFile.UserId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
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

        [Test]
        public void MediaFile_UpdateAt_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedUpdateAt = new DateTime(2024, 2, 1);

            // Act
            mediaFile.UpdateAt = expectedUpdateAt;
            var actualUpdateAt = mediaFile.UpdateAt;

            // Assert
            Assert.AreEqual(expectedUpdateAt, actualUpdateAt);
        }

        [Test]
        public void MediaFile_User_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedUser = new AppUser { Name = "John Doe" };

            // Act
            mediaFile.User = expectedUser;
            var actualUser = mediaFile.User;

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }

        [Test]
        public void MediaFile_MediaFolder_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var mediaFile = new MediaFile();
            var expectedMediaFolder = new MediaFolder { Name = "Folder 1" };

            // Act
            mediaFile.MediaFolder = expectedMediaFolder;
            var actualMediaFolder = mediaFile.MediaFolder;

            // Assert
            Assert.AreEqual(expectedMediaFolder, actualMediaFolder);
        }

        [Test]
        public void MediaFile_Description_ShouldAllowNull()
        {
            // Arrange
            var mediaFile = new MediaFile();

            // Act
            mediaFile.Description = null;
            var actualDescription = mediaFile.Description;

            // Assert
            Assert.IsNull(actualDescription);
        }

        [Test]
        public void MediaFile_Title_ShouldAllowNull()
        {
            // Arrange
            var mediaFile = new MediaFile();

            // Act
            mediaFile.Title = null;
            var actualTitle = mediaFile.Title;

            // Assert
            Assert.IsNull(actualTitle);
        }

        [Test]
        public void MediaFile_FolderId_ShouldAllowNull()
        {
            // Arrange
            var mediaFile = new MediaFile();

            // Act
            mediaFile.FolderId = null;
            var actualFolderId = mediaFile.FolderId;

            // Assert
            Assert.IsNull(actualFolderId);
        }

        [Test]
        public void MediaFile_UserId_ShouldAllowNull()
        {
            // Arrange
            var mediaFile = new MediaFile();

            // Act
            mediaFile.UserId = null;
            var actualUserId = mediaFile.UserId;

            // Assert
            Assert.IsNull(actualUserId);
        }

        [Test]
        public void MediaFile_UpdateAt_ShouldAllowNull()
        {
            // Arrange
            var mediaFile = new MediaFile();

            // Act
            mediaFile.UpdateAt = null;
            var actualUpdateAt = mediaFile.UpdateAt;

            // Assert
            Assert.IsNull(actualUpdateAt);
        }

        [Test]
        public void MediaFile_MediaFolder_ShouldAllowNull()
        {
            // Arrange
            var mediaFile = new MediaFile();

            // Act
            mediaFile.MediaFolder = null;
            var actualMediaFolder = mediaFile.MediaFolder;

            // Assert
            Assert.IsNull(actualMediaFolder);
        }
    }
}
