using MediatR;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Queries
{
    public class GetSammlerByTelephoneQuery : IRequest<List<Sammler>>
    {
        public string TelePhone { get; set; }
    }

    public class GetSammlerByTelephoneQueryHandler : IRequestHandler<GetSammlerByTelephoneQuery, List<Sammler>>
    {
        private readonly ApiContext context;

        public GetSammlerByTelephoneQueryHandler(ApiContext context)
        {
            this.context = context;
        }

        public async Task<List<Sammler>> Handle(GetSammlerByTelephoneQuery request, CancellationToken cancellationToken)
        {
            if (request.TelePhone != null)
            {
                return await context.Sammlers
                    .Where(n => n.Telephone.Contains(request.TelePhone))
                    .ToListAsync(cancellationToken);
            }
            return null;
        }
    }
}
