using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oldtimer.Api.Commands;
using Oldtimer.Api.Data;
using Oldtimer.Api.Queries;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var oldtimerBySammlerId = new GetOldtimerBySammlerIdQuery { SammlerId = id };
            var oldtimer = await mediator.Send(oldtimerBySammlerId);

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
        public async Task<ActionResult<Sammler>> CreateSammler([FromBody] Sammler neuerSammler)
        {
            var query = new SammlerVorhandenQuery { NeuerSammler = neuerSammler };
            var sammlerBereitsVorhanden = await mediator.Send(query);

            if (sammlerBereitsVorhanden)
            {
                return Conflict(neuerSammler);
            }
            else
            {
                var addSammlerCommand = new AddSammlerCommand { Sammler = neuerSammler };
                var addedSammler = await mediator.Send(addSammlerCommand);

                if (addedSammler != null)
                {
                    return Ok(addedSammler);
                }
                else
                {
                    return BadRequest("Sammler Not Found.");
                }
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
            var deleteSammlerCommand = new DeleteSammlerCommand { Id = id };
            await mediator.Send(deleteSammlerCommand);

            return Ok(sammler);
        }

        [HttpPut("Sammler/{id}")]
        [SwaggerOperation("Update Sammler by ID")]
        public async Task<IActionResult> UpdateSammler(long id, 
            [FromBody] SammlerUpdateData sammlerUpdate)

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

            var updateSammlerCommand = new UpdateSammlerCommand { Sammler = sammler};
            await mediator.Send(updateSammlerCommand);

            return Ok(sammler);
        }


        [HttpGet("Oldtimer")]
        [SwaggerOperation("Get all Oldtimer")]
        public async Task<IActionResult> GetAllOldtimer()
        {
            var query = new GetAllOldtimerQuery { };
            var oldtimer = await mediator.Send(query);

            return Ok(oldtimer);
        }

        [HttpGet("Oldtimer/Sammler/{id}")]
        [SwaggerOperation("Get Oldtimer by Sammler ID")]
        public async Task<IActionResult> GetOldtimerBySammlerId(long id)
        {
            var query = new GetOldtimerPlusSammlerBySammlerIdQuery { SammlerId = id};
            var oldtimer = await mediator.Send(query);

            if (oldtimer.Count == 0) 
            {
                return NotFound($"Id:{id} does not exist!");
            }
            return Ok(oldtimer);
        }

        [HttpGet("Sammler/Oldtimer")]
        [SwaggerOperation("Get Sammler by Oldtimer Brand and Model")]
        public async Task<IActionResult> GetSammlerByOldtimerBrandAndModel(string brand, string model)
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

            var query = new GetSammlerByOldtimerBrandAndModelQuery { Brand = brand, Model = model};
            var sammler = await mediator.Send(query);

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

            var addOldtimerToSammlerCommand = new AddOldtimerToSammlerCommand { SammlerId = id, CarDto = carDto};
            var addedOldtimer = await mediator.Send(addOldtimerToSammlerCommand);

            return Ok(addedOldtimer);
        }

        [HttpDelete("Oldtimer/{id}")]
        [SwaggerOperation("Remove Oldtimer by ID")]
        public async Task<IActionResult> RemoveOldtimer(long id)
        {
            var command = new RemoveOldtimerCommand { OldtimerId = id };

            await mediator.Send(command);

            return Ok();
        }
    }
}