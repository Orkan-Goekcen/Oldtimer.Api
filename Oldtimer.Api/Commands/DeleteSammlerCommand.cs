using MediatR;
using Oldtimer.Api.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Commands
{
    public class DeleteSammlerCommand : IRequest<Unit>
    {
        public long Id { get; set; }
    }

    public class DeleteSammlerCommandHandler : IRequestHandler<DeleteSammlerCommand, Unit>
    {
        private readonly ApiContext context;

        public DeleteSammlerCommandHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteSammlerCommand request, CancellationToken cancellationToken)
        {
            var sammler = await context.Sammlers.FindAsync(request.Id);
            if (sammler != null)
            {
                context.Sammlers.Remove(sammler);
                await context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
