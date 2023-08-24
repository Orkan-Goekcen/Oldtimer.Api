using Dasync.Collections;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Linq;

namespace Oldtimer.Api.Tests
{
    public static class TestData
    {
        public static List<Car> GetCarsTestData()
        {
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
                    Id = 2,
                    Sammler = new Sammler
                    {
                        Id = 2,
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
            return cars;
        }

        public static Mock<IApiContext> GetApiContextMock(List<Car> cars)
        {
            var queryableCars = cars.AsQueryable();
            var dbSetMock = new Mock<DbSet<Car>>();

            dbSetMock.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(queryableCars.Provider);
            dbSetMock.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(queryableCars.Expression);
            dbSetMock.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(queryableCars.ElementType);
            dbSetMock.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(queryableCars.GetEnumerator());

            dbSetMock.As<IAsyncEnumerable<Car>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Car>(queryableCars.GetEnumerator()));

            var apiContextMock = new Mock<IApiContext>();
            apiContextMock.Setup(c => c.Cars).Returns(dbSetMock.Object);
            return apiContextMock;
        }

        public static List<Sammler> GetSammlersTestData()
        {
            var sammlers = new List<Sammler>
        {
            new Sammler
            {
                Id = 1,
                Surname = "Frankenstein",
                Firstname = "Frank",
                Nickname = "Franko",
                Birthdate = new DateTime(1997, 8, 15),
                Email = "Frank.Franko@gmail.com",
                Telephone = "12424242432314"
            },
            new Sammler
            {
                Id = 2,
                Surname = "Hansemann",
                Firstname = "Hans",
                Nickname = "Hanso",
                Birthdate = new DateTime(1966, 4, 16),
                Email = "Hans.Hanso@gmail.com",
                Telephone = "4446366663"
            }
        };
            return sammlers;
        }

        public static Mock<IApiContext> GetApiContextMockForSammlers(List<Sammler> sammlers)
        {
            var queryableSammlers = sammlers.AsQueryable();
            var dbSetMock = new Mock<DbSet<Sammler>>();

            dbSetMock.As<IQueryable<Sammler>>().Setup(m => m.Provider).Returns(queryableSammlers.Provider);
            dbSetMock.As<IQueryable<Sammler>>().Setup(m => m.Expression).Returns(queryableSammlers.Expression);
            dbSetMock.As<IQueryable<Sammler>>().Setup(m => m.ElementType).Returns(queryableSammlers.ElementType);
            dbSetMock.As<IQueryable<Sammler>>().Setup(m => m.GetEnumerator()).Returns(queryableSammlers.GetEnumerator());

            dbSetMock.As<IAsyncEnumerable<Sammler>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Sammler>(queryableSammlers.GetEnumerator()));

            var apiContextMock = new Mock<IApiContext>();
            apiContextMock.Setup(c => c.Sammlers).Returns(dbSetMock.Object);
            return apiContextMock;
        }
    }
}
