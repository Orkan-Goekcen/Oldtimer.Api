using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Oldtimer.Api.Data
{
    public class Sammler
    {
        [SwaggerSchema("The unique identifier of the Sammler.")]
        public long Id { get; set; }
        [SwaggerSchema("The surname of the Sammler.")]
        public string Surname { get; set; }
        [SwaggerSchema("The firstname of the Sammler.")]
        public string Firstname { get; set; }
        [SwaggerSchema("The Nickname of the Sammler.")]
        public string Nickname { get; set; }
        [SwaggerSchema("The date of birth of the Sammler.")]
        public DateTime Birthdate { get; set; }
        [SwaggerSchema("The E-Mail of the Sammler.")]
        public string Email { get; set; }
        [SwaggerSchema("The telephone number of the Sammler.")]
        public string Telephone { get; set; }
        [JsonIgnore]
        [SwaggerSchema("The oldtimer the Sammler owns.")]
        public List<Car> Cars { get; set; } = new List<Car>(); // Initialize as an empty list

    }
}