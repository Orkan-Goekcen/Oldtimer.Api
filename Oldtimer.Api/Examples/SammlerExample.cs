using Oldtimer.Api.Data;
using Swashbuckle.AspNetCore.Filters;

namespace Oldtimer.Api.Examples
{
    public class SammlerExample : IExamplesProvider<Sammler>
    {
        public Sammler GetExamples()
        {
            // Example Sammler instance
            var sammler = new Sammler
            {
                Id = 1,
                Surname = "Smith",
                Firstname = "John",
                Nickname = "Johnny",
                Birthdate = new DateTime(1985, 5, 15),
                Email = "john.smith@example.com",
                Telephone = "+1 555-1234"
            };

            // Example Car instances
            var car1 = new Car
            {
                Id = 101,
                Sammler = sammler,
                Brand = "Porsche",
                Model = "911",
                LicensePlate = "PORSCH123",
                YearOfConstruction = "1969",
                Colors = Car.Color.Red
            };

            var car2 = new Car
            {
                Id = 102,
                Sammler = sammler,
                Brand = "Ford",
                Model = "Mustang",
                LicensePlate = "MUST123",
                YearOfConstruction = "1970",
                Colors = Car.Color.Blue
            };

            // Set the Cars property of the Sammler instance
            sammler.Cars = new List<Car> { car1, car2 };

            return sammler;
        }
    }
}
