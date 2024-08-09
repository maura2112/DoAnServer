using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class RelatedBlogTests
    {
        [Test]
        public void RelatedBlog_BlogId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var relatedBlog = new RelatedBlog();
            var expectedBlogId = 1;

            // Act
            relatedBlog.BlogId = expectedBlogId;
            var actualBlogId = relatedBlog.BlogId;

            // Assert
            Assert.AreEqual(expectedBlogId, actualBlogId);
        }

        [Test]
        public void RelatedBlog_RelatedBlogId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var relatedBlog = new RelatedBlog();
            var expectedRelatedBlogId = 2;

            // Act
            relatedBlog.RelatedBlogId = expectedRelatedBlogId;
            var actualRelatedBlogId = relatedBlog.RelatedBlogId;

            // Assert
            Assert.AreEqual(expectedRelatedBlogId, actualRelatedBlogId);
        }

        [Test]
        public void RelatedBlog_CreatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var relatedBlog = new RelatedBlog();
            var expectedCreatedDate = new DateTime(2024, 1, 1);

            // Act
            relatedBlog.CreatedDate = expectedCreatedDate;
            var actualCreatedDate = relatedBlog.CreatedDate;

            // Assert
            Assert.AreEqual(expectedCreatedDate, actualCreatedDate);
        }

        [Test]
        public void RelatedBlog_UpdatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var relatedBlog = new RelatedBlog();
            var expectedUpdatedDate = new DateTime(2024, 2, 1);

            // Act
            relatedBlog.UpdatedDate = expectedUpdatedDate;
            var actualUpdatedDate = relatedBlog.UpdatedDate;

            // Assert
            Assert.AreEqual(expectedUpdatedDate, actualUpdatedDate);
        }

        [Test]
        public void RelatedBlog_UpdatedDate_ShouldAllowNull()
        {
            // Arrange
            var relatedBlog = new RelatedBlog();

            // Act
            relatedBlog.UpdatedDate = null;
            var actualUpdatedDate = relatedBlog.UpdatedDate;

            // Assert
            Assert.IsNull(actualUpdatedDate);
        }

        [Test]
        public void RelatedBlog_Blog_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var relatedBlog = new RelatedBlog();
            var expectedBlog = new Blog { Title = "Test Blog" };

            // Act
            relatedBlog.Blog = expectedBlog;
            var actualBlog = relatedBlog.Blog;

            // Assert
            Assert.AreEqual(expectedBlog, actualBlog);
        }

        [Test]
        public void RelatedBlog_Blog_Related_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var relatedBlog = new RelatedBlog();
            var expectedBlogRelated = new Blog { Title = "Related Blog" };

            // Act
            relatedBlog.Blog_Related = expectedBlogRelated;
            var actualBlogRelated = relatedBlog.Blog_Related;

            // Assert
            Assert.AreEqual(expectedBlogRelated, actualBlogRelated);
        }
    }
}
