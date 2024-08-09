using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class RatingTests
    {
        [Test]
        public void Rating_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var expectedUserId = 1;

            // Act
            rating.UserId = expectedUserId;
            var actualUserId = rating.UserId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [Test]
        public void Rating_Comment_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var expectedComment = "Great job!";

            // Act
            rating.Comment = expectedComment;
            var actualComment = rating.Comment;

            // Assert
            Assert.AreEqual(expectedComment, actualComment);
        }

        [Test]
        public void Rating_Star_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var expectedStar = 5;

            // Act
            rating.Star = expectedStar;
            var actualStar = rating.Star;

            // Assert
            Assert.AreEqual(expectedStar, actualStar);
        }

        [Test]
        public void Rating_RateToUserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var expectedRateToUserId = 2;

            // Act
            rating.RateToUserId = expectedRateToUserId;
            var actualRateToUserId = rating.RateToUserId;

            // Assert
            Assert.AreEqual(expectedRateToUserId, actualRateToUserId);
        }

        [Test]
        public void Rating_RateTransactionId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var expectedRateTransactionId = 3;

            // Act
            rating.RateTransactionId = expectedRateTransactionId;
            var actualRateTransactionId = rating.RateTransactionId;

            // Assert
            Assert.AreEqual(expectedRateTransactionId, actualRateTransactionId);
        }

        [Test]
        public void Rating_RateTransaction_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var expectedRateTransaction = new RateTransaction { };

            // Act
            rating.RateTransaction = expectedRateTransaction;
            var actualRateTransaction = rating.RateTransaction;

            // Assert
            Assert.AreEqual(expectedRateTransaction, actualRateTransaction);
        }

        [Test]
        public void Rating_CreatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var expectedCreatedDate = new DateTime(2024, 1, 1);

            // Act
            rating.CreatedDate = expectedCreatedDate;
            var actualCreatedDate = rating.CreatedDate;

            // Assert
            Assert.AreEqual(expectedCreatedDate, actualCreatedDate);
        }

        [Test]
        public void Rating_UpdatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var expectedUpdatedDate = new DateTime(2024, 2, 1);

            // Act
            rating.UpdatedDate = expectedUpdatedDate;
            var actualUpdatedDate = rating.UpdatedDate;

            // Assert
            Assert.AreEqual(expectedUpdatedDate, actualUpdatedDate);
        }

        [Test]
        public void Rating_UpdatedDate_ShouldAllowNull()
        {
            // Arrange
            var rating = new Rating();

            // Act
            rating.UpdatedDate = null;
            var actualUpdatedDate = rating.UpdatedDate;

            // Assert
            Assert.IsNull(actualUpdatedDate);
        }
    }
}
