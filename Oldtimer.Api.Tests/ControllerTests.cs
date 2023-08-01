using Xunit;
using Oldtimer.Api.Data;
using Oldtimer.Api.Service;
using Oldtimer.Api.Controller;
using Microsoft.AspNetCore.Mvc;

namespace Oldtimer.Api.Tests
{
    public class ApiControllerTests
    {
        private readonly ApiController _controller;

        public ApiControllerTests()
        {
            var apiService = Mocks.CreateMockApiService().Object;
            _controller = new ApiController(apiService);
        }

        // Test for GetSammlers method
        [Fact]
        public void Test_GetSammlers_ReturnsOkResultWithListOfSammlers()
        {
            // Act
            var result = _controller.GetSammlers();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<List<Sammler>>(okResult.Value);
            Assert.Equal(Mocks.SammlersList.Count, (okResult.Value as List<Sammler>).Count);
        }

        // Test for GetSammlerById method with existing Sammler
        [Fact]
        public void Test_GetSammlerById_WithExistingSammler_ReturnsOkResultWithSammler()
        {
            // Arrange
            long sammlerIdToFind = 1;

            // Act
            var result = _controller.GetSammlerById(sammlerIdToFind);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<Sammler>(okResult.Value);
            var sammler = okResult.Value as Sammler;
            Assert.Equal("John", sammler.Firstname);
        }

        // Test for GetSammlerById method with non-existing Sammler
        [Fact]
        public void Test_GetSammlerById_WithNonExistingSammler_ReturnsNotFoundResult()
        {
            // Arrange
            long nonExistingSammlerId = 999;

            // Act
            var result = _controller.GetSammlerById(nonExistingSammlerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test for CreateSammler method with a new Sammler
        [Fact]
        public void Test_CreateSammler_WithNewSammler_ReturnsOkResult()
        {
            // Arrange
            var newSammler = new Sammler { Id = 3, Firstname = "Alice", Surname = "Smith" };

            // Act
            var result = _controller.CreateSammler(newSammler);

            // Assert
            Assert.IsType<ActionResult<Sammler>>(result);
            Assert.IsType<OkResult>(result.Result); // Hier überprüfen wir den Typ des ActionResult.Result
        }

        // Test for CreateSammler method with an existing Sammler
        [Fact]
        public void Test_CreateSammler_WithExistingSammler_ReturnsConflictResult()
        {
            // Arrange
            var existingSammler = new Sammler { Id = 1, Firstname = "John", Surname = "Doe" };

            // Act
            var result = _controller.CreateSammler(existingSammler);

            // Assert
            Assert.IsType<ActionResult<Sammler>>(result);
            Assert.IsType<ConflictObjectResult>(result.Result); // Hier überprüfen wir den Typ des ActionResult.Result
        }
        // Test for DeleteSammler method with existing Sammler
        [Fact]
        public void Test_DeleteSammler_WithExistingSammler_ReturnsOkResult()
        {
            // Arrange
            long existingSammlerId = 1;

            // Act
            var result = _controller.DeleteSammler(existingSammlerId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        // Test for DeleteSammler method with non-existing Sammler
        [Fact]
        public void Test_DeleteSammler_WithNonExistingSammler_ReturnsNotFoundResult()
        {
            // Arrange
            long nonExistingSammlerId = 999;

            // Act
            var result = _controller.DeleteSammler(nonExistingSammlerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        //// Test for UpdateSammler method with existing Sammler
        [Fact]
        public void Test_UpdateSammler_WithExistingSammler_ReturnsOkResultWithUpdatedSammler()
        {
            // Arrange
            long existingSammlerId = 1;
            var sammlerUpdate = new SammlerUpdateData
            {
                Firstname = "UpdatedName",
                Surname = "UpdatedSurname",
                Nickname = "UpdatedNickname",
                Birthdate = new DateTime(1990, 5, 10), 
                Email = "updated@example.com",
                Telephone = "555-5678"
            };

            // Act
            var result = _controller.UpdateSammler(existingSammlerId, sammlerUpdate);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<Sammler>(okResult.Value);
            var updatedSammler = okResult.Value as Sammler;
            Assert.Equal("UpdatedName", updatedSammler.Firstname);
            Assert.Equal("UpdatedSurname", updatedSammler.Surname);
            Assert.Equal("UpdatedNickname", updatedSammler.Nickname);
            Assert.Equal(new DateTime(1990, 5, 10), updatedSammler.Birthdate); 
            Assert.Equal("updated@example.com", updatedSammler.Email);
            Assert.Equal("555-5678", updatedSammler.Telephone);
        }


        //// Test for UpdateSammler method with non-existing Sammler
        [Fact]
        public void Test_UpdateSammler_WithNonExistingSammler_ReturnsNotFoundResult()
        {
            // Arrange
            long nonExistingSammlerId = 999;
            var sammlerUpdate = new SammlerUpdateData
            {
                Firstname = "UpdatedName",
                Surname = "UpdatedSurname",
                Nickname = "UpdatedNickname",
                Birthdate = new DateTime(1990, 5, 10),
                Email = "updated@example.com",
                Telephone = "555-5678"
            };

            // Act
            var result = _controller.UpdateSammler(nonExistingSammlerId, sammlerUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        //// Test for GetAllOldtimer method
        [Fact]
        public void Test_GetAllOldtimer_ReturnsOkResultWithListOfOldtimer()
        {
            // Act
            var result = _controller.GetAllOldtimer();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<List<Car>>(okResult.Value);
            Assert.Equal(Mocks.CarsList.Count, (okResult.Value as List<Car>).Count);
        }

        //// Other test methods can be similarly added for the remaining endpoints.
    }
}
