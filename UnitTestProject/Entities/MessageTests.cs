using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class MessageTests
    {
        [Test]
        public void Message_MessagesId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedMessagesId = 1;

            // Act
            message.MessagesId = expectedMessagesId;
            var actualMessagesId = message.MessagesId;

            // Assert
            Assert.AreEqual(expectedMessagesId, actualMessagesId);
        }

        [Test]
        public void Message_ConversationId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedConversationId = 1;

            // Act
            message.ConversationId = expectedConversationId;
            var actualConversationId = message.ConversationId;

            // Assert
            Assert.AreEqual(expectedConversationId, actualConversationId);
        }

        [Test]
        public void Message_SenderId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedSenderId = 1;

            // Act
            message.SenderId = expectedSenderId;
            var actualSenderId = message.SenderId;

            // Assert
            Assert.AreEqual(expectedSenderId, actualSenderId);
        }

        [Test]
        public void Message_MessageText_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedMessageText = "Hello, this is a test message.";

            // Act
            message.MessageText = expectedMessageText;
            var actualMessageText = message.MessageText;

            // Assert
            Assert.AreEqual(expectedMessageText, actualMessageText);
        }

        [Test]
        public void Message_SendDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedSendDate = new DateTime(2024, 1, 1);

            // Act
            message.SendDate = expectedSendDate;
            var actualSendDate = message.SendDate;

            // Assert
            Assert.AreEqual(expectedSendDate, actualSendDate);
        }

        [Test]
        public void Message_IsRead_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedIsRead = 1;

            // Act
            message.IsRead = expectedIsRead;
            var actualIsRead = message.IsRead;

            // Assert
            Assert.AreEqual(expectedIsRead, actualIsRead);
        }

        [Test]
        public void Message_MessageType_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedMessageType = 1;

            // Act
            message.MessageType = expectedMessageType;
            var actualMessageType = message.MessageType;

            // Assert
            Assert.AreEqual(expectedMessageType, actualMessageType);
        }

        [Test]
        public void Message_File_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedFile = "file.png";

            // Act
            message.File = expectedFile;
            var actualFile = message.File;

            // Assert
            Assert.AreEqual(expectedFile, actualFile);
        }

        [Test]
        public void Message_Conversation_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedConversation = new Conversation { ConversationId = 1 };

            // Act
            message.Conversation = expectedConversation;
            var actualConversation = message.Conversation;

            // Assert
            Assert.AreEqual(expectedConversation, actualConversation);
        }

        [Test]
        public void Message_Sender_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var message = new Message();
            var expectedSender = new AppUser { Name = "Truong BQ" };

            // Act
            message.Sender = expectedSender;
            var actualSender = message.Sender;

            // Assert
            Assert.AreEqual(expectedSender, actualSender);
        }

        [Test]
        public void Message_ConversationId_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.ConversationId = null;
            var actualConversationId = message.ConversationId;

            // Assert
            Assert.IsNull(actualConversationId);
        }

        [Test]
        public void Message_SenderId_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.SenderId = null;
            var actualSenderId = message.SenderId;

            // Assert
            Assert.IsNull(actualSenderId);
        }

        [Test]
        public void Message_MessageText_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.MessageText = null;
            var actualMessageText = message.MessageText;

            // Assert
            Assert.IsNull(actualMessageText);
        }

        [Test]
        public void Message_SendDate_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.SendDate = null;
            var actualSendDate = message.SendDate;

            // Assert
            Assert.IsNull(actualSendDate);
        }

        [Test]
        public void Message_IsRead_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.IsRead = null;
            var actualIsRead = message.IsRead;

            // Assert
            Assert.IsNull(actualIsRead);
        }

        [Test]
        public void Message_MessageType_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.MessageType = null;
            var actualMessageType = message.MessageType;

            // Assert
            Assert.IsNull(actualMessageType);
        }

        [Test]
        public void Message_File_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.File = null;
            var actualFile = message.File;

            // Assert
            Assert.IsNull(actualFile);
        }

        [Test]
        public void Message_Conversation_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.Conversation = null;
            var actualConversation = message.Conversation;

            // Assert
            Assert.IsNull(actualConversation);
        }

        [Test]
        public void Message_Sender_ShouldAllowNull()
        {
            // Arrange
            var message = new Message();

            // Act
            message.Sender = null;
            var actualSender = message.Sender;

            // Assert
            Assert.IsNull(actualSender);
        }
    }
}
