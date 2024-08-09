using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class AddressTests
    {
        [Test]
        public void Address_UserId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var address = new Address();
            var expectedUserId = 1;

            // Act
            address.UserId = expectedUserId;
            var actualUserId = address.UserId;

            // Assert
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [Test]
        public void Address_Street_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var address = new Address();
            var expectedStreet = "123 NTT";

            // Act
            address.Street = expectedStreet;
            var actualStreet = address.Street;

            // Assert
            Assert.AreEqual(expectedStreet, actualStreet);
        }

        [Test]
        public void Address_City_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var address = new Address();
            var expectedCity = "Hanoi";

            // Act
            address.City = expectedCity;
            var actualCity = address.City;

            // Assert
            Assert.AreEqual(expectedCity, actualCity);
        }

        [Test]
        public void Address_State_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var address = new Address();
            var expectedState = "HN";

            // Act
            address.State = expectedState;
            var actualState = address.State;

            // Assert
            Assert.AreEqual(expectedState, actualState);
        }

        [Test]
        public void Address_PostalCode_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var address = new Address();
            var expectedPostalCode = "100000";

            // Act
            address.PostalCode = expectedPostalCode;
            var actualPostalCode = address.PostalCode;

            // Assert
            Assert.AreEqual(expectedPostalCode, actualPostalCode);
        }

        [Test]
        public void Address_Country_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var address = new Address();
            var expectedCountry = "Vietnam";

            // Act
            address.Country = expectedCountry;
            var actualCountry = address.Country;

            // Assert
            Assert.AreEqual(expectedCountry, actualCountry);
        }

        [Test]
        public void Address_AppUser_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var address = new Address();
            var expectedAppUser = new AppUser { Name = "Test User" };

            // Act
            address.AppUser = expectedAppUser;
            var actualAppUser = address.AppUser;

            // Assert
            Assert.AreEqual(expectedAppUser, actualAppUser);
        }

        [Test]
        public void Address_Street_ShouldAllowNull()
        {
            // Arrange
            var address = new Address();

            // Act
            address.Street = null;
            var actualStreet = address.Street;

            // Assert
            Assert.IsNull(actualStreet);
        }

        [Test]
        public void Address_AppUser_ShouldAllowNull()
        {
            // Arrange
            var address = new Address();

            // Act
            address.AppUser = null;
            var actualAppUser = address.AppUser;

            // Assert
            Assert.IsNull(actualAppUser);
        }
    }
}
