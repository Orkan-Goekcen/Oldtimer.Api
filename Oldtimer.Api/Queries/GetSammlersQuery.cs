using MediatR;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Oldtimer.Api.Queries
{
    public class GetSammlersQuery : IRequest<List<Sammler>>
    {

    }

    public class GetSammlersQueryHandler : IRequestHandler<GetSammlersQuery, List<Sammler>>
    {
        private readonly ApiContext context;

        public GetSammlersQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<List<Sammler>> Handle(GetSammlersQuery request, CancellationToken cancellationToken)
        {
            var result = await context.Sammlers.ToListAsync();

            return result;
        }
    }
}