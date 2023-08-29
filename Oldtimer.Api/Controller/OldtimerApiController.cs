using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oldtimer.Api.Commands;
using Oldtimer.Api.Data;
using Oldtimer.Api.Queries;
using Swashbuckle.AspNetCore.Annotations;
namespace Oldtimer.Api.Controller
{
    [Route("api/oldtimer")]
    [ApiController]
    public class OldtimerApiController : ControllerBase
    {
        private readonly IMediator mediator;

        public OldtimerApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation("Get all Oldtimer")]
        public async Task<ActionResult<List<Car>>> GetAllOldtimer()
        {
            var query = new GetAllOldtimerQuery { };
            var oldtimer = await mediator.Send(query);

            return oldtimer;
        }

        [HttpGet("Sammler/{id}")]
        [SwaggerOperation("Get Oldtimer by Sammler ID")]
        public async Task<ActionResult<List<Car>>> GetOldtimerBySammlerId(long id)
        {
            var query = new GetOldtimerBySammlerIdQuery { SammlerId = id };
            var oldtimer = await mediator.Send(query);

            if (oldtimer == null || oldtimer.Count == 0)
            {
                return NotFound($"No Oldtimers found for Sammler ID: {id}");
            }

            return oldtimer;
        }


        [HttpGet("Sammler")]
        [SwaggerOperation("Get Sammler by Oldtimer Brand and Model")]
        public async Task<ActionResult> GetSammlerByOldtimerBrandAndModel(string brand, string model)
        {
            if (string.IsNullOrWhiteSpace(brand) && string.IsNullOrWhiteSpace(model))
            {
                return BadRequest("Both 'brand' and 'model' cannot be empty or null.");
            }

            //Damit die Groß und Kleinschreibung egal ist: 
            if (!string.IsNullOrWhiteSpace(brand))
                brand = char.ToUpper(brand[0]) + brand.Substring(1);
            if (!string.IsNullOrWhiteSpace(model))
                model = char.ToUpper(model[0]) + model.Substring(1);

            var query = new GetSammlerByOldtimerBrandAndModelQuery { Brand = brand, Model = model };
            var sammler = await mediator.Send(query);

            if (sammler == null || sammler.Count == 0)
            {
                return NotFound();
            }

            return Ok(sammler);
        }

        [HttpPost("Sammler/{id}")]
        [SwaggerOperation("Add Oldtimer to Sammler")]
        public async Task<ActionResult> AddOldtimerToSammler(long id, [FromBody] CarDto carDto)
        {
            var query = new GetSammlerByIdQuery { SammlerId = id };
            var sammler = await mediator.Send(query);

            if (sammler == null)
            {
                return NotFound();
            }

            var addOldtimerToSammlerCommand = new AddOldtimerToSammlerCommand { SammlerId = id, CarDto = carDto };
            var addedOldtimer = await mediator.Send(addOldtimerToSammlerCommand);

            return Ok();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Remove Oldtimer by ID")]
        public async Task<ActionResult> RemoveOldtimer(long id)
        {
            var command = new RemoveOldtimerCommand { OldtimerId = id };

            await mediator.Send(command);

            return Ok();
        }
    }
}
