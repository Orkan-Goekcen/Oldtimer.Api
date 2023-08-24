using Castle.Core.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using MySqlX.XDevAPI.Common;
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
        private readonly GetAllOldtimerQuery sut;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<ILogger> loggerMock;

        public QueryTest()
        {
            mediatorMock = new Mock<IMediator>();
            loggerMock = new Mock<ILogger>();

            sut = new GetAllOldtimerQuery();
        }

        [Fact]
        public async Task GetAllOldtimer_liefert_alle_Oldtimer()
        {
            // Arrange
            var cars = TestData.GetCarsTestData();
            var apiContextMock = TestData.GetApiContextMock(cars);

            var sut = new GetAllOldtimerQueryHandler(apiContextMock.Object);

            // Act
            var result = await sut.Handle(new GetAllOldtimerQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(cars.Count, result.Count);
        }
        [Fact]
        public async Task GetSammlersQuery_liefert_alle_Sammler()
        {
            // Arrange
            var sammlers = TestData.GetSammlersTestData();
            var apiContextMock = TestData.GetApiContextMockForSammlers(sammlers);
            var queryHandler = new GetSammlersQueryHandler(apiContextMock.Object);

            // Act
            var result = await queryHandler.Handle(new GetSammlersQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(sammlers.Count, result.Count);
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
