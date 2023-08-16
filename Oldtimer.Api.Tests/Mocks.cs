using Moq;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Linq;

namespace Oldtimer.Tests.Mocks
{
    public static class ApiServiceMocks
    {
        public static Mock<IApiService> CreateMockApiService()
        {
            var mockApiService = new Mock<IApiService>();

            // Führe hier alle Setups für die verschiedenen Methoden deiner ApiService-Schnittstelle durch
            SetupGetAllOldtimerQuery(mockApiService);

            // Füge hier weitere Setups hinzu

            return mockApiService;
        }

        private static void SetupGetAllOldtimerQuery(Mock<IApiService> mockApiService)
        {
            var carsList = new List<Car>
            {
                new Car { Id = 1, Brand = "Toyota", Model = "Supra" },
                new Car { Id = 2, Brand = "Ford", Model = "Mustang" }
                // Füge hier weitere Testdaten hinzu
            };

            mockApiService.Setup(a => a.GetAllOldtimer())
                          .Returns(carsList);
        }

        // Füge hier weitere Methoden zum Setup von anderen Abfragen hinzu
    }
}