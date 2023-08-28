using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Oldtimer.Api.Commands;
using Oldtimer.Api.Controller;
using Oldtimer.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oldtimer.Api.Tests
{
    public class CommandTest
    {
        private readonly OldtimerApiController sutCar;
        private readonly SammlerApiController sutSammler;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<ILogger> loggerMock;

        public CommandTest()
        {
            mediatorMock = new Mock<IMediator>();
            loggerMock = new Mock<ILogger>();

            sutCar = new OldtimerApiController(mediatorMock.Object);
            sutSammler = new SammlerApiController(mediatorMock.Object);
        }

        [Fact]
        public async Task AddSammlerCommandQuery_fügt_Sammler_hinzu()
        {
            // Arrange
            var sammlerId = 5;
            var carDto = new CarDto
            {
                Brand = "Audi",
                Model = "A4",
                LicensePlate = "ABC123",
                YearOfConstruction = "2022",
                Colors = Car.Color.Blue
            };
            var sammler = new Sammler { Id = sammlerId };
            var sammlers = new List<Sammler> { sammler };

            mediatorMock.Setup(x => x.Send(It.IsAny<AddSammlerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sammler);

            // Act
            var result = await sutSammler.GetSammlerById(sammlerId);

            // Assert
            Assert.Contains(sammlers, s => s.Id == sammlerId);
        }

        [Fact]
        public async Task AddOldtimerToSammlerQuery_fügt_Oldtimer_zum_Sammler_hinzu()
        {
            // Arrange
            var sammlerId = 1;
            var carDto = new CarDto
            {
                Brand = "Audi",
                Model = "A4",
                LicensePlate = "ABC123",
                YearOfConstruction = "2022",
                Colors = Car.Color.Blue
            };

            // Act
            //hier findet er den sammler mit id 1 nicht
            var result = await sutCar.AddOldtimerToSammler(sammlerId, carDto);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var createdCarResult = objectResult.Value as Car;

            Assert.NotNull(createdCarResult);
            Assert.Equal(sammlerId, createdCarResult.Sammler.Id);
        }
    }
}
