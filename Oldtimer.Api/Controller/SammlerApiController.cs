using FluentValidation;
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
        public async Task<ActionResult> GetSammlerById(long id)
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
        public async Task<ActionResult> GetSammlerDetails(long id)
        {
            var query = new SammlerDetailsQuery { SammlerId = id };
            var result = await mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation("Create Sammler")]
        public async Task<ActionResult<Sammler>> CreateSammler([FromBody] SammlerDto neuerSammlerDto, [FromServices] IValidator<SammlerDto> validator)
        {
            // Wandeln Sie das SammlerDto in ein Sammler-Objekt um.
            var neuerSammler = new Sammler
            {
                Surname = neuerSammlerDto.Surname,
                Firstname = neuerSammlerDto.Firstname,
                Nickname = neuerSammlerDto.Nickname,
                Birthdate = neuerSammlerDto.Birthdate,
                Email = neuerSammlerDto.Email,
                Telephone = neuerSammlerDto.Telephone
            };

            var query = new SammlerVorhandenQuery { NeuerSammler = neuerSammler };
            var sammlerBereitsVorhanden = await mediator.Send(query);

            var validationResult = await validator.ValidateAsync(neuerSammlerDto);

            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            if (sammlerBereitsVorhanden)
            {
                return Conflict(neuerSammler);
            }
            else
            {
                var addSammlerCommand = new AddSammlerCommand { Sammler = neuerSammler };
                var addedSammler = await mediator.Send(addSammlerCommand);

                return Ok(addedSammler);
            }
        }



        [HttpDelete("{id}")]
        [SwaggerOperation("Delete Sammler by ID")]
        public async Task<ActionResult> DeleteSammler(long id)
        {
            var query = new GetSammlerByIdQuery { SammlerId = id };
            var sammler = await mediator.Send(query);

            if (sammler == null)
            {
                return NotFound();
            }
            var deleteSammlerCommand = new DeleteSammlerCommand { Id = id };
            await mediator.Send(deleteSammlerCommand);

            return Ok();
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Update Sammler by ID")]
        public async Task<ActionResult> UpdateSammler(long id, [FromBody] SammlerUpdateData sammlerUpdate, [FromServices] IValidator<SammlerUpdateData> validator)
        {
            var query = new GetSammlerByIdQuery { SammlerId = id };
            var sammler = await mediator.Send(query);

            if (sammler == null)
            {
                return NotFound();
            }

            var validationResult = await validator.ValidateAsync(sammlerUpdate);

            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }


            // Mapping von Sammler und SammlerUpdateModel
            sammler.Surname = sammlerUpdate.Surname;
            sammler.Firstname = sammlerUpdate.Firstname;
            sammler.Nickname = sammlerUpdate.Nickname;
            sammler.Birthdate = sammlerUpdate.Birthdate;
            sammler.Email = sammlerUpdate.Email;
            sammler.Telephone = sammlerUpdate.Telephone;

            var updateSammlerCommand = new UpdateSammlerCommand { Sammler = sammler };
            await mediator.Send(updateSammlerCommand);

            return Ok(sammler);
        }

    }
}
