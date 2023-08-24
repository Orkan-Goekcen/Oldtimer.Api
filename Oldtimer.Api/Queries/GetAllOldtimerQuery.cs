using MediatR;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oldtimer.Api.Queries
{
    public class GetAllOldtimerQuery : IRequest<List<Car>>
    {

    }

    public class GetAllOldtimerQueryHandler : IRequestHandler<GetAllOldtimerQuery, List<Car>>
    {
        private readonly IApiContext context;

        public GetAllOldtimerQueryHandler(IApiContext context)
        {
            this.context = context;
        }
        // Test vorhanden
        public async Task<List<Car>> Handle(GetAllOldtimerQuery request, CancellationToken cancellationToken)
        {
            return await context.Cars
                .Include(c => c.Sammler)
                .ToListAsync(cancellationToken);
        }
    }
}
