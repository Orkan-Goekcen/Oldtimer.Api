using Castle.Core.Logging;
using Dasync.Collections;
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
        //private readonly GetAllOldtimerQuery sut;
        //private readonly Mock<IMediator> mediatorMock;
        //private readonly Mock<ILogger> loggerMock;

        //public QueryTest()
        //{
        //    mediatorMock = new Mock<IMediator>();
        //    loggerMock = new Mock<ILogger>();

        //    sut = new GetAllOldtimerQuery();
        //}

        [Fact]
        public async Task GetAllOldtimer_liefert_alle_Oldtimer()
        {
            // Arrange
            var cars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Sammler = new Sammler
                    {
                        Id = 1,
                        Surname = "Frankenstein",
                        Firstname = "Frank",
                        Nickname = "Franko",
                        Birthdate = new DateTime(1997, 8, 15),
                        Email = "Frank.Franko@gmail.com",
                        Telephone = "12424242432314"
                    },
                    Brand = "Audi",
                    Model = "A6",
                    LicensePlate ="HI AB5255",
                    YearOfConstruction = "2020",
                    Colors = Car.Color.Red
                },

                new Car
                {
                    Id = 1,
                    Sammler = new Sammler
                    {
                        Id = 1,
                        Surname = "Hansemann",
                        Firstname = "Hans",
                        Nickname = "Hanso",
                        Birthdate = new DateTime(1966, 4, 16),
                        Email = "Hans.Hanso@gmail.com",
                        Telephone = "4446366663"
                    },
                    Brand = "Mercedes",
                    Model = "CL500",
                    LicensePlate ="H KJ3033",
                    YearOfConstruction = "2018",
                    Colors = Car.Color.Blue
                },
            };

            var queryableCars = cars.AsQueryable(); // Convert the list to a queryable

            var apiContextMock = new Mock<IApiContext>(); // Use IApiContext interface here
            var dbSetMock = new Mock<DbSet<Car>>();

            // Adjust the setup for async operations
            dbSetMock.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(queryableCars.Provider);
            dbSetMock.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(queryableCars.Expression);
            dbSetMock.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(queryableCars.ElementType);
            dbSetMock.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(queryableCars.GetEnumerator());

            // Set up async behavior using System.Linq.Async.ToAsyncEnumerable
            var asyncCars = queryableCars.ToAsyncEnumerable();
            dbSetMock.As<IAsyncEnumerable<Car>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(asyncCars.GetAsyncEnumerator());

            // Set up the context to return the mock DbSet
            apiContextMock.Setup(c => c.Cars).Returns(dbSetMock.Object); // Use Cars property from the interface

            var sut = new GetAllOldtimerQueryHandler(apiContextMock.Object);

            // Act
            var result = await sut.Handle(new GetAllOldtimerQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(cars.Count, result.Count);
        }
    }
}
