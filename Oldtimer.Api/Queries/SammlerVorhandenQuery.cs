using MediatR;
using Oldtimer.Api.Data;
using System.Data.Entity;

namespace Oldtimer.Api.Queries
{
    public class SammlerVorhandenQuery : IRequest<bool>
    {
        public Sammler NeuerSammler { get; set; }
    }

    public class SammlerVorhandenQueryHandler : IRequestHandler<SammlerVorhandenQuery, bool>
    {
        private readonly ApiContext context;

        public SammlerVorhandenQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(SammlerVorhandenQuery request, CancellationToken cancellationToken)
        {
            return await context.Sammlers.AnyAsync(x =>
                (x.Id != request.NeuerSammler.Id) &&
                (x.Firstname.Equals(request.NeuerSammler.Firstname) ||
                x.Nickname.Equals(request.NeuerSammler.Nickname) ||
                x.Telephone.Equals(request.NeuerSammler.Telephone) ||
                x.Surname.Equals(request.NeuerSammler.Surname)), cancellationToken);
        }
    }
}
