using MediatR;
using Oldtimer.Api.Data;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Commands
{
    public class UpdateSammlerCommand : IRequest<Unit>
    {
        public Sammler Sammler { get; set; }
    }

    public class UpdateSammlerCommandHandler : IRequestHandler<UpdateSammlerCommand, Unit>
    {
        private readonly ApiContext _context;

        public UpdateSammlerCommandHandler(ApiContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateSammlerCommand request, CancellationToken cancellationToken)
        {
            var existingSammler = await _context.Sammlers.FirstOrDefaultAsync(s => s.Id == request.Sammler.Id, cancellationToken);

            if (existingSammler != null)
            {
                existingSammler.Firstname = request.Sammler.Firstname;
                existingSammler.Surname = request.Sammler.Surname;
                existingSammler.Nickname = request.Sammler.Nickname;
                existingSammler.Birthdate = request.Sammler.Birthdate;
                existingSammler.Email = request.Sammler.Email;
                existingSammler.Telephone = request.Sammler.Telephone;

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
