using Microsoft.EntityFrameworkCore;

namespace Oldtimer.Api.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Sammler> Sammlers { get; set; }
        public DbSet<Car> Cars { get; set; }

        public string DbPath { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public ApiContext(IConfiguration config, DbContextOptions<ApiContext> options) : base(options)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            DbPath = Path.Join(path, config.GetConnectionString("SammlerDbFilename"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sammler>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Car>()
                .HasKey(e => e.Id);
        }

        public void SeedInitialData()
        {
            Database.EnsureCreated();

            if (Sammlers.Any())
            {
                Sammlers.RemoveRange(Sammlers);
                SaveChanges();
            }

            if (Cars.Any())
            {
                Cars.RemoveRange(Cars);
                SaveChanges();
            }

            // Seed data for Sammler entity
            var sammler1 = new Sammler
            {
                Firstname = "Niklas",
                Surname = "Mueller",
                Nickname = "Nick",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "muellerr@example.com",
                Telephone = "1234214"
            };

            var sammler2 = new Sammler
            {
                Firstname = "Erkan",
                Surname = "Manti",
                Nickname = "Erkannicht",
                Birthdate = new DateTime(1995, 1, 1),
                Email = "erkan@example.com",
                Telephone = "34241"
            };

            var sammler3 = new Sammler
            {
                Firstname = "Menschmann",
                Surname = "Mustermann",
                Nickname = "Mann",
                Birthdate = new DateTime(1985, 1, 1),
                Email = "mann@example.com",
                Telephone = "5467654"
            };

            Sammlers.AddRange(sammler1, sammler2, sammler3);
            SaveChanges();

            // Seed data for Car entity
            var car1 = new Car
            {
                Brand = "Volkswagen",
                Model = "Beetle",
                LicensePlate = "OLD123",
                YearOfConstruction = "1965",
                Colors = Car.Color.Red,
                Sammler = sammler1
            };

            var car2 = new Car
            {
                Brand = "Ford",
                Model = "Mustang",
                LicensePlate = "OLD456",
                YearOfConstruction = "1968",
                Colors = Car.Color.Blue,
                Sammler = sammler2
            };

            var car3 = new Car
            {
                Brand = "Porsche",
                Model = "911",
                LicensePlate = "OLD789",
                YearOfConstruction = "1973",
                Colors = Car.Color.Black,
                Sammler = sammler3
            };

            Cars.AddRange(car1, car2, car3);
            SaveChanges();
        }

    }
}
