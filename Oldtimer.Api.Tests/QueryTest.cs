﻿using Castle.Core.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MySqlX.XDevAPI.Common;
using Oldtimer.Api.Controller;
using Oldtimer.Api.Data;
using Oldtimer.Api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Oldtimer.Api.Tests
{
    public class QueryTest
    {
        private readonly OldtimerApiController sutCar;
        private readonly SammlerApiController sutSammler;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<ILogger> loggerMock;

        public QueryTest()
        {
            mediatorMock = new Mock<IMediator>();
            loggerMock = new Mock<ILogger>();

            sutCar = new OldtimerApiController(mediatorMock.Object);
            sutSammler = new SammlerApiController(mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllOldtimer_liefert_alle_Oldtimer()
        {
            // Arrange
            var cars = TestData.GetCarsTestData();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAllOldtimerQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cars);

            // Act
            var result = await sutCar.GetAllOldtimer();


            // Assert
            Assert.Equal(cars.Count, result.Value.Count);
        }

        [Fact]
        public async Task GetAllOldtimer_returns_empty_list()   
        {
            // Arrange
            var emptyList = new List<Car>(); 

            mediatorMock.Setup(x => x.Send(It.IsAny<GetAllOldtimerQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(emptyList); 

            // Act
            var result = await sutCar.GetAllOldtimer();

            // Assert
            Assert.Equal(emptyList.Count, result.Value.Count);
        }

        [Fact]
        public async Task GetSammlersQuery_liefert_alle_Sammler()
        {
            // Arrange
            var sammlers = TestData.GetSammlersTestData();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sammlers);

            // Act
            var result = await sutSammler.GetSammlers();

            // Assert
            Assert.Equal(sammlers.Count, result.Value.Count);
        }

        [Fact]
        public async Task GetSammlers_returns_empty_list_on_failure()
        {
            // Arrange
            var emptyList = new List<Sammler>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlersQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(emptyList);

            // Act
            var result = await sutSammler.GetSammlers();

            // Assert
            Assert.Equal(emptyList.Count, result.Value.Count);
        }

        [Fact]
        public async Task GetSammlerById_returns_ok_on_existing_sammler()
        {
            // Arrange
            var sammlers = TestData.GetSammlersTestData(); // Testdaten laden
            var sammler = sammlers[0]; // Nehme den ersten Sammler als Beispiel

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(sammler);

            // Act
            var result = await sutSammler.GetSammlerById(sammler.Id); // Verwende die ID des Beispiel-Sammlers

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<Sammler>(okResult.Value);
            Assert.Equal(sammler.Id, model.Id);
        }

        [Fact]
        public async Task GetSammlerById_returns_not_found_on_non_existing_sammler()
        {
            // Arrange
            var sammlers = TestData.GetSammlersTestData(); // Testdaten laden
            var nonExistingId = 9999; // Beispiel für eine nicht vorhandene ID

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Sammler)null); // null zurückgeben

            // Act
            var result = await sutSammler.GetSammlerById(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetSammlerByFirstName_ReturnsMatchingSammlerList()
        {
            // Arrange
            var firstName = "Frank";
            var expectedSammlers = TestData.GetSammlersTestData()
                .Where(s => s.Firstname == firstName)
                .ToList();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByFirstNameQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedSammlers);

            // Act
            var actionResult = await sutSammler.GetSammlerByFirstName(firstName);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            var resultValue = result.Value as List<Sammler>;
            Assert.NotNull(result);
            Assert.NotNull(resultValue);
            Assert.Equal(expectedSammlers.Count, resultValue.Count);
            Assert.All(resultValue, sammler => Assert.Equal(firstName, sammler.Firstname));
        }

        [Fact]
        public async Task GetSammlerByFirstName_returns_not_found_on_empty_list()
        {
            // Arrange
            var emptyList = new List<Sammler>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByFirstNameQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(emptyList);

            // Act
            var result = await sutSammler.GetSammlerByFirstName("Kahi"); // Beispiel-Vorname

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Sammler>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal($"No Sammler found with the first name: Kahi", notFoundResult.Value);
        }

        [Fact]
        public async Task GetSammlerBySurName_ReturnsMatchingSammlerList()
        {
            // Arrange
            var surName = "Hansemann";
            var sammlers = TestData.GetSammlersTestData();
            var expectedSammlers = sammlers.Where(s => s.Surname == surName).ToList();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerBySurNameQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedSammlers);

            // Act
            var actionResult = await sutSammler.GetSammlerBySurName(surName);

            // Assert
            var resultValue = actionResult.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(expectedSammlers.Count, resultValue.Count);
            Assert.All(resultValue, sammler => Assert.Equal(surName, sammler.Surname));
        }

        [Fact]
        public async Task GetSammlerBySurName_returns_not_found_on_empty_list()
        {
            // Arrange
            var emptyList = new List<Sammler>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerBySurNameQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(emptyList);

            // Act
            var result = await sutSammler.GetSammlerBySurName("hshshshshshshshshshshshshs"); // Beispiel-Nachname

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Sammler>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal($"No Sammler found with the Surname: hshshshshshshshshshshshshs", notFoundResult.Value);
        }

        [Fact]
        public async Task GetSammlerByNickName_ReturnsMatchingSammlerList()
        {
            // Arrange
            var nickName = "Hanso";
            var sammlers = TestData.GetSammlersTestData();
            var expectedSammlers = sammlers.Where(s => s.Nickname == nickName).ToList();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByNickNameQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedSammlers);

            // Act
            var actionResult = await sutSammler.GetSammlerByNickName(nickName);

            // Assert
            var resultValue = actionResult.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(expectedSammlers.Count, resultValue.Count);
            Assert.All(resultValue, sammler => Assert.Equal(nickName, sammler.Nickname));
        }

        [Fact]
        public async Task GetSammlerByNickName_returns_not_found_on_empty_list()
        {
            // Arrange
            var emptyList = new List<Sammler>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByNickNameQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(emptyList);

            // Act
            var result = await sutSammler.GetSammlerByNickName("johndoe"); 

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Sammler>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal($"No Sammler found with the Nickname: johndoe", notFoundResult.Value);
        }

        [Fact]
        public async Task GetSammlerByTelePhone_ReturnsMatchingSammlerList()
        {
            // Arrange
            var telePhone = "12424242432314";
            var sammlers = TestData.GetSammlersTestData();
            var expectedSammlers = sammlers.Where(s => s.Telephone == telePhone).ToList();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByTelephoneQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedSammlers);

            // Act
            var actionResult = await sutSammler.GetSammlerByTelephone(telePhone);

            // Assert
            var resultValue = actionResult.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(expectedSammlers.Count, resultValue.Count);
            Assert.All(resultValue, sammler => Assert.Equal(telePhone, sammler.Telephone));
        }

        [Fact]
        public async Task GetSammlerByTelephone_returns_not_found_on_empty_list()
        {
            // Arrange
            var emptyList = new List<Sammler>();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlerByTelephoneQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(emptyList);

            // Act
            var result = await sutSammler.GetSammlerByTelephone("123456789"); 

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Sammler>>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal($"No Sammler found with the Telephone number: 123456789", notFoundResult.Value);
        }

        [Fact]
        public async Task GetSammlerDetails_Returns_OkResult_With_Valid_Id()
        {
            // Arrange
            long sammlerId = 1;
            var sammler = TestData.GetSammlersTestData().FirstOrDefault(s => s.Id == sammlerId);
            var cars = TestData.GetCarsTestData().Where(c => c.Sammler.Id == sammlerId).ToList();

            mediatorMock.Setup(x => x.Send(It.IsAny<SammlerDetailsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new SammlerDetailsResponse { Sammler = sammler, Oldtimer = cars });

            // Act
            var result = await sutSammler.GetSammlerDetails(sammlerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<SammlerDetailsResponse>(okResult.Value);
            Assert.Equal(sammlerId, model.Sammler.Id);
            Assert.Equal(sammler.Surname, model.Sammler.Surname);
            Assert.Equal(sammler.Firstname, model.Sammler.Firstname);
            Assert.Equal(cars.Count, model.Oldtimer.Count);
        }

        [Fact]
        public async Task GetSammlerDetails_Returns_NotFound_With_Invalid_Id()
        {
            // Arrange
            long invalidSammlerId = 999; // Beispiel für eine ungültige ID
            mediatorMock.Setup(x => x.Send(It.IsAny<SammlerDetailsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((SammlerDetailsResponse)null); // null zurückgeben, um den nicht gefundenen Sammler zu simulieren

            // Act
            var result = await sutSammler.GetSammlerDetails(invalidSammlerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
