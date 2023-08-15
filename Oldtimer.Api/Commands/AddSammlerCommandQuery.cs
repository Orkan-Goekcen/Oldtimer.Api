using MediatR;
using Oldtimer.Api.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Commands
{
    public class AddSammlerCommand : IRequest<Sammler>
    {
        public Sammler Sammler { get; set; }
    }

    public class AddSammlerCommandHandler : IRequestHandler<AddSammlerCommand, Sammler>
    {
        private readonly ApiContext context;

        public AddSammlerCommandHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<Sammler> Handle(AddSammlerCommand request, CancellationToken cancellationToken)
        {
            if (request.Sammler != null)
            {
                context.Sammlers.Add(request.Sammler);
                await context.SaveChangesAsync(cancellationToken);
                return request.Sammler;
            }
            return null;
        }
    }
}
