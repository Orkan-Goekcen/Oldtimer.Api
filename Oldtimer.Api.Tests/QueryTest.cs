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
    }
}
