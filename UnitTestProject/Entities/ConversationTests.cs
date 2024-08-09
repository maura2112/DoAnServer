using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class ConversationTests
    {
        [Test]
        public void Conversation_ConversationId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var conversation = new Conversation();
            var expectedConversationId = 1;

            // Act
            conversation.ConversationId = expectedConversationId;
            var actualConversationId = conversation.ConversationId;

            // Assert
            Assert.AreEqual(expectedConversationId, actualConversationId);
        }

        [Test]
        public void Conversation_User1_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var conversation = new Conversation();
            var expectedUser1 = 1;

            // Act
            conversation.User1 = expectedUser1;
            var actualUser1 = conversation.User1;

            // Assert
            Assert.AreEqual(expectedUser1, actualUser1);
        }

        [Test]
        public void Conversation_User2_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var conversation = new Conversation();
            var expectedUser2 = 2;

            // Act
            conversation.User2 = expectedUser2;
            var actualUser2 = conversation.User2;

            // Assert
            Assert.AreEqual(expectedUser2, actualUser2);
        }

        [Test]
        public void Conversation_User1Navigation_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var conversation = new Conversation();
            var expectedUser1Navigation = new AppUser { Name = "User1" };

            // Act
            conversation.User1Navigation = expectedUser1Navigation;
            var actualUser1Navigation = conversation.User1Navigation;

            // Assert
            Assert.AreEqual(expectedUser1Navigation, actualUser1Navigation);
        }

        [Test]
        public void Conversation_User2Navigation_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var conversation = new Conversation();
            var expectedUser2Navigation = new AppUser { Name = "User2" };

            // Act
            conversation.User2Navigation = expectedUser2Navigation;
            var actualUser2Navigation = conversation.User2Navigation;

            // Assert
            Assert.AreEqual(expectedUser2Navigation, actualUser2Navigation);
        }

        [Test]
        public void Conversation_ShouldStartWithEmptyMessages()
        {
            // Arrange
            var conversation = new Conversation();

            // Act
            var messages = conversation.Messages;

            // Assert
            Assert.IsNotNull(messages);
            Assert.IsEmpty(messages);
        }

        [Test]
        public void Conversation_ShouldAddMessage()
        {
            // Arrange
            var conversation = new Conversation();
            var message = new Message { MessagesId = 1, MessageText = "Hello" };

            // Act
            conversation.Messages.Add(message);

            // Assert
            Assert.Contains(message, (System.Collections.ICollection)conversation.Messages);
        }

        [Test]
        public void Conversation_User1_ShouldAllowNull()
        {
            // Arrange
            var conversation = new Conversation();

            // Act
            conversation.User1 = null;
            var actualUser1 = conversation.User1;

            // Assert
            Assert.IsNull(actualUser1);
        }

        [Test]
        public void Conversation_User2_ShouldAllowNull()
        {
            // Arrange
            var conversation = new Conversation();

            // Act
            conversation.User2 = null;
            var actualUser2 = conversation.User2;

            // Assert
            Assert.IsNull(actualUser2);
        }

        [Test]
        public void Conversation_User1Navigation_ShouldAllowNull()
        {
            // Arrange
            var conversation = new Conversation();

            // Act
            conversation.User1Navigation = null;
            var actualUser1Navigation = conversation.User1Navigation;

            // Assert
            Assert.IsNull(actualUser1Navigation);
        }

        [Test]
        public void Conversation_User2Navigation_ShouldAllowNull()
        {
            // Arrange
            var conversation = new Conversation();

            // Act
            conversation.User2Navigation = null;
            var actualUser2Navigation = conversation.User2Navigation;

            // Assert
            Assert.IsNull(actualUser2Navigation);
        }
    }
}
