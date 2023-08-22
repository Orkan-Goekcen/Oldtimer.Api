using Microsoft.EntityFrameworkCore;

namespace Oldtimer.Api.Data
{
    public interface IApiContext
    {
        DbSet<Car> Cars { get; }
        string DbPath { get; set; }
        DbSet<Sammler> Sammlers { get; set; }

        void SeedInitialData();
    }
}
