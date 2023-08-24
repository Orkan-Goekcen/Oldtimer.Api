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
    }
}
