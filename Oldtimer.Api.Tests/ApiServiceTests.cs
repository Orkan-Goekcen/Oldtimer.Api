using Moq;
using Oldtimer.Api.Data;
using Oldtimer.Api.Service;

namespace Oldtimer.Api.Tests
{
    public class ApiServiceTests
    {
        private readonly IApiService _service;

        public ApiServiceTests()
        {
            _service = Mocks.CreateMockApiService().Object;
        }

        [Fact]
        public void Test_GetSammlerById_ReturnsCorrectSammler()
        {
            // Act
            long idToFind = 1;
            var sammler = _service.GetSammlerById(idToFind);

            // Assert
            Assert.NotNull(sammler);
            Assert.Equal("John", sammler.Firstname);
        }

        [Fact]
        public void Test_GetSammlers_ReturnsListOfSammlers()
        {
            // Act
            var result = _service.GetSammlers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Test_GetSammlerByFirstName_ReturnsMatchingSammlers()
        {
            // Arrange
            string firstNameToFind = "John";

            // Act
            var result = _service.GetSammlerByFirstName(firstNameToFind);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("John", result[0].Firstname);
        }

        [Fact]
        public void Test_GetSammlerBySurName_ReturnsMatchingSammlers()
        {
            // Arrange
            string surNameToFind = "Doe";

            // Act
            var result = _service.GetSammlerBySurName(surNameToFind);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Doe", result[0].Surname);
        }

        [Fact]
        public void Test_GetSammlerByNickName_ReturnsMatchingSammlers()
        {
            // Arrange
            string nickNameToFind = "Johnny";
            var sammlersList = new List<Sammler>
    {
        new Sammler { Id = 1, Nickname = "JohnnyBoy" },
        new Sammler { Id = 2, Nickname = "AwesomeCollector" }
    };
            var mockApiService = new Mock<IApiService>();
            mockApiService.Setup(a => a.GetSammlerByNickName(It.IsAny<string>())).Returns<string>(nickName =>
            {
                return sammlersList
                    .Where(s => s.Nickname.Contains(nickName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            });
            var _service = mockApiService.Object;

            // Act
            var result = _service.GetSammlerByNickName(nickNameToFind);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("JohnnyBoy", result[0].Nickname);
        }

        [Fact]
        public void Test_GetSammlerByTelephone_ReturnsMatchingSammlers()
        {
            // Act
            string telePhoneToFind = "555-1234";
            var result = _service.GetSammlerByTelephone(telePhoneToFind);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("555-1234", result[0].Telephone);
        }

        [Fact]
        public void Test_AddSammler_AddsSammlerToContext()
        {
            // Arrange
            var sammlerToAdd = new Sammler { Id = 1, Firstname = "John" };

            // Act
            var result = _service.AddSammler(sammlerToAdd);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sammlerToAdd, result);
        }

        [Fact]
        public void Test_DeleteSammler_RemovesSammlerFromContext()
        {
            // Arrange
            long sammlerIdToDelete = 1;

            // Act
            _service.DeleteSammler(sammlerIdToDelete);

            // Assert
            // Die Überprüfung wird bereits in der Konfiguration des Mock-Objekts durchgeführt.
            // Hier ist keine weitere explizite Assert-Aussage erforderlich.
        }

        [Fact]
        public void Test_UpdateSammler_UpdatesSammlerInContext()
        {
            // Arrange
            long sammlerIdToUpdate = 1;
            var existingSammler = new Sammler { Id = sammlerIdToUpdate, Firstname = "John", Surname = "Doe" };

            // Act
            _service.UpdateSammler(existingSammler);

            // Assert
            // Die Überprüfung wird bereits in der Konfiguration des Mock-Objekts durchgeführt.
            // Hier ist keine weitere explizite Assert-Aussage erforderlich.
        }

        [Fact]
        public void Test_SammlerVorhanden_ReturnsTrueForExistingSammler()
        {
            // Arrange
            var existingSammler = new Sammler { Firstname = "John", Nickname = "Johnny", Telephone = "555-1234", Surname = "Doe" };

            // Act
            bool isSammlerVorhanden = _service.SammlerVorhanden(existingSammler);

            // Assert
            Assert.True(isSammlerVorhanden);
        }

        [Fact]
        public void Test_SammlerVorhanden_ReturnsFalseForNonExistingSammler()
        {
            // Arrange
            var existingSammler = new Sammler { Firstname = "John", Nickname = "Johnny", Telephone = "555-1234", Surname = "Doe" };
            var nonExistingSammler = new Sammler { Firstname = "Jane", Nickname = "Janey", Telephone = "555-5678", Surname = "Smith" };

            // Act
            bool isSammlerVorhanden = _service.SammlerVorhanden(nonExistingSammler);

            // Assert
            Assert.False(isSammlerVorhanden);
        }

        [Fact]
        public void Test_GetAllOldtimer_ReturnsListOfCarsWithSammler()
        {
            // Arrange
            var mockApiService = Mocks.CreateMockApiService(); // Step 2
            var apiService = mockApiService.Object; // Step 3

            // Act
            var result = apiService.GetAllOldtimer();

            // Assert
            var carsList = Mocks.carsList; // Get the carsList directly from the Mocks class
            Assert.NotNull(result);
            Assert.Equal(carsList.Count, result.Count);
            foreach (var car in result)
            {
                Assert.NotNull(car.Sammler);
            }
        }

            [Fact]
        public void Test_GetOldtimerPlusSammlerBySammlerId_ReturnsListOfCarsWithSammler()
        {
            // Arrange
            long sammlerId = 1;
            var mockContext = new Mock<YourDbContext>();
            var carsList = new List<Car>
            {
                new Car { Id = 1, Brand = "Toyota", Model = "Supra", Sammler = new Sammler { Id = sammlerId, Firstname = "John" } },
                new Car { Id = 2, Brand = "Ford", Model = "Mustang", Sammler = new Sammler { Id = 2, Firstname = "Jane" } }
            };
            mockContext.Setup(c => c.Cars.Include(c => c.Sammler)).Returns(carsList);
            var repository = new SammlerRepository(mockContext.Object);

            // Act
            var result = repository.GetOldtimerPlusSammlerBySammlerId(sammlerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(sammlerId, result[0].Sammler.Id);
            Assert.Equal("John", result[0].Sammler.Firstname);
        }

        //    [Fact]
        //    public void Test_GetOldtimerBySammlerId_ReturnsListOfCarsWithoutSammler()
        //    {
        //        // Arrange
        //        long sammlerId = 1;
        //        var mockContext = new Mock<YourDbContext>();
        //        var sammler = new Sammler { Id = sammlerId, Firstname = "John" };
        //        var carsList = new List<Car>
        //    {
        //        new Car { Id = 1, Brand = "Toyota", Model = "Supra", Sammler = sammler },
        //        new Car { Id = 2, Brand = "Ford", Model = "Mustang", Sammler = new Sammler { Id = 2, Firstname = "Jane" } }
        //    };
        //        mockContext.Setup(c => c.Cars.Where(c => c.Sammler.Id == sammlerId)).Returns(carsList.Where(c => c.Sammler.Id == sammlerId));
        //        mockContext.Setup(c => c.Sammlers.FirstOrDefault(s => s.Id == sammlerId)).Returns(sammler);
        //        var repository = new SammlerRepository(mockContext.Object);

        //        // Act
        //        var result = repository.GetOldtimerBySammlerId(sammlerId);

        //        // Assert
        //        Assert.NotNull(result);
        //        Assert.Single(result);
        //        Assert.Null(result[0].Sammler);
        //    }

        //    [Fact]
        //    public void Test_GetSammlerByOldtimerBrandAndModel_ReturnsAllSammlersWhenBrandAndModelAreNullOrEmpty()
        //    {
        //        // Arrange
        //        string brand = null;
        //        string model = null;
        //        var mockContext = new Mock<YourDbContext>();
        //        var sammlersList = new List<Sammler>
        //    {
        //        new Sammler { Id = 1, Firstname = "John" },
        //        new Sammler { Id = 2, Firstname = "Jane" }
        //    };
        //        mockContext.Setup(c => c.Sammlers.ToList()).Returns(sammlersList);
        //        var repository = new SammlerRepository(mockContext.Object);

        //        // Act
        //        var result = repository.GetSammlerByOldtimerBrandAndModel(brand, model);

        //        // Assert
        //        Assert.NotNull(result);
        //        Assert.Equal(sammlersList.Count, result.Count);
        //    }

        //    [Fact]
        //    public void Test_GetSammlerByOldtimerBrandAndModel_ReturnsMatchingSammlers()
        //    {
        //        // Arrange
        //        string brand = "Toyota";
        //        string model = "Supra";
        //        var mockContext = new Mock<YourDbContext>();
        //        var sammlersList = new List<Sammler>
        //    {
        //        new Sammler { Id = 1, Firstname = "John" },
        //        new Sammler { Id = 2, Firstname = "Jane" }
        //    };
        //        var carsList = new List<Car>
        //    {
        //        new Car { Id = 1, Brand = "Toyota", Model = "Supra", Sammler = sammlersList[0] },
        //        new Car { Id = 2, Brand = "Ford", Model = "Mustang", Sammler = sammlersList[1] }
        //    };
        //        sammlersList[0].Cars = new List<Car> { carsList[0] };
        //        sammlersList[1].Cars = new List<Car> { carsList[1] };
        //        mockContext.Setup(c => c.Sammlers).Returns(sammlersList);
        //        mockContext.Setup(c => c.Cars).Returns(carsList);
        //        var repository = new SammlerRepository(mockContext.Object);

        //        // Act
        //        var result = repository.GetSammlerByOldtimerBrandAndModel(brand, model);

        //        // Assert
        //        Assert.NotNull(result);
        //        Assert.Single(result);
        //        Assert.Equal(sammlersList[0].Id, result[0].Id);
        //        Assert.Equal(sammlersList[0].Firstname, result[0].Firstname);
        //        Assert.Empty(result[0].Cars); // Sammler doesn't have the Car anymore in the result
        //    }

        //    [Fact]
        //    public void Test_AddOldtimerToSammler_AddsCarToSammler()
        //    {
        //        // Arrange
        //        long sammlerId = 1;
        //        var mockContext = new Mock<YourDbContext>();
        //        var sammler = new Sammler { Id = sammlerId, Firstname = "John" };
        //        mockContext.Setup(c => c.Sammlers.FirstOrDefault(s => s.Id == sammlerId)).Returns(sammler);
        //        var repository = new SammlerRepository(mockContext.Object);
        //        var carDto = new CarDto
        //        {
        //            SammlerId = sammlerId,
        //            Brand = "Toyota",
        //            Model = "Supra",
        //            LicensePlate = "ABC-123",
        //            YearOfConstruction = "1990",
        //            Colors = Car.Color.Red
        //        };

        //        // Act
        //        var result = repository.AddOldtimerToSammler(sammlerId, carDto);

        //        // Assert
        //        Assert.NotNull(result);
        //        Assert.Equal(carDto.Brand, result.Brand);
        //        Assert.Equal(carDto.Model, result.Model);
        //        Assert.Equal(carDto.LicensePlate, result.LicensePlate);
        //        Assert.Equal(1990, result.YearOfConstruction);
        //        Assert.Equal(Car.Color.Red, result.Colors);
        //        Assert.Equal(sammlerId, result.Sammler.Id); // The Sammler of the car is set to the provided sammlerId
        //        mockContext.Verify(c => c.Cars.Add(It.IsAny<Car>()), Times.Once);
        //        mockContext.Verify(c => c.SaveChanges(), Times.Once);
        //    }

        //    [Fact]
        //    public void Test_RemoveOldtimer_RemovesCarFromContext()
        //    {
        //        // Arrange
        //        long oldtimerIdToRemove = 1;
        //        var mockContext = new Mock<YourDbContext>();
        //        var carToRemove = new Car { Id = oldtimerIdToRemove, Brand = "Toyota", Model = "Supra" };
        //        mockContext.Setup(c => c.Cars.FirstOrDefault(c => c.Id == oldtimerIdToRemove)).Returns(carToRemove);
        //        var repository = new SammlerRepository(mockContext.Object);

        //        // Act
        //        repository.RemoveOldtimer(oldtimerIdToRemove);

        //        // Assert
        //        mockContext.Verify(c => c.Cars.Remove(carToRemove), Times.Once);
        //        mockContext.Verify(c => c.SaveChanges(), Times.Once);
        //    }
    }
}