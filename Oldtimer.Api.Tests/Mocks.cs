﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Moq;
using Oldtimer.Api.Data;

namespace Oldtimer.Api.Tests.Mocks
{
    public static class MockData
    {
        public static IQueryable<Car> GetMockCarsData()
        {
            var cars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Brand = "Porsche",
                    Model = "911",
                    LicensePlate = "PORSCH123",
                    YearOfConstruction = "1969",
                    Colors = Car.Color.Red,
                    Sammler = null
                },
                new Car
                {
                    Id = 2,
                    Brand = "Ford",
                    Model = "Mustang",
                    LicensePlate = "MUST123",
                    YearOfConstruction = "1970",
                    Colors = Car.Color.Blue,
                    Sammler = null
                },
            };

            return cars.AsQueryable();
        }

        public static ApiContext GetMockApiContext()
        {
            var mockContext = new Mock<ApiContext>();

            var mockCarsData = GetMockCarsData();
            var mockDbSet = new Mock<DbSet<Car>>();
            mockDbSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(mockCarsData.Provider);
            mockDbSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(mockCarsData.Expression);
            mockDbSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(mockCarsData.ElementType);
            mockDbSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(mockCarsData.GetEnumerator());

            mockContext.Setup(c => c.Cars).Returns(mockDbSet.Object);

            return mockContext.Object;
        }
    }
}