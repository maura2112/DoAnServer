using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class AppUserTests
    {
        [Test]
        public void AppUser_Name_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedName = "TruongBQ";

            // Act
            user.Name = expectedName;
            var actualName = user.Name;

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void AppUser_CreatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedCreatedDate = new DateTime(2024, 1, 1);

            // Act
            user.CreatedDate = expectedCreatedDate;
            var actualCreatedDate = user.CreatedDate;

            // Assert
            Assert.AreEqual(expectedCreatedDate, actualCreatedDate);
        }

        [Test]
        public void AppUser_UpdatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedUpdatedDate = new DateTime(2024, 2, 1);

            // Act
            user.UpdatedDate = expectedUpdatedDate;
            var actualUpdatedDate = user.UpdatedDate;

            // Assert
            Assert.AreEqual(expectedUpdatedDate, actualUpdatedDate);
        }

        [Test]
        public void AppUser_Address_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedAddress = new Address { City = "Lao Cai" };

            // Act
            user.Address = expectedAddress;
            var actualAddress = user.Address;

            // Assert
            Assert.AreEqual(expectedAddress, actualAddress);
        }

        [Test]
        public void AppUser_Description_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedDescription = "This is a description.";

            // Act
            user.Description = expectedDescription;
            var actualDescription = user.Description;

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void AppUser_TaxCode_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedTaxCode = "TX123";

            // Act
            user.TaxCode = expectedTaxCode;
            var actualTaxCode = user.TaxCode;

            // Assert
            Assert.AreEqual(expectedTaxCode, actualTaxCode);
        }

        [Test]
        public void AppUser_IsCompany_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedIsCompany = true;

            // Act
            user.IsCompany = expectedIsCompany;
            var actualIsCompany = user.IsCompany;

            // Assert
            Assert.AreEqual(expectedIsCompany, actualIsCompany);
        }

        [Test]
        public void AppUser_AmountBid_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedAmountBid = 5;

            // Act
            user.AmountBid = expectedAmountBid;
            var actualAmountBid = user.AmountBid;

            // Assert
            Assert.AreEqual(expectedAmountBid, actualAmountBid);
        }

        [Test]
        public void AppUser_IsPaid_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedIsPaid = true;

            // Act
            user.IsPaid = expectedIsPaid;
            var actualIsPaid = user.IsPaid;

            // Assert
            Assert.AreEqual(expectedIsPaid, actualIsPaid);
        }

        [Test]
        public void AppUser_AmoutProject_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedAmoutProject = 3;

            // Act
            user.AmoutProject = expectedAmoutProject;
            var actualAmoutProject = user.AmoutProject;

            // Assert
            Assert.AreEqual(expectedAmoutProject, actualAmoutProject);
        }

        [Test]
        public void AppUser_Education_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedEducation = "Bachelor's Degree";

            // Act
            user.Education = expectedEducation;
            var actualEducation = user.Education;

            // Assert
            Assert.AreEqual(expectedEducation, actualEducation);
        }

        [Test]
        public void AppUser_Experience_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedExperience = "5 years of experience";

            // Act
            user.Experience = expectedExperience;
            var actualExperience = user.Experience;

            // Assert
            Assert.AreEqual(expectedExperience, actualExperience);
        }

        [Test]
        public void AppUser_Qualifications_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedQualifications = "Certified Professional";

            // Act
            user.Qualifications = expectedQualifications;
            var actualQualifications = user.Qualifications;

            // Assert
            Assert.AreEqual(expectedQualifications, actualQualifications);
        }

        [Test]
        public void AppUser_Avatar_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedAvatar = "avatar.png";

            // Act
            user.Avatar = expectedAvatar;
            var actualAvatar = user.Avatar;

            // Assert
            Assert.AreEqual(expectedAvatar, actualAvatar);
        }

        [Test]
        public void AppUser_PasswordResetToken_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedPasswordResetToken = "token123";

            // Act
            user.PasswordResetToken = expectedPasswordResetToken;
            var actualPasswordResetToken = user.PasswordResetToken;

            // Assert
            Assert.AreEqual(expectedPasswordResetToken, actualPasswordResetToken);
        }

        [Test]
        public void AppUser_ResetTokenExpires_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var user = new AppUser();
            var expectedResetTokenExpires = new DateTime(2024, 3, 1);

            // Act
            user.ResetTokenExpires = expectedResetTokenExpires;
            var actualResetTokenExpires = user.ResetTokenExpires;

            // Assert
            Assert.AreEqual(expectedResetTokenExpires, actualResetTokenExpires);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyBids()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var bids = user.Bids;

            // Assert
            Assert.IsNotNull(bids);
            Assert.IsEmpty(bids);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyBookmarks()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var bookmarks = user.Bookmarks;

            // Assert
            Assert.IsNotNull(bookmarks);
            Assert.IsEmpty(bookmarks);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyUserProjects()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var userProjects = user.UserProjects;

            // Assert
            Assert.IsNotNull(userProjects);
            Assert.IsEmpty(userProjects);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyUserSkills()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var userSkills = user.UserSkills;

            // Assert
            Assert.IsNotNull(userSkills);
            Assert.IsEmpty(userSkills);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyMediaFiles()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var mediaFiles = user.MediaFiles;

            // Assert
            Assert.IsNotNull(mediaFiles);
            Assert.IsEmpty(mediaFiles);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyUserReports()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var userReports = user.UserReports;

            // Assert
            Assert.IsNotNull(userReports);
            Assert.IsEmpty(userReports);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyBlogs()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var blogs = user.Blogs;

            // Assert
            Assert.IsNotNull(blogs);
            Assert.IsEmpty(blogs);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyRecieveNavigations()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var recieveNavigations = user.RecieveNavigations;

            // Assert
            Assert.IsNotNull(recieveNavigations);
            Assert.IsEmpty(recieveNavigations);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptySendNavigations()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var sendNavigations = user.SendNavigations;

            // Assert
            Assert.IsNotNull(sendNavigations);
            Assert.IsEmpty(sendNavigations);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyUser1Navigations()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var user1Navigations = user.User1Navigations;

            // Assert
            Assert.IsNotNull(user1Navigations);
            Assert.IsEmpty(user1Navigations);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyFavoriteProjects()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var favoriteProjects = user.FavoriteProjects;

            // Assert
            Assert.IsNotNull(favoriteProjects);
            Assert.IsEmpty(favoriteProjects);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptyUser2Navigations()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var user2Navigations = user.User2Navigations;

            // Assert
            Assert.IsNotNull(user2Navigations);
            Assert.IsEmpty(user2Navigations);
        }

        [Test]
        public void AppUser_ShouldStartWithEmptySenders()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var senders = user.Senders;

            // Assert
            Assert.IsNotNull(senders);
            Assert.IsEmpty(senders);
        }

        [Test]
        public void AppUser_UpdatedDate_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.UpdatedDate = null;
            var actualUpdatedDate = user.UpdatedDate;

            // Assert
            Assert.IsNull(actualUpdatedDate);
        }

        [Test]
        public void AppUser_Address_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.Address = null;
            var actualAddress = user.Address;

            // Assert
            Assert.IsNull(actualAddress);
        }

        [Test]
        public void AppUser_Description_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.Description = null;
            var actualDescription = user.Description;

            // Assert
            Assert.IsNull(actualDescription);
        }

        [Test]
        public void AppUser_TaxCode_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.TaxCode = null;
            var actualTaxCode = user.TaxCode;

            // Assert
            Assert.IsNull(actualTaxCode);
        }

        [Test]
        public void AppUser_Education_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.Education = null;
            var actualEducation = user.Education;

            // Assert
            Assert.IsNull(actualEducation);
        }

        [Test]
        public void AppUser_Experience_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.Experience = null;
            var actualExperience = user.Experience;

            // Assert
            Assert.IsNull(actualExperience);
        }

        [Test]
        public void AppUser_Qualifications_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.Qualifications = null;
            var actualQualifications = user.Qualifications;

            // Assert
            Assert.IsNull(actualQualifications);
        }

        [Test]
        public void AppUser_Avatar_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.Avatar = null;
            var actualAvatar = user.Avatar;

            // Assert
            Assert.IsNull(actualAvatar);
        }

        [Test]
        public void AppUser_PasswordResetToken_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.PasswordResetToken = null;
            var actualPasswordResetToken = user.PasswordResetToken;

            // Assert
            Assert.IsNull(actualPasswordResetToken);
        }

        [Test]
        public void AppUser_ResetTokenExpires_ShouldAllowNull()
        {
            // Arrange
            var user = new AppUser();

            // Act
            user.ResetTokenExpires = null;
            var actualResetTokenExpires = user.ResetTokenExpires;

            // Assert
            Assert.IsNull(actualResetTokenExpires);
        }
    }
}
