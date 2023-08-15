using MediatR;
using Microsoft.EntityFrameworkCore;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Queries
{
    public class GetOldtimerPlusSammlerBySammlerIdQuery : IRequest<List<Car>>
    {
        public long SammlerId { get; set; }
    }

    public class GetOldtimerPlusSammlerBySammlerIdQueryHandler : IRequestHandler<GetOldtimerPlusSammlerBySammlerIdQuery, List<Car>>
    {
        private readonly ApiContext context;

        public GetOldtimerPlusSammlerBySammlerIdQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<List<Car>> Handle(GetOldtimerPlusSammlerBySammlerIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Cars
                .Include(c => c.Sammler)
                .Where(c => c.Sammler.Id == request.SammlerId)
                .ToListAsync(cancellationToken);
        }
    }
}
