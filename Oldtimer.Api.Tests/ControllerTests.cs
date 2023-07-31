//using System;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Oldtimer.Api.Controller;
//using Oldtimer.Api.Data;
//using Oldtimer.Api.Service;
//using Xunit;

//namespace Oldtimer.Api.Tests
//{
//    public class ControllerTests
//    {
//        private readonly ApiController _controller;
//        private readonly Mock<IApiService> _mockService;

//        public ControllerTests()
//        {
//            _mockService = new Mock<IApiService>();
//            _controller = new ApiController(_mockService.Object);
//        }

//        // Test for GetSammlers
//        [Fact]
//        public void GetSammlers_Should_Return_OkResult_With_Sammlers()
//        {
//            // Arrange
//            var sammlers = new List<Sammler>
//            {
//                new Sammler { Id = 1, Firstname = "Max" },
//                new Sammler { Id = 2, Firstname = "Anna" }
//            };
//            _mockService.Setup(s => s.GetSammlers()).Returns(sammlers);

//            // Act
//            var result = _controller.GetSammlers();

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var model = Assert.IsAssignableFrom<IEnumerable<Sammler>>(okResult.Value);
//            Assert.Equal(sammlers, model);
//        }

//        // Test for GetSammlerById with a valid ID
//        [Fact]
//        public void GetSammlerById_Should_Return_OkResult_With_Sammler_When_ValidId()
//        {
//            // Arrange
//            var sammlerId = 1;
//            var sammler = new Sammler { Id = sammlerId, Firstname = "Max" };
//            _mockService.Setup(s => s.GetSammlerById(sammlerId)).Returns(sammler);

//            // Act
//            var result = _controller.GetSammlerById(sammlerId);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var model = Assert.IsAssignableFrom<Sammler>(okResult.Value);
//            Assert.Equal(sammler, model);
//        }

//        // Test for GetSammlerById with an invalid ID
//        [Fact]
//        public void GetSammlerById_Should_Return_NotFoundResult_When_InvalidId()
//        {
//            // Arrange
//            var invalidId = 999; // Assume this ID is invalid and not present in the mocked data.
//            _mockService.Setup(s => s.GetSammlerById(invalidId)).Returns((Sammler)null);

//            // Act
//            var result = _controller.GetSammlerById(invalidId);

//            // Assert
//            var notFoundResult = Assert.IsType<NotFoundResult>(result);
//        }

//        // Test for CreateSammler when Sammler already exists
//        [Fact]
//        public void CreateSammler_Should_Return_ConflictResult_When_Sammler_Already_Exists()
//        {
//            // Arrange
//            var newSammler = new Sammler { Firstname = "Max" };
//            _mockService.Setup(s => s.SammlerVorhanden(newSammler)).Returns(true);

//            // Act
//            var result = _controller.CreateSammler(newSammler);

//            // Assert
//            var conflictResult = Assert.IsType<ActionResult<Sammler>>(result);
//            Assert.Equal(newSammler, conflictResult.Value);
//        }

//        // Test for CreateSammler when Sammler does not exist
//        [Fact]
//        public void CreateSammler_Should_Return_OkResult_When_Sammler_Does_Not_Exist()
//        {
//            // Arrange
//            var newSammler = new Sammler { Firstname = "Max" };
//            _mockService.Setup(s => s.SammlerVorhanden(newSammler)).Returns(false);

//            // Act
//            var result = _controller.CreateSammler(newSammler);

//            // Assert
//            var okResult = Assert.IsType<OkResult>(result);
//        }

//        // Add more tests for other methods in the ApiController class...

//    }
//}