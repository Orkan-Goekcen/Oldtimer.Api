using Microsoft.EntityFrameworkCore;
using Moq;
using Oldtimer.Api.Data;

namespace Oldtimer.Api.Service
{
    public class ApiService : IApiService
    {
        private readonly ApiContext context;

        public ApiService(ApiContext context)
        {
            this.context = context;
        }

        public Sammler GetSammlerById(long id)
        {
            return context.Sammlers
                .FirstOrDefault(sammler => sammler.Id == id);
        }

        public List<Sammler> GetSammlers()
        {
            return context.Sammlers
                .ToList();
        }

        public List<Sammler> GetSammlerByFirstName(string firstName)
        {
            if (firstName != null)
            {
                return context.Sammlers.Where(n => n.Firstname.Contains(firstName, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return null;
        }

        public List<Sammler> GetSammlerBySurName(string surName)
        {
            if (string.IsNullOrWhiteSpace(surName))
            {
                return new List<Sammler>();
            }

            return context.Sammlers
                .Where(n => n.Surname.Contains(surName, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public List<Sammler> GetSammlerByNickName(string nickName)
        {
            if (nickName != null)
            {
                return context.Sammlers.Where(n => n.Nickname.Contains(nickName, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return null;
        }

        public List<Sammler> GetSammlerByTelephone(string telePhone)
        {
            if (telePhone != null)
            {
                return context.Sammlers.Where(n => n.Telephone.Contains(telePhone)).ToList();
            }
            return null;
        }

        public Sammler AddSammler(Sammler sammler)
        {
            context.Sammlers.Add(sammler);
            context.SaveChanges();
            return sammler;
        }

        public void DeleteSammler(long id)
        {
            var sammler = context.Sammlers.FirstOrDefault(s => s.Id == id);
            if (sammler != null)
            {
                context.Sammlers.Remove(sammler);
                context.SaveChanges();
            }
        }

        public void UpdateSammler(Sammler sammler)
        {
            var existingSammler = context.Sammlers.FirstOrDefault(s => s.Id == sammler.Id);
            if (existingSammler != null)
            {
                existingSammler.Firstname = sammler.Firstname;
                existingSammler.Surname = sammler.Surname;
                existingSammler.Nickname = sammler.Nickname;
                existingSammler.Birthdate = sammler.Birthdate;
                existingSammler.Email = sammler.Email;
                existingSammler.Telephone = sammler.Telephone;

                context.SaveChanges();
            }
        }

        public bool SammlerVorhanden(Sammler neuerSammler)
        {
            return context.Sammlers.Any(x =>
                (x.Id != neuerSammler.Id) &&
                (x.Firstname.Equals(neuerSammler.Firstname) ||
                x.Nickname.Equals(neuerSammler.Nickname) ||
                x.Telephone.Equals(neuerSammler.Telephone) ||
                x.Surname.Equals(neuerSammler.Surname)));
        }


        public List<Car> GetAllOldtimer()
        {
            return context.Cars
                .Include(c => c.Sammler)
                .ToList();
        }

        public List<Car> GetOldtimerPlusSammlerBySammlerId(long sammlerId)
        {
            return context.Cars
                .Include(c => c.Sammler)
                .Where(c => c.Sammler.Id == sammlerId)
                .ToList();
        }

        public List<Car> GetOldtimerBySammlerId(long sammlerId)
        {
            var sammler = context.Sammlers.FirstOrDefault(s => s.Id == sammlerId);
            if (sammler == null)
            {
                return null;
            }

            var oldtimer = context.Cars
                .Where(c => c.Sammler.Id == sammlerId)
                .ToList();

            // damit im Response Body nicht jedes mal der Sammler aufgezählt wird
            foreach (var car in oldtimer)
            {
                car.Sammler = null;
            }

            return oldtimer;
        }

        public List<Sammler> GetSammlerByOldtimerBrandAndModel(string? brand, string? model)
        {
            if (string.IsNullOrWhiteSpace(brand) && string.IsNullOrWhiteSpace(model))
            {
                return new List<Sammler>(); 
            }

            return context.Sammlers
                .Where(s => s.Cars.Any(c => (string.IsNullOrEmpty(brand) || c.Brand.Equals(brand)) && (string.IsNullOrEmpty(model) || c.Model.Equals(model))))
                .ToList();
        }

        public Car AddOldtimerToSammler(long sammlerId, CarDto carDto)
        {
            var sammler = context.Sammlers.FirstOrDefault(s => s.Id == sammlerId);
            if (sammler == null)
            {
                return null;
            }

            var car = new Car // Mapping von carDto und car
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                LicensePlate = carDto.LicensePlate,
                YearOfConstruction = carDto.YearOfConstruction,
                Colors = carDto.Colors,
                Sammler = sammler
            };

            context.Cars.Add(car);
            context.SaveChanges();

            return car;
        }

        public void RemoveOldtimer(long oldtimerId)
        {
            var oldtimer = context.Cars.FirstOrDefault(c => c.Id == oldtimerId);
            if (oldtimer != null)
            {
                context.Cars.Remove(oldtimer);
                context.SaveChanges();
            }
        }
    }
}