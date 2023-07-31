using Swashbuckle.AspNetCore.Annotations;

namespace Oldtimer.Api.Data
{
    public class Car
    {
        [SwaggerSchema("The unique identifier of the Oldtimer.")]
        public long Id { get; set; }

        [SwaggerSchema("The Sammler who owns the Oldtimer.")]
        public Sammler? Sammler { get; set; } // Verweis auf den Sammler

        [SwaggerSchema("The brand of the Oldtimer.")]
        public string Brand { get; set; }

        [SwaggerSchema("The model of the Oldtimer.")]
        public string Model { get; set; }

        [SwaggerSchema("The unique license plate of the Oldtimer.")]
        public string LicensePlate { get; set; }

        [SwaggerSchema("The year of construction of the Oldtimer.")]
        public string YearOfConstruction { get; set; }

        [SwaggerSchema("The color of the Oldtimer.")]
        public Color Colors { get; set; }

        public enum Color
        {
            Red = 1,
            Blue = 2,
            Green = 3,
            Yellow = 4,
            Black = 5,
            White = 6
        }
    }
}