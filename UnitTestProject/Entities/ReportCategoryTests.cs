using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class ReportCategoryTests
    {
        [Test]
        public void ReportCategory_Name_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var reportCategory = new ReportCategory();
            var expectedName = "Spam";

            // Act
            reportCategory.Name = expectedName;
            var actualName = reportCategory.Name;

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void ReportCategory_Description_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var reportCategory = new ReportCategory();
            var expectedDescription = "This is a spam report category.";

            // Act
            reportCategory.Description = expectedDescription;
            var actualDescription = reportCategory.Description;

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void ReportCategory_ReportCode_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var reportCategory = new ReportCategory();
            var expectedReportCode = "SPAM123";

            // Act
            reportCategory.ReportCode = expectedReportCode;
            var actualReportCode = reportCategory.ReportCode;

            // Assert
            Assert.AreEqual(expectedReportCode, actualReportCode);
        }

        [Test]
        public void ReportCategory_ReportCode_ShouldAllowNull()
        {
            // Arrange
            var reportCategory = new ReportCategory();

            // Act
            reportCategory.ReportCode = null;
            var actualReportCode = reportCategory.ReportCode;

            // Assert
            Assert.IsNull(actualReportCode);
        }

        [Test]
        public void ReportCategory_ShouldStartWithEmptyUserReports()
        {
            // Arrange
            var reportCategory = new ReportCategory();

            // Act
            var userReports = reportCategory.UserReports;

            // Assert
            Assert.IsNotNull(userReports);
            Assert.IsEmpty(userReports);
        }

        [Test]
        public void ReportCategory_ShouldAddUserReport()
        {
            // Arrange
            var reportCategory = new ReportCategory
            {
                UserReports = new List<UserReport>()
            };
            var userReport = new UserReport { Description = "Test Report" };

            // Act
            reportCategory.UserReports.Add(userReport);

            // Assert
            Assert.Contains(userReport, (System.Collections.ICollection)reportCategory.UserReports);
        }
    }
}
