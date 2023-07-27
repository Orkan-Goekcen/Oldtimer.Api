using Microsoft.AspNetCore.Mvc;
using Oldtimer.Api.Data;
using Oldtimer.Api.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace Oldtimer.Api.Controller
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly IApiService _service;

        public ApiController(IApiService service)
        {
            _service = service;
        }

        [HttpGet("Sammler")]
        [SwaggerOperation("Get all Sammlers")]
        public IActionResult GetSammlers()
        {
            var sammlers = _service.GetSammlers();
            return Ok(sammlers);
        }

        [HttpGet("Sammler/{id}")]
        [SwaggerOperation("Get Sammler by ID")]
        public IActionResult GetSammlerById(long id)
        {
            var sammler = _service.GetSammlerById(id);
            if (sammler == null)
            {
                return NotFound();
            }

            return Ok(sammler);
        }

        [HttpPost("Sammler")]
        [SwaggerOperation("Create Sammler")]
        public ActionResult<Sammler> CreateSammler([FromBody] Sammler neuerSammler)
        {
            var sammlerBereitsVorhanden = _service.SammlerVorhanden(neuerSammler);

            if (sammlerBereitsVorhanden)
            {
                return Conflict(neuerSammler);
            }
            else
            {
                _service.AddSammler(neuerSammler);
                return Ok();
            }
        }

        [HttpDelete("Sammler/{id}")]
        [SwaggerOperation("Delete Sammler by ID")]
        public IActionResult DeleteSammler(long id)
        {
            var sammler = _service.GetSammlerById(id);
            if (sammler == null)
            {
                return NotFound();
            }
            _service.DeleteSammler(id);
            return Ok();
        }

        [HttpPut("Sammler/{id}")]
        [SwaggerOperation("Update Sammler by ID")]
        public IActionResult UpdateSammler(long id, [FromBody] SammlerUpdateData sammlerUpdate)
        {
            var existingSammler = _service.GetSammlerById(id);
            if (existingSammler == null)
            {
                return NotFound();
            }

            // Mapping von Sammler und SammlerUpdateModel
            existingSammler.Surname = sammlerUpdate.Surname;
            existingSammler.Firstname = sammlerUpdate.Firstname;
            existingSammler.Nickname = sammlerUpdate.Nickname;
            existingSammler.Birthdate = sammlerUpdate.Birthdate;
            existingSammler.Email = sammlerUpdate.Email;
            existingSammler.Telephone = sammlerUpdate.Telephone;

            _service.UpdateSammler(existingSammler);
            return Ok(existingSammler);
        }


        // Oldtimer-Endpunkte------------------------------------------------------------------------------------------

        [HttpGet("Oldtimer")]
        [SwaggerOperation("Get all Oldtimer")]
        public IActionResult GetAllOldtimer()
        {
            List<Car> oldtimer = _service.GetAllOldtimer();
            return Ok(oldtimer);
        }

        [HttpGet("Oldtimer/Sammler/{id}")]
        [SwaggerOperation("Get Oldtimer by Sammler ID")]
        public IActionResult GetOldtimerBySammlerId(long id)
        {
            List<Car> oldtimer = _service.GetOldtimerPlusSammlerBySammlerId(id);
            if (oldtimer == null)
            {
                return NotFound();
            }
            return Ok(oldtimer);
        }

        [HttpGet("Sammler/Oldtimer")]
        [SwaggerOperation("Get Sammler by Oldtimer Brand and Model")]
        public IActionResult GetSammlerByOldtimerBrandAndModel(string? brand, string? model)
        {
            if (string.IsNullOrWhiteSpace(brand) && string.IsNullOrWhiteSpace(model))
            {
                return BadRequest("Both 'brand' and 'model' cannot be empty or null.");
            }

            List<Sammler> sammler = _service.GetSammlerByOldtimerBrandAndModel(brand, model);
            if (sammler == null || sammler.Count == 0)
            {
                return NotFound();
            }

            return Ok(sammler);
        }

        [HttpPost("Sammler/{id}/Oldtimer")] 
        [SwaggerOperation("Add Oldtimer to Sammler")]
        public IActionResult AddOldtimerToSammler(long id, [FromBody] CarDto carDto)
        {
            var sammler = _service.GetSammlerById(id);
            if (sammler == null)
            {
                return NotFound();
            }

            var addedOldtimer = _service.AddOldtimerToSammler(id, carDto); 

            return Ok(addedOldtimer); 
        }





        [HttpDelete("Oldtimer/{id}")]
        [SwaggerOperation("Remove Oldtimer by ID")]
        public IActionResult RemoveOldtimer(long id)
        {
            var oldtimer = _service.GetAllOldtimer().Find(c => c.Id == id);
            if (oldtimer == null)
            {
                return NotFound();
            }
            _service.RemoveOldtimer(id);
            return Ok();
        }

        [HttpGet("Sammler/{id}/Details")]
        [SwaggerOperation("Get Sammler Details by ID")]
        public IActionResult GetSammlerDetails(long id)
        {
            var sammler = _service.GetSammlerById(id);
            if (sammler == null)
            {
                return NotFound();
            }
            List<Car> oldtimer = _service.GetOldtimerBySammlerId(id);

            // Extrahierte Infos über den Sammler für den Request Body
            var sammlerInfo = new
            {
                Id = sammler.Id,
                Surname = $"{sammler.Surname}",
                Firstname = $"{sammler.Firstname}",
            };

            var sammlerDetails = new
            {
                Sammler = sammlerInfo,
                Oldtimer = oldtimer
            };
            return Ok(sammlerDetails);
        }
    }
}
