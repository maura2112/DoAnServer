using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class HubConnectionTests
    {
        [Test]
        public void HubConnection_Id_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var hubConnection = new HubConnection();
            var expectedId = 1;

            // Act
            hubConnection.Id = expectedId;
            var actualId = hubConnection.Id;

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void HubConnection_ConnectionId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var hubConnection = new HubConnection();
            var expectedConnectionId = "conn123";

            // Act
            hubConnection.ConnectionId = expectedConnectionId;
            var actualConnectionId = hubConnection.ConnectionId;

            // Assert
            Assert.AreEqual(expectedConnectionId, actualConnectionId);
        }

        [Test]
        public void HubConnection_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var hubConnection = new HubConnection();
            var expectedUserId = 1;

            // Act
            hubConnection.userId = expectedUserId;
            var actualUserId = hubConnection.userId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [Test]
        public void HubConnection_UserId_ShouldAllowNull()
        {
            // Arrange
            var hubConnection = new HubConnection();

            // Act
            hubConnection.userId = null;
            var actualUserId = hubConnection.userId;

            // Assert
            Assert.IsNull(actualUserId);
        }

        [Test]
        public void HubConnection_ConnectionId_ShouldAllowNull()
        {
            // Arrange
            var hubConnection = new HubConnection();

            // Act
            hubConnection.ConnectionId = null;
            var actualConnectionId = hubConnection.ConnectionId;

            // Assert
            Assert.IsNull(actualConnectionId);
        }
    }
}
