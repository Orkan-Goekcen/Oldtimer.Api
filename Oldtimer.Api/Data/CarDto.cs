namespace Oldtimer.Api.Data
{
    public class CarDto
    {
        public long SammlerId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public string YearOfConstruction { get; set; }
        public Car.Color Colors { get; set; }
    }
}