using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oldtimer.Api.Data;
using Oldtimer.Api.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Oldtimer.Api.Controller
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly IMediator mediator;

        public ApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("Sammler")]
        [SwaggerOperation("Get all Sammlers")]
        public async Task<IActionResult> GetSammlers()
        {
            var query = new GetSammlersQuery { };
            var sammler = await mediator.Send(query);


            return Ok(sammler);
        }

        [HttpGet("Sammler/{id}")]
        [SwaggerOperation("Get Sammler by ID")]
        public async Task<IActionResult> GetSammlerById(long id)
        {
            var query = new GetSammlerByIdQuery { SammlerId = id };
            var sammler = await mediator.Send(query);

            if (sammler == null)
            {
                return NotFound();
            }

            return Ok(sammler);
        }


        [HttpGet("Sammler/{id}/Details")]
        [SwaggerOperation("Get Sammler Details by ID")]
        public async Task<IActionResult> GetSammlerDetails(long id)
        {
            var query = new GetSammlerByIdQuery { SammlerId = id };
            var sammler = await mediator.Send(query);

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

        [HttpPost("Sammler")]
        [SwaggerOperation("Create Sammler")]
        public ActionResult<Sammler> CreateSammler([FromBody] Sammler neuerSammler)
        {
            if (neuerSammler == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

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
        public async Task<IActionResult> DeleteSammler(long id)
        {
            var query = new GetSammlerByIdQuery { SammlerId = id };
            var sammler = await mediator.Send(query);

            if (sammler == null)
            {
                return NotFound();
            }
            _service.DeleteSammler(id);
            return Ok();
        }

        [HttpPut("Sammler/{id}")]
        [SwaggerOperation("Update Sammler by ID")]
        public async Task<IActionResult> UpdateSammler(long id, 
            [FromBody] SammlerUpdateData sammlerUpdate, 
            [FromServices] IMediator mediator)

        {
            var query = new GetSammlerByIdQuery { SammlerId = id };
            var sammler = await mediator.Send(query);

            if (sammler == null)
            {
                return NotFound();
            }

            // Mapping von Sammler und SammlerUpdateModel
            sammler.Surname = sammlerUpdate.Surname;
            sammler.Firstname = sammlerUpdate.Firstname;
            sammler.Nickname = sammlerUpdate.Nickname;
            sammler.Birthdate = sammlerUpdate.Birthdate;
            sammler.Email = sammlerUpdate.Email;
            sammler.Telephone = sammlerUpdate.Telephone;

            _service.UpdateSammler(sammler);
            return Ok(sammler);
        }





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
            if (oldtimer.Count == 0) 
            {
                return NotFound($"Id:{id} does not exist!");
            }
            return Ok(oldtimer);
        }

        [HttpGet("Sammler/Oldtimer")]
        [SwaggerOperation("Get Sammler by Oldtimer Brand and Model")]
        public IActionResult GetSammlerByOldtimerBrandAndModel(string brand, string model)
        {
            if (string.IsNullOrWhiteSpace(brand) && string.IsNullOrWhiteSpace(model))
            {
                return BadRequest("Both 'brand' and 'model' cannot be empty or null.");
            }

            if (!string.IsNullOrWhiteSpace(brand))
            {
                brand = char.ToUpper(brand[0]) + brand.Substring(1);
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                model = char.ToUpper(model[0]) + model.Substring(1);
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
        public async Task<IActionResult> AddOldtimerToSammler(long id, [FromBody] CarDto carDto)
        {
            var query = new GetSammlerByIdQuery { SammlerId = id };
            var sammler = await mediator.Send(query);

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
    }
}