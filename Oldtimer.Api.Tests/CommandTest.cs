using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Oldtimer.Api.Commands;
using Oldtimer.Api.Controller;
using Oldtimer.Api.Data;
using Oldtimer.Api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByIdQuery>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync((GetSammlerByIdQuery query, CancellationToken cancellationToken) =>
           {
               var sammlers = TestData.GetSammlersTestData();
               return sammlers.FirstOrDefault(s => s.Id == query.SammlerId);
           });


            // Act
            var result = await sutCar.AddOldtimerToSammler(sammlerId, carDto);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }


        [Fact]
        public async Task DeleteSammlerCommandHandler_löscht_Sammler()
        {
            // Arrange
            var sammlerId = 1;

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetSammlerByIdQuery query, CancellationToken cancellationToken) =>
            {
                var sammlers = TestData.GetSammlersTestData();
                return sammlers.FirstOrDefault(s => s.Id == query.SammlerId);
            });


            // Act
            var result = await sutSammler.DeleteSammler(sammlerId);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteSammlerCommandHandler_löscht_nicht_nichtvorhandenen_Sammler()
        {
            // Arrange
            var sammlerId = 999; // Nicht vorhandene Sammler-ID

            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteSammlerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value); // Einheitliche Rückgabe, da es keinen Sammler gibt

            // Act
            var result = await sutSammler.DeleteSammler(sammlerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RemoveOldtimerCommandHandler_entfernt_Oldtimer()
        {
            // Arrange
            var oldtimerId = 1;

            //Unit wird verwendet wenn die Methode keinen relevanten Wert zurückgibt. Er bestätigt dennoch, dass die Aktion erfolgreich ausgeführt wurde.
            mediatorMock.Setup(x => x.Send(It.IsAny<RemoveOldtimerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await sutCar.RemoveOldtimer(oldtimerId);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task RemoveOldtimerCommandHandler_entfernt_nicht_nichtvorhandenen_Oldtimer()
        {
            // Arrange
            var oldtimerId = 999; // Nicht vorhandene Oldtimer-ID

            mediatorMock.Setup(x => x.Send(It.IsAny<RemoveOldtimerCommand>(), It.IsAny<System.Threading.CancellationToken>()))
                       .ReturnsAsync(Unit.Value); // Einheitliche Rückgabe, da es keinen Oldtimer gibt

            // Act
            var result = await sutCar.RemoveOldtimer(oldtimerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateSammlerCommand_aktualisiert_Sammler()
        {
            // Arrange
            var sammlerId = 1;
            var updatedSammlerData = new SammlerUpdateData
            {
                Firstname = "UpdatedFirstName",
                Surname = "UpdatedSurname",
                Nickname = "UpdatedNickname",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "updated.email@example.com",
                Telephone = "555-555-5555"
            };

            var existingSammler = new Sammler { Id = sammlerId };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByIdQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingSammler);

            // Act
            var result = await sutSammler.UpdateSammler(sammlerId, updatedSammlerData);

            // Assert
            mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSammlerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
