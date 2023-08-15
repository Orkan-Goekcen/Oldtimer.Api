using MediatR;
using Oldtimer.Api.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Queries
{
    public class GetSammlerByIdQuery : IRequest<Sammler>
    {
        public long SammlerId { get; set; }
    }

    public class GetSammlerByIdQueryHandler : IRequestHandler<GetSammlerByIdQuery, Sammler>
    {
        private readonly ApiContext context;

        public GetSammlerByIdQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<Sammler> Handle(GetSammlerByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await context.FindAsync<Sammler>(request.SammlerId);

            return result;
        }
    }
}
