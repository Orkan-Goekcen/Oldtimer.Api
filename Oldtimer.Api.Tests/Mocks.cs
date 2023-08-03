using Moq;
using Oldtimer.Api.Data;
using Oldtimer.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;

public class Mocks
{
    public static void SetupGetOldtimerPlusSammlerBySammlerId(Mock<IApiService> mockApiService, List<Car> carsList)
    {
        mockApiService.Setup(a => a.GetOldtimerPlusSammlerBySammlerId(It.IsAny<long>()))
                      .Returns<long>(sammlerId => carsList.Where(c => c.Sammler != null && c.Sammler.Id == sammlerId).ToList());
    }

    public static void SetupRemoveOldtimer(Mock<IApiService> mockApiService, List<Car> carsList)
    {
        mockApiService.Setup(a => a.RemoveOldtimer(It.IsAny<long>()))
            .Callback<long>(oldtimerIdToRemove =>
            {
                var carToRemove = carsList.FirstOrDefault(c => c.Id == oldtimerIdToRemove);
                if (carToRemove != null)
                {
                    carsList.Remove(carToRemove); // Entfernen des Cars aus der CarsList
                }
            });
    }

    public static List<Sammler> SammlersList { get; } = new List<Sammler>
    {
        new Sammler
        {
            Id = 1,
            Firstname = "John",
            Surname = "Doe",
            Telephone = "555-1234"
        },
        new Sammler
        {
            Id = 2,
            Firstname = "Jane",
            Surname = "Smith",
            Telephone = "555-5678"
        }
    };

    public static List<Car> CarsList { get; } = new List<Car>
    {
        new Car
        {
            Id = 1,
            Brand = "Toyota",
            Model = "Supra",
            Sammler = new Sammler
            {
                Id = 1,
                Firstname = "John"
            }
        },
        new Car
        {
            Id = 2,
            Brand = "Ford",
            Model = "Mustang",
            Sammler = new Sammler
            {
                Id = 2,
                Firstname = "Jane"
            }
        }
    };

    public static Mock<IApiService> CreateMockApiService()
    {
        var mockApiService = new Mock<IApiService>();
        var mockSammlerRepository = new Mock<IApiService>();

        long idToFind = 1;
        var expectedSammler = new Sammler
        {
            Id = idToFind,
            Firstname = "John"
        };

        mockApiService.Setup(a => a.GetSammlerByTelephone(It.IsAny<string>())).Returns<string>(telePhone
                => SammlersList.Where(s => s.Telephone.Contains(telePhone, StringComparison.InvariantCultureIgnoreCase)).ToList());

        mockApiService.Setup(a => a.GetSammlerById(idToFind)).Returns(expectedSammler);

        mockSammlerRepository.Setup(r => r.GetSammlerById(idToFind)).Returns(expectedSammler);

        mockApiService.Setup(a => a.GetSammlers()).Returns(SammlersList);

        mockApiService.Setup(a => a.GetSammlerByFirstName(It.IsAny<string>())).Returns<string>(firstName
            => SammlersList.Where(s => s.Firstname.Contains(firstName, StringComparison.InvariantCultureIgnoreCase)).ToList());

        mockApiService.Setup(a => a.GetSammlerBySurName(It.IsAny<string>())).Returns<string>(surName =>
        {
            if (string.IsNullOrWhiteSpace(surName))
            {
                return new List<Sammler>();
            }
            return SammlersList.Where(s => s.Surname.Contains(surName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        });

        mockApiService.Setup(a => a.AddOldtimerToSammler(It.IsAny<long>(), It.IsAny<CarDto>()))
    .Returns<long, CarDto>((sammlerId, carDto) =>
    {
        var sammler = SammlersList.FirstOrDefault(s => s.Id == sammlerId);
        if (sammler == null)
        {
            return null;
        }

        var car = new Car
        {
            Brand = carDto.Brand,
            Model = carDto.Model,
            LicensePlate = carDto.LicensePlate,
            YearOfConstruction = carDto.YearOfConstruction,
            Colors = carDto.Colors,
            Sammler = sammler
        };

        CarsList.Add(car);
        return car;
    });

        mockApiService.Setup(a => a.AddSammler(It.IsAny<Sammler>()))
          .Returns<Sammler>(sammler => sammler);

        mockApiService.Setup(a => a.DeleteSammler(It.IsAny<long>()))
           .Callback<long>(sammlerId =>
           {
               mockApiService.Verify(a => a.DeleteSammler(sammlerId), Times.Once);
           });

        mockApiService.Setup(a => a.UpdateSammler(It.IsAny<Sammler>()))
                      .Callback<Sammler>(updatedSammler =>
                      {
                          mockApiService.Verify(a => a.UpdateSammler(updatedSammler), Times.Once);
                      });

        mockApiService.Setup(a => a.GetSammlerByOldtimerBrandAndModel(It.IsAny<string>(), It.IsAny<string>()))
              .Returns<string, string>((brand, model) =>
                  SammlersList
                      .Where(s => s.Cars.Any(c => (string.IsNullOrEmpty(brand) || c.Brand.Equals(brand)) && (string.IsNullOrEmpty(model) || c.Model.Equals(model))))
                      .ToList());

        mockApiService.Setup(a => a.SammlerVorhanden(It.IsAny<Sammler>())).Returns((Sammler neuerSammler) =>
        {
            return SammlersList.Any(x => x.Id == neuerSammler.Id);
        });

        mockApiService.Setup(a => a.GetAllOldtimer()).Returns(CarsList);

        mockApiService.Setup(a => a.GetOldtimerPlusSammlerBySammlerId(It.IsAny<long>()))
                      .Returns<long>(sammlerId => CarsList.Where(c => c.Sammler.Id == sammlerId).ToList());

        return mockApiService;
    }
}
