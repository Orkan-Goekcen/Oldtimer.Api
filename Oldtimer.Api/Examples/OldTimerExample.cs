using Oldtimer.Api.Data;
using Swashbuckle.AspNetCore.Filters;

namespace Oldtimer.Api.Examples
{
    public class OldTimerExample : IExamplesProvider<Car>
    {
        public Car GetExamples()
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
            
            // Example Car instance
            var car = new Car
            {
                Id = 101,
                Sammler = sammler,
                Brand = "Porsche",
                Model = "911",
                LicensePlate = "PORSCH123",
                YearOfConstruction = "1969",
                Colors = Car.Color.Red
            };

            return car;
        }
    }
}
