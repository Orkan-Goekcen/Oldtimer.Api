using MediatR;
using Microsoft.AspNetCore.Mvc;
using Oldtimer.Api.Commands;
using Oldtimer.Api.Data;
using Oldtimer.Api.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Oldtimer.Api.Controller
{
    [ApiController]
    [Route("api/sammler")]
    public class SammlerApiController : ControllerBase
    {
        private readonly IMediator mediator;

        public SammlerApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation("Get all Sammlers")]
        public async Task<ActionResult<List<Sammler>>> GetSammlers()
        {
            var query = new GetSammlersQuery { };
            var sammler = await mediator.Send(query);

            return sammler;
        }

        [HttpGet("{id}")]
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

        [HttpGet("FirstName/{firstName}")]
        [SwaggerOperation("Get Sammler by First Name")]
        public async Task<ActionResult<List<Sammler>>> GetSammlerByFirstName(string firstName)
        {
            var query = new GetSammlerByFirstNameQuery { FirstName = firstName };
            var sammlerList = await mediator.Send(query);

            if (sammlerList == null || sammlerList.Count == 0)
            {
                return NotFound($"No Sammler found with the first name: {firstName}");
            }

            return Ok(sammlerList);
        }

        [HttpGet("NickName/{nickName}")]
        [SwaggerOperation("Get Sammler by Nickname")]
        public async Task<ActionResult<List<Sammler>>> GetSammlerByNickName(string nickName)
        {
            var query = new GetSammlerByNickNameQuery { NickName = nickName };
            var sammlerList = await mediator.Send(query);

            if (sammlerList == null || sammlerList.Count == 0)
            {
                return NotFound($"No Sammler found with the Nickname: {nickName}");
            }

            return sammlerList;
        }

        [HttpGet("SurName/{surName}")]
        [SwaggerOperation("Get Sammler by Surname")]
        public async Task<ActionResult<List<Sammler>>> GetSammlerBySurName(string surName)
        {
            var query = new GetSammlerBySurNameQuery { SurName = surName };
            var sammlerList = await mediator.Send(query);

            if (sammlerList == null || sammlerList.Count == 0)
            {
                return NotFound($"No Sammler found with the Surname: {surName}");
            }

            return sammlerList;
        }

        [HttpGet("TelePhone/{telePhone}")]
        [SwaggerOperation("Get Sammler by Telephone")]
        public async Task<ActionResult<List<Sammler>>> GetSammlerByTelephone(string telePhone)
        {
            var query = new GetSammlerByTelephoneQuery { TelePhone = telePhone };
            var sammlerList = await mediator.Send(query);

            if (sammlerList == null || sammlerList.Count == 0)
            {
                return NotFound($"No Sammler found with the Telephone number: {telePhone}");
            }

            return sammlerList;
        }


        [HttpGet("{id}/Details")]
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

        [HttpPost]
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

        [HttpDelete("{id}")]
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

        [HttpPut("{id}")]
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
    }
}
