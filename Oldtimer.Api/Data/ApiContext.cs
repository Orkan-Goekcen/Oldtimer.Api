using Microsoft.EntityFrameworkCore;

namespace Oldtimer.Api.Data
{
    public class ApiContext : DbContext, IApiContext
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

            modelBuilder.Entity<Car>(b =>
            {
                b.Property<long?>("SammlerId").HasColumnType("INTEGER");
                b.HasOne<Sammler>("Sammler")
                    .WithMany("Cars")
                    .HasForeignKey("SammlerId");
            });
        }

        public void SeedInitialData()
        {
            Database.EnsureCreated();

            if (!Sammlers.Any())
            {
                var sammler1 = new Sammler
                {
                    Surname = "Smith",
                    Firstname = "John",
                    Nickname = "Johnny",
                    Birthdate = new DateTime(1985, 5, 15),
                    Email = "john.smith@example.com",
                    Telephone = "+1 555-1234"
                };

                var sammler2 = new Sammler
                {
                    Surname = "Doe",
                    Firstname = "Jane",
                    Nickname = "Janey",
                    Birthdate = new DateTime(1990, 8, 20),
                    Email = "jane.doe@example.com",
                    Telephone = "+1 555-5678"
                };

                Sammlers.AddRange(sammler1, sammler2);
            }

            if (!Cars.Any())
            {
                var sammler1 = Sammlers.First();
                var sammler2 = Sammlers.Skip(1).First();

                var car1 = new Car
                {
                    Brand = "Porsche",
                    Model = "911",
                    LicensePlate = "PORSCH123",
                    YearOfConstruction = "1969",
                    Colors = Car.Color.Red,
                    Sammler = sammler1
                };

                var car2 = new Car
                {
                    Brand = "Ford",
                    Model = "Mustang",
                    LicensePlate = "MUST123",
                    YearOfConstruction = "1970",
                    Colors = Car.Color.Blue,
                    Sammler = sammler1
                };

                var car3 = new Car
                {
                    Brand = "Volkswagen",
                    Model = "Beetle",
                    LicensePlate = "BEETL123",
                    YearOfConstruction = "1965",
                    Colors = Car.Color.Green,
                    Sammler = sammler2
                };

                var car4 = new Car
                {
                    Brand = "Chevrolet",
                    Model = "Camaro",
                    LicensePlate = "CAMRO123",
                    YearOfConstruction = "1972",
                    Colors = Car.Color.Yellow,
                    Sammler = sammler2
                };

                Cars.AddRange(car1, car2, car3, car4);
            }

            SaveChanges();
        }
    }
}