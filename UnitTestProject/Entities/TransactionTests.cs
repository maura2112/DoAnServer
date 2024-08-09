using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void Transaction_Id_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedId = "TX12345";

            // Act
            transaction.Id = expectedId;
            var actualId = transaction.Id;

            // Assert
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public void Transaction_OrderCode_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedOrderCode = "OC12345";

            // Act
            transaction.OrderCode = expectedOrderCode;
            var actualOrderCode = transaction.OrderCode;

            // Assert
            Assert.AreEqual(expectedOrderCode, actualOrderCode);
        }

        [Test]
        public void Transaction_Amount_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedAmount = 1000;

            // Act
            transaction.Amount = expectedAmount;
            var actualAmount = transaction.Amount;

            // Assert
            Assert.AreEqual(expectedAmount, actualAmount);
        }

        [Test]
        public void Transaction_Type_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedType = "Credit";

            // Act
            transaction.Type = expectedType;
            var actualType = transaction.Type;

            // Assert
            Assert.AreEqual(expectedType, actualType);
        }

        [Test]
        public void Transaction_TotalMoney_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedTotalMoney = 1500;

            // Act
            transaction.TotalMoney = expectedTotalMoney;
            var actualTotalMoney = transaction.TotalMoney;

            // Assert
            Assert.AreEqual(expectedTotalMoney, actualTotalMoney);
        }

        [Test]
        public void Transaction_CreateAt_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedCreateAt = new DateTime(2024, 1, 1);

            // Act
            transaction.CreateAt = expectedCreateAt;
            var actualCreateAt = transaction.CreateAt;

            // Assert
            Assert.AreEqual(expectedCreateAt, actualCreateAt);
        }

        [Test]
        public void Transaction_TransactionDateTime_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedTransactionDateTime = new DateTime(2024, 1, 2);

            // Act
            transaction.TransactionDateTime = expectedTransactionDateTime;
            var actualTransactionDateTime = transaction.TransactionDateTime;

            // Assert
            Assert.AreEqual(expectedTransactionDateTime, actualTransactionDateTime);
        }

        [Test]
        public void Transaction_Description_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedDescription = "Transaction Description";

            // Act
            transaction.Description = expectedDescription;
            var actualDescription = transaction.Description;

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void Transaction_CounterAccountBankId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedCounterAccountBankId = "BANK123";

            // Act
            transaction.counterAccountBankId = expectedCounterAccountBankId;
            var actualCounterAccountBankId = transaction.counterAccountBankId;

            // Assert
            Assert.AreEqual(expectedCounterAccountBankId, actualCounterAccountBankId);
        }

        [Test]
        public void Transaction_CounterAccountName_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedCounterAccountName = "TruongBQ";

            // Act
            transaction.CounterAccountName = expectedCounterAccountName;
            var actualCounterAccountName = transaction.CounterAccountName;

            // Assert
            Assert.AreEqual(expectedCounterAccountName, actualCounterAccountName);
        }

        [Test]
        public void Transaction_CounterAccountNumber_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedCounterAccountNumber = "123456789";

            // Act
            transaction.CounterAccountNumber = expectedCounterAccountNumber;
            var actualCounterAccountNumber = transaction.CounterAccountNumber;

            // Assert
            Assert.AreEqual(expectedCounterAccountNumber, actualCounterAccountNumber);
        }

        [Test]
        public void Transaction_Reference_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedReference = "REF123";

            // Act
            transaction.reference = expectedReference;
            var actualReference = transaction.reference;

            // Assert
            Assert.AreEqual(expectedReference, actualReference);
        }

        [Test]
        public void Transaction_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedUserId = 1;

            // Act
            transaction.UserId = expectedUserId;
            var actualUserId = transaction.UserId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [Test]
        public void Transaction_User_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var transaction = new Transaction();
            var expectedUser = new AppUser { Name = "Test User" };

            // Act
            transaction.User = expectedUser;
            var actualUser = transaction.User;

            // Assert
            Assert.AreEqual(expectedUser, actualUser);
        }
    }
}
