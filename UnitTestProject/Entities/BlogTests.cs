using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace UnitTestProject.Entities
{
    [TestFixture]
    public class BlogTests
    {
        [Test]
        public void Blog_CreatedBy_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedCreatedBy = 1;

            // Act
            blog.CreatedBy = expectedCreatedBy;
            var actualCreatedBy = blog.CreatedBy;

            // Assert
            Assert.AreEqual(expectedCreatedBy, actualCreatedBy);
        }

        [Test]
        public void Blog_Title_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedTitle = "Test Blog";

            // Act
            blog.Title = expectedTitle;
            var actualTitle = blog.Title;

            // Assert
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [Test]
        public void Blog_ShortDescription_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedShortDescription = "Short Description";

            // Act
            blog.ShortDescription = expectedShortDescription;
            var actualShortDescription = blog.ShortDescription;

            // Assert
            Assert.AreEqual(expectedShortDescription, actualShortDescription);
        }

        [Test]
        public void Blog_Description_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedDescription = "Full Description";

            // Act
            blog.Description = expectedDescription;
            var actualDescription = blog.Description;

            // Assert
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void Blog_CategoryId_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedCategoryId = 2;

            // Act
            blog.CategoryId = expectedCategoryId;
            var actualCategoryId = blog.CategoryId;

            // Assert
            Assert.AreEqual(expectedCategoryId, actualCategoryId);
        }

        [Test]
        public void Blog_BlogImage_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedBlogImage = "image.png";

            // Act
            blog.BlogImage = expectedBlogImage;
            var actualBlogImage = blog.BlogImage;

            // Assert
            Assert.AreEqual(expectedBlogImage, actualBlogImage);
        }

        [Test]
        public void Blog_IsPublished_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedIsPublished = true;

            // Act
            blog.IsPublished = expectedIsPublished;
            var actualIsPublished = blog.IsPublished;

            // Assert
            Assert.AreEqual(expectedIsPublished, actualIsPublished);
        }

        [Test]
        public void Blog_IsHot_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedIsHot = true;

            // Act
            blog.IsHot = expectedIsHot;
            var actualIsHot = blog.IsHot;

            // Assert
            Assert.AreEqual(expectedIsHot, actualIsHot);
        }

        [Test]
        public void Blog_IsHomePage_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedIsHomePage = true;

            // Act
            blog.IsHomePage = expectedIsHomePage;
            var actualIsHomePage = blog.IsHomePage;

            // Assert
            Assert.AreEqual(expectedIsHomePage, actualIsHomePage);
        }

        [Test]
        public void Blog_Category_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedCategory = new Category { CategoryName = "Programming" };

            // Act
            blog.Category = expectedCategory;
            var actualCategory = blog.Category;

            // Assert
            Assert.AreEqual(expectedCategory, actualCategory);
        }

        [Test]
        public void Blog_CreatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedCreatedDate = new DateTime(2024, 1, 1);

            // Act
            blog.CreatedDate = expectedCreatedDate;
            var actualCreatedDate = blog.CreatedDate;

            // Assert
            Assert.AreEqual(expectedCreatedDate, actualCreatedDate);
        }

        [Test]
        public void Blog_UpdatedDate_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedUpdatedDate = new DateTime(2024, 2, 1);

            // Act
            blog.UpdatedDate = expectedUpdatedDate;
            var actualUpdatedDate = blog.UpdatedDate;

            // Assert
            Assert.AreEqual(expectedUpdatedDate, actualUpdatedDate);
        }

        [Test]
        public void Blog_UpdatedDate_ShouldAllowNull()
        {
            // Arrange
            var blog = new Blog();

            // Act
            blog.UpdatedDate = null;
            var actualUpdatedDate = blog.UpdatedDate;

            // Assert
            Assert.IsNull(actualUpdatedDate);
        }

        [Test]
        public void Blog_AppUser_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var blog = new Blog();
            var expectedAppUser = new AppUser { Name = "TruongBQ" };

            // Act
            blog.AppUser = expectedAppUser;
            var actualAppUser = blog.AppUser;

            // Assert
            Assert.AreEqual(expectedAppUser, actualAppUser);
        }

        [Test]
        public void Blog_ShouldStartWithEmptyRelatedBlogs()
        {
            // Arrange
            var blog = new Blog();

            // Act
            var relatedBlogs = blog.RelatedBlogs;

            // Assert
            Assert.IsNotNull(relatedBlogs);
            Assert.IsEmpty(relatedBlogs);
        }

        [Test]
        public void Blog_ShouldAddRelatedBlog()
        {
            // Arrange
            var blog = new Blog();
            var relatedBlog = new RelatedBlog { BlogId = 1, RelatedBlogId = 2 };

            // Act
            blog.RelatedBlogs.Add(relatedBlog);

            // Assert
            Assert.Contains(relatedBlog, (System.Collections.ICollection)blog.RelatedBlogs);
        }
    }
}
