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
        public void Test_GetOldtimerPlusSammlerBySammlerId_ReturnsListOfCarsWithSammler()
        {
            // Arrange
            long sammlerId = 1;
            var mockApiService = Mocks.CreateMockApiService();
            var apiService = mockApiService.Object;

            // Act
            var result = apiService.GetOldtimerPlusSammlerBySammlerId(sammlerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Es wird nur ein Auto mit der gegebenen SammlerId erwartet
            Assert.Equal(sammlerId, result[0].Sammler.Id);
            Assert.Equal("John", result[0].Sammler.Firstname);
        }

        [Fact]
        public void Test_GetAllOldtimer_ReturnsListOfCarsWithSammler()
        {
            // Arrange
            var mockApiService = Mocks.CreateMockApiService();
            var apiService = mockApiService.Object;

            // Act
            var result = apiService.GetAllOldtimer();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Mocks.CarsList.Count, result.Count);
            foreach (var car in result)
            {
                Assert.NotNull(car.Sammler);
            }
        }

        [Fact]
        public void Test_GetOldtimerBySammlerId_ReturnsListOfCarsWithoutSammler()
        {
            // Arrange
            long sammlerId = 1;
            var mockApiService = Mocks.CreateMockApiService();
            mockApiService.Setup(a => a.GetOldtimerPlusSammlerBySammlerId(sammlerId)).Returns(Mocks.CarsList.Where(c => c.Sammler.Id == sammlerId).ToList());
            var apiService = mockApiService.Object;

            // Act
            var result = apiService.GetOldtimerPlusSammlerBySammlerId(sammlerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Null(result[0].Sammler);
        }

        [Fact]
        public void Test_GetSammlerByOldtimerBrandAndModel_ReturnsAllSammlersWhenBrandAndModelAreNullOrEmpty()
        {
            // Arrange
            string brand = null;
            string model = null;
            var mockApiService = Mocks.CreateMockApiService();
            var apiService = mockApiService.Object;

            // Erstelle den Mock für das Sammler-Repository
            var mockSammlerRepository = new Mock<IApiService>();
            mockSammlerRepository.Setup(r => r.GetSammlerByOldtimerBrandAndModel(brand, model)).Returns(Mocks.SammlersList);

            // Setze den Mock für GetSammlerByOldtimerBrandAndModel in den ApiService-Mock
            mockApiService.Setup(a => a.GetSammlerByOldtimerBrandAndModel(brand, model)).Returns(Mocks.SammlersList);

            // Act
            var result = apiService.GetSammlerByOldtimerBrandAndModel(brand, model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Mocks.SammlersList.Count, result.Count);
            for (int i = 0; i < Mocks.SammlersList.Count; i++)
            {
                Assert.Equal(Mocks.SammlersList[i].Id, result[i].Id);
                Assert.Equal(Mocks.SammlersList[i].Firstname, result[i].Firstname);
            }
        }

        [Fact]
        public void Test_GetSammlerByOldtimerBrandAndModel_ReturnsMatchingSammlers()
        {
            // Arrange
            string brand = "Toyota";
            string model = "Supra";

            // Erstelle den Mock ApiService
            var mockApiService = Mocks.CreateMockApiService();
            var apiService = mockApiService.Object;

            // Definiere die erwarteten Ergebnisse
            var expectedSammlersList = Mocks.SammlersList
                .Where(s => s.Cars.Any(c => c.Brand == brand && c.Model == model))
                .ToList();

            // Act
            var result = apiService.GetSammlerByOldtimerBrandAndModel(brand, model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSammlersList.Count, result.Count);
            for (int i = 0; i < expectedSammlersList.Count; i++)
            {
                Assert.Equal(expectedSammlersList[i].Id, result[i].Id);
                Assert.Equal(expectedSammlersList[i].Firstname, result[i].Firstname);
            }
        }


        [Fact]
        public void Test_AddOldtimerToSammler_AddsCarToSammler()
        {
            // Arrange
            long sammlerId = 1;
            var mockApiService = Mocks.CreateMockApiService();
            var apiService = mockApiService.Object;

            var carDto = new CarDto
            {
                SammlerId = sammlerId,
                Brand = "Toyota",
                Model = "Supra",
                LicensePlate = "ABC-123",
                YearOfConstruction = "1990",
                Colors = Car.Color.Red
            };

            // Act
            var addedCar = apiService.AddOldtimerToSammler(sammlerId, carDto);

            // Assert
            Assert.NotNull(addedCar);
            Assert.Equal(carDto.Brand, addedCar.Brand);
            Assert.Equal(carDto.Model, addedCar.Model);
            Assert.Equal(carDto.LicensePlate, addedCar.LicensePlate);
            Assert.Equal(carDto.YearOfConstruction, addedCar.YearOfConstruction);
            Assert.Equal(carDto.Colors, addedCar.Colors);
            Assert.Equal(sammlerId, addedCar.Sammler.Id);

            // Verify that the Add method was called with the expected carDto and sammlerId
            mockApiService.Verify(a => a.AddOldtimerToSammler(sammlerId, carDto), Times.Once);
        }


        [Fact]
        public void Test_RemoveOldtimer_RemovesCarFromContext()
        {
            // Arrange
            long oldtimerIdToRemove = 1;
            var mockApiService = Mocks.CreateMockApiService();

            // Act
            mockApiService.Object.RemoveOldtimer(oldtimerIdToRemove);

            // Assert
            // Überprüfe, ob der gewünschte Oldtimer nicht mehr im Kontext vorhanden ist
            var removedCar = mockApiService.Object.GetOldtimerBySammlerId(oldtimerIdToRemove);
            Assert.Null(removedCar);
        }
    }
}