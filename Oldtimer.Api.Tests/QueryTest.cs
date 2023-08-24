using Castle.Core.Logging;
using MediatR;
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
        public async Task GetSammlersQuery_liefert_alle_Sammler()
        {
            // Arrange
            var sammlers = TestData.GetSammlersTestData();

            mediatorMock.Setup(x => x.Send(It.IsAny<GetSammlersQuery>(), It.IsAny<CancellationToken>()));

            // Act
            var result = await sutSammler.GetSammlers();
            // Assert
            Assert.Equal(sammlers.Count, result.Value.Count);
        }

        [Fact]
        public async Task SammlerVorhandenQueryHandler_ReturnsTrue_WhenMatchingSammlerExists()
        {
            // Arrange
            var sammlerToCheck = new Sammler
            {
                Id = 3,
                Surname = "Test",
                Firstname = "Tester",
                Nickname = "Testo",
                Telephone = "1234567890"
            };

            var existingSammler = new Sammler
            {
                Id = 1,
                Surname = "Existing",
                Firstname = "Exister",
                Nickname = "Existo",
                Telephone = "9876543210"
            };

            var sammlers = TestData.GetSammlersTestData();
            sammlers.Add(existingSammler);

            var apiContextMock = TestData.GetApiContextMockForSammlers(sammlers);
            var queryHandler = new SammlerVorhandenQueryHandler(apiContextMock.Object);

            var query = new SammlerVorhandenQuery
            {
                NeuerSammler = sammlerToCheck
            };

            // Act
            var result = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SammlerVorhandenQueryHandler_ReturnsFalse_WhenNoMatchingSammlerExists()
        {
            // Arrange
            var sammlerToCheck = new Sammler
            {
                Id = 3,
                Surname = "Test",
                Firstname = "Tester",
                Nickname = "Testo",
                Telephone = "1234567890"
            };

            var sammlers = TestData.GetSammlersTestData();
            var apiContextMock = TestData.GetApiContextMockForSammlers(sammlers);
            var queryHandler = new SammlerVorhandenQueryHandler(apiContextMock.Object);

            var query = new SammlerVorhandenQuery
            {
                NeuerSammler = sammlerToCheck
            };

            // Act
            var result = await queryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result);
        }
    }
}  
