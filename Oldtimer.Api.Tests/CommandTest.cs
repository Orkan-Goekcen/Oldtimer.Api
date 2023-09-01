using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Oldtimer.Api.Commands;
using Oldtimer.Api.Controller;
using Oldtimer.Api.Data;
using Oldtimer.Api.Queries;
using Oldtimer.Api.Validation;
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
            var neuerSammler = new Sammler
            {
                Id = sammlerId,
                Firstname = "John",
                Surname = "Doe",
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<AddSammlerCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(neuerSammler);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(neuerSammler); // Mock das Abrufen des Sammlers

            // Act
            var result = await sutSammler.GetSammlerById(sammlerId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            // IsAssignableFrom<Sammler> prüft ob das Objekt vom Typ Sammler abgeleitet oder implementiert wird
            var addedSammler = Assert.IsAssignableFrom<Sammler>(objectResult.Value);
            Assert.Equal(sammlerId, addedSammler.Id);
        }



        [Fact]
        public async Task AddOldtimerToSammlerQuery_fügt_Oldtimer_zum_Sammler_hinzu()
        {
            // Arrange
            var sammlerId = 1;

            var carDto = new CarDto
            {
                SammlerId = 1,
                Brand = "Audi",
                Model = "A4",
                LicensePlate = "ABC123",
                YearOfConstruction = "1990",
                Colors = Car.Color.Blue
            };

            var expectedValidationResult = new ValidationResult(); // Erwartetes ValidationResult

            var validatorMock = new Mock<IValidator<CarDto>>();
            validatorMock
                .Setup(x => x.ValidateAsync(carDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedValidationResult);

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetSammlerByIdQuery query, CancellationToken cancellationToken) =>
                {
                    var sammlers = TestData.GetSammlersTestData();
                    return sammlers.FirstOrDefault(s => s.Id == query.SammlerId);
                });

            // Act
            var result = await sutCar.AddOldtimerToSammler(sammlerId, carDto, validatorMock.Object);

            // Assert
            validatorMock.Verify(x => x.ValidateAsync(carDto, It.IsAny<CancellationToken>()), Times.Once);
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
                       .ReturnsAsync(true);

            // Act
            var result = await sutCar.RemoveOldtimer(oldtimerId);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task RemoveOldtimerCommandHandler_entfernt_nicht_nichtvorhandenen_Oldtimer()
        {
            // Arrang
            var oldtimerId = 999; // Nicht vorhandene Oldtimer-ID

            mediatorMock.Setup(x => x.Send(It.IsAny<RemoveOldtimerCommand>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(false);

            // Act
            var result = await sutCar.RemoveOldtimer(oldtimerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateSammlerCommand_aktualisiert_Sammler()
        {
            // Arrange
            var validatorMock = new Mock<SammlerUpdateDataValidator>();

            var sammlerId = 5;
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
            var result = await sutSammler.UpdateSammler(sammlerId, updatedSammlerData, validatorMock.Object);

            // Assert
            mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSammlerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateSammlerCommand_ungültige_ID()
        {
            // Arrange
            var validatorMock = new Mock<SammlerUpdateDataValidator>();

            var sammlerId = 999; // Nicht vorhandene Sammler-ID
            var updatedSammlerData = new SammlerUpdateData
            {
                Firstname = "UpdatedFirstName",
                Surname = "UpdatedSurname",
                Nickname = "UpdatedNickname",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "updated.email@example.com",
                Telephone = "555-555-5555"
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByIdQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Sammler)null); // Kein Sammler mit der ID vorhanden

            // Act
            var result = await sutSammler.UpdateSammler(sammlerId, updatedSammlerData, validatorMock.Object);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<UpdateSammlerCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
