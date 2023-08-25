using MediatR;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oldtimer.Api.Queries
{
    public class GetSammlerByNickNameQuery : IRequest<List<Sammler>>
    {
        public string NickName { get; set; }
    }

    public class GetSammlerByNickNameQueryHandler : IRequestHandler<GetSammlerByNickNameQuery, List<Sammler>>
    {
        private readonly IApiContext context;

        public GetSammlerByNickNameQueryHandler(IApiContext context)
        {
            this.context = context;
        }

        public async Task<List<Sammler>> Handle(GetSammlerByNickNameQuery request, CancellationToken cancellationToken)
        {
            if (request.NickName != null)
            {
                return await context.Sammlers
                    .Where(n => n.Nickname.Contains(request.NickName, StringComparison.InvariantCultureIgnoreCase))
                    .ToListAsync(cancellationToken);
            }
            return null;
        }
    }
}
