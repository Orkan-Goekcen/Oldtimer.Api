using MediatR;
using Microsoft.EntityFrameworkCore;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Queries
{
    public class GetSammlersQuery : IRequest<List<Sammler>>
    {

    }

    public class GetSammlersQueryHandler : IRequestHandler<GetSammlersQuery, List<Sammler>>
    {
        private readonly IApiContext context;

        public GetSammlersQueryHandler(IApiContext context)
        {
            this.context = context;
        }
        // Test Vorhanden
        public async Task<List<Sammler>> Handle(GetSammlersQuery request, CancellationToken cancellationToken)
        {
            var result = await context.Sammlers
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}