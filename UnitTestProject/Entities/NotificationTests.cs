using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class NotificationTests
    {
        [Test]
        public void Notification_NotificationId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedNotificationId = 1;

            // Act
            notification.NotificationId = expectedNotificationId;
            var actualNotificationId = notification.NotificationId;

            // Assert
            Assert.AreEqual(expectedNotificationId, actualNotificationId);
        }

        [Test]
        public void Notification_SendId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedSendId = 1;

            // Act
            notification.SendId = expectedSendId;
            var actualSendId = notification.SendId;

            // Assert
            Assert.AreEqual(expectedSendId, actualSendId);
        }

        [Test]
        public void Notification_SendUserName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedSendUserName = "TruongBQ";

            // Act
            notification.SendUserName = expectedSendUserName;
            var actualSendUserName = notification.SendUserName;

            // Assert
            Assert.AreEqual(expectedSendUserName, actualSendUserName);
        }

        [Test]
        public void Notification_ProjectName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedProjectName = "Project 1";

            // Act
            notification.ProjectName = expectedProjectName;
            var actualProjectName = notification.ProjectName;

            // Assert
            Assert.AreEqual(expectedProjectName, actualProjectName);
        }

        [Test]
        public void Notification_RecieveId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedRecieveId = 2;

            // Act
            notification.RecieveId = expectedRecieveId;
            var actualRecieveId = notification.RecieveId;

            // Assert
            Assert.AreEqual(expectedRecieveId, actualRecieveId);
        }

        [Test]
        public void Notification_Description_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedDescription = "Notification description";

            // Act
            notification.Description = expectedDescription;
            var actualDescription = notification.Description;

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void Notification_Datetime_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedDatetime = new DateTime(2024, 1, 1);

            // Act
            notification.Datetime = expectedDatetime;
            var actualDatetime = notification.Datetime;

            // Assert
            Assert.AreEqual(expectedDatetime, actualDatetime);
        }

        [Test]
        public void Notification_NotificationType_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedNotificationType = 1;

            // Act
            notification.NotificationType = expectedNotificationType;
            var actualNotificationType = notification.NotificationType;

            // Assert
            Assert.AreEqual(expectedNotificationType, actualNotificationType);
        }

        [Test]
        public void Notification_IsRead_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedIsRead = 1;

            // Act
            notification.IsRead = expectedIsRead;
            var actualIsRead = notification.IsRead;

            // Assert
            Assert.AreEqual(expectedIsRead, actualIsRead);
        }

        [Test]
        public void Notification_Link_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedLink = "/detail/1";

            // Act
            notification.Link = expectedLink;
            var actualLink = notification.Link;

            // Assert
            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void Notification_RecieveNavigation_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedRecieveNavigation = new AppUser { Name = "TruongBQ" };

            // Act
            notification.RecieveNavigation = expectedRecieveNavigation;
            var actualRecieveNavigation = notification.RecieveNavigation;

            // Assert
            Assert.AreEqual(expectedRecieveNavigation, actualRecieveNavigation);
        }

        [Test]
        public void Notification_SendNavigation_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var notification = new Notification();
            var expectedSendNavigation = new AppUser { Name = "DucLV" };

            // Act
            notification.SendNavigation = expectedSendNavigation;
            var actualSendNavigation = notification.SendNavigation;

            // Assert
            Assert.AreEqual(expectedSendNavigation, actualSendNavigation);
        }

        [Test]
        public void Notification_SendId_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.SendId = null;
            var actualSendId = notification.SendId;

            // Assert
            Assert.IsNull(actualSendId);
        }

        [Test]
        public void Notification_SendUserName_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.SendUserName = null;
            var actualSendUserName = notification.SendUserName;

            // Assert
            Assert.IsNull(actualSendUserName);
        }

        [Test]
        public void Notification_ProjectName_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.ProjectName = null;
            var actualProjectName = notification.ProjectName;

            // Assert
            Assert.IsNull(actualProjectName);
        }

        [Test]
        public void Notification_RecieveId_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.RecieveId = null;
            var actualRecieveId = notification.RecieveId;

            // Assert
            Assert.IsNull(actualRecieveId);
        }

        [Test]
        public void Notification_Description_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.Description = null;
            var actualDescription = notification.Description;

            // Assert
            Assert.IsNull(actualDescription);
        }

        [Test]
        public void Notification_Datetime_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.Datetime = null;
            var actualDatetime = notification.Datetime;

            // Assert
            Assert.IsNull(actualDatetime);
        }

        [Test]
        public void Notification_NotificationType_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.NotificationType = null;
            var actualNotificationType = notification.NotificationType;

            // Assert
            Assert.IsNull(actualNotificationType);
        }

        [Test]
        public void Notification_IsRead_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.IsRead = null;
            var actualIsRead = notification.IsRead;

            // Assert
            Assert.IsNull(actualIsRead);
        }

        [Test]
        public void Notification_Link_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.Link = null;
            var actualLink = notification.Link;

            // Assert
            Assert.IsNull(actualLink);
        }

        [Test]
        public void Notification_RecieveNavigation_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.RecieveNavigation = null;
            var actualRecieveNavigation = notification.RecieveNavigation;

            // Assert
            Assert.IsNull(actualRecieveNavigation);
        }

        [Test]
        public void Notification_SendNavigation_ShouldAllowNull()
        {
            // Arrange
            var notification = new Notification();

            // Act
            notification.SendNavigation = null;
            var actualSendNavigation = notification.SendNavigation;

            // Assert
            Assert.IsNull(actualSendNavigation);
        }
    }
}
