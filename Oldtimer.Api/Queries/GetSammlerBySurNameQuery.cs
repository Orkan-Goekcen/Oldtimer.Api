using MediatR;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oldtimer.Api.Queries
{
    public class GetSammlerBySurNameQuery : IRequest<List<Sammler>>
    {
        public string SurName { get; set; }
    }

    public class GetSammlerBySurNameQueryHandler : IRequestHandler<GetSammlerBySurNameQuery, List<Sammler>>
    {
        private readonly ApiContext context;

        public GetSammlerBySurNameQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<List<Sammler>> Handle(GetSammlerBySurNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SurName))
            {
                return new List<Sammler>();
            }

            return await context.Sammlers
                .Where(n => n.Surname.Contains(request.SurName, StringComparison.InvariantCultureIgnoreCase))
                .ToListAsync(cancellationToken);
        }
    }
}