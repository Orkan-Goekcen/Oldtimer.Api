using MediatR;
using Microsoft.EntityFrameworkCore;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Queries
{
    public class GetOldtimerBySammlerIdQuery : IRequest<List<Car>>
    {
        public long SammlerId { get; set; }
    }

    public class GetOldtimerBySammlerIdQueryHandler : IRequestHandler<GetOldtimerBySammlerIdQuery, List<Car>>
    {
        private readonly ApiContext context;

        public GetOldtimerBySammlerIdQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<List<Car>> Handle(GetOldtimerBySammlerIdQuery request, CancellationToken cancellationToken)
        {
            var sammler = await context.Sammlers.FirstOrDefaultAsync(s => s.Id == request.SammlerId);
            if (sammler == null)
            {
                return null;
            }

            var oldtimer = await context.Cars
                .Where(c => c.Sammler.Id == request.SammlerId)
                .ToListAsync(cancellationToken);

            return oldtimer;
        }
    }
}
