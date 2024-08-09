//using API.Controllers;
//using Application.IServices;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UnitTestProject.Controller
//{
//    [TestFixture]
//    public class ImageControllerTest
//    {
//        private Mock<IMediaService> _mediaServiceMock;
//        private ImagesController _controller;

//        [SetUp]
//        public void SetUp()
//        {
//            _mediaServiceMock = new Mock<IMediaService>();
//            _controller = new ImagesController(_mediaServiceMock.Object);
//        }

//        #region Upload


//        [Test]
//        public async Task UpLoadAsync_ReturnsOkResult_WithFileName()
//        {
//            // Arrange
//            var fileMock = new Mock<IFormFile>();
//            var cancellationToken = CancellationToken.None;
//            var expectedFileName = "uploaded_file.jpg";

//            _mediaServiceMock.Setup(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>()))
//                .ReturnsAsync(expectedFileName);

//            // Act
//            var result = await _controller.UpLoadAsync(fileMock.Object, cancellationToken) as OkObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(200, result.StatusCode);
//            Assert.AreEqual(expectedFileName, result.Value);
//        }
//        #endregion

//    }
//}
