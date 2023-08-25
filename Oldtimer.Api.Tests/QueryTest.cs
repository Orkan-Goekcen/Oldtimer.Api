using Castle.Core.Logging;
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
        public async Task GetOldtimerBySammlerId_ReturnsOldtimerList()
        {
            // Arrange
            var sammlerId = 1;
            var expectedOldtimers = TestData.GetCarsTestData();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetOldtimerBySammlerIdQuery>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedOldtimers);

            // Act
            var actionResult = await sutCar.GetOldtimerBySammlerId(sammlerId);
            var result = actionResult.Result as OkObjectResult;
            var resultValue = result.Value as List<Car>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedOldtimers.Count, resultValue.Count);
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
    }
}
