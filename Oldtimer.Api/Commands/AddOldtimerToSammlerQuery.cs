using MediatR;
using Oldtimer.Api.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oldtimer.Api.Commands
{
    public class AddOldtimerToSammlerCommand : IRequest<Car>
    {
        public long SammlerId { get; set; }
        public CarDto CarDto { get; set; }
    }

    public class AddOldtimerToSammlerCommandHandler : IRequestHandler<AddOldtimerToSammlerCommand, Car>
    {
        private readonly ApiContext _context;

        public AddOldtimerToSammlerCommandHandler(ApiContext context)
        {
            _context = context;
        }

        public async Task<Car> Handle(AddOldtimerToSammlerCommand request, CancellationToken cancellationToken)
        {
            var sammler = await _context.Sammlers.FirstOrDefaultAsync(s => s.Id == request.SammlerId, cancellationToken);
            if (sammler == null)
            {
                return null;
            }

            var car = new Car // Mapping von CarDto und Car
            {
                Brand = request.CarDto.Brand,
                Model = request.CarDto.Model,
                LicensePlate = request.CarDto.LicensePlate,
                YearOfConstruction = request.CarDto.YearOfConstruction,
                Colors = request.CarDto.Colors,
                Sammler = sammler
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync(cancellationToken);

            return car;
        }
    }
}
