using MediatR;
using Oldtimer.Api.Data;
using System.Collections.Generic;
using System.Data.Entity;

namespace Oldtimer.Api.Queries
{
    public class SammlerDetailsQuery : IRequest<SammlerDetailsResponse>
    {
        public long SammlerId { get; set; }
    }

    public class SammlerDetailsResponse
    {
        public Sammler Sammler { get; set; }
        public List<Car> Oldtimer { get; set; }
    }

    public class SammlerDetailsQueryHandler : IRequestHandler<SammlerDetailsQuery, SammlerDetailsResponse>
    {
        private readonly IApiContext context;

        public SammlerDetailsQueryHandler(IApiContext context)
        {
            this.context = context;
        }

        public async Task<SammlerDetailsResponse> Handle(SammlerDetailsQuery request, CancellationToken cancellationToken)
        {
            var sammler = await context.Sammlers.FirstOrDefaultAsync(s => s.Id == request.SammlerId, cancellationToken);

            if (sammler == null)
            {
                return null;
            }

            var oldtimer = await context.Cars
                .Where(c => c.Sammler.Id == request.SammlerId)
                .ToListAsync(cancellationToken);

            var response = new SammlerDetailsResponse
            {
                Sammler = sammler,
                Oldtimer = oldtimer
            };

            return response;
        }
    }
}
