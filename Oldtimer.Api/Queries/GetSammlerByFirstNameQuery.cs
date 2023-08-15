using MediatR;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Oldtimer.Api.Queries
{
    public class GetSammlerByFirstNameQuery : IRequest<List<Sammler>>
    {
        public string FirstName { get; set; }
    }

    public class GetSammlerByFirstNameQueryHandler : IRequestHandler<GetSammlerByFirstNameQuery, List<Sammler>>
    {
        private readonly ApiContext context;

        public GetSammlerByFirstNameQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<List<Sammler>> Handle(GetSammlerByFirstNameQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.FirstName))
            {
                return await context.Sammlers
                    .Where(n => n.Firstname.Contains(request.FirstName, StringComparison.InvariantCultureIgnoreCase))
                    .ToListAsync(cancellationToken);
            }
            return new List<Sammler>();
        }
    }
}
