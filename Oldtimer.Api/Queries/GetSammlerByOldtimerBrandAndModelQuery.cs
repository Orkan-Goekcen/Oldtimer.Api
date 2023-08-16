using MediatR;
using Microsoft.EntityFrameworkCore;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Queries
{
    public class GetSammlerByOldtimerBrandAndModelQuery : IRequest<List<Sammler>>
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
    }

    public class GetSammlerByOldtimerBrandAndModelQueryHandler : IRequestHandler<GetSammlerByOldtimerBrandAndModelQuery, List<Sammler>>
    {
        private readonly ApiContext context;

        public GetSammlerByOldtimerBrandAndModelQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<List<Sammler>> Handle(GetSammlerByOldtimerBrandAndModelQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Brand) && string.IsNullOrWhiteSpace(request.Model))
            {
                return new List<Sammler>();
            }

            return await context.Sammlers
                .Where(s => s.Cars.Any(c => (string.IsNullOrEmpty(request.Brand) || c.Brand.Equals(request.Brand)) && (string.IsNullOrEmpty(request.Model) || c.Model.Equals(request.Model))))
                .ToListAsync(cancellationToken);
        }
    }
}
