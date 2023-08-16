using MediatR;
using Microsoft.EntityFrameworkCore;
using Oldtimer.Api.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Commands
{
    public class RemoveOldtimerCommand : IRequest<Unit>
    {
        public long OldtimerId { get; set; }
    }

    public class RemoveOldtimerCommandHandler : IRequestHandler<RemoveOldtimerCommand, Unit>
    {
        private readonly ApiContext _context;

        public RemoveOldtimerCommandHandler(ApiContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveOldtimerCommand request, CancellationToken cancellationToken)
        {
            var oldtimer = await _context.Cars.FirstOrDefaultAsync(c => c.Id == request.OldtimerId, cancellationToken);
            if (oldtimer != null)
            {
                _context.Cars.Remove(oldtimer);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
