using Moq;
using Oldtimer.Api.Data;
using Oldtimer.Api.Service;
using System.Drawing.Drawing2D;

public class Mocks
{
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
        Id = 1, Brand = "Toyota",
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

        mockApiService.Setup(a => a.SammlerVorhanden(It.IsAny<Sammler>())).Returns(true);
        mockApiService.Setup(a => a.SammlerVorhanden(It.IsAny<Sammler>())).Returns(false);

        // Mock die Methode GetAllOldtimer, damit sie die oben erstellte CarsList zurückgibt
        mockApiService.Setup(a => a.GetAllOldtimer()).Returns(CarsList);

        // Mock die Methode GetOldtimerPlusSammlerBySammlerId, damit sie die Cars zurückgibt, die zum übergebenen Sammler gehören
        mockApiService.Setup(a => a.GetOldtimerPlusSammlerBySammlerId(It.IsAny<long>()))
                      .Returns<long>(sammlerId => CarsList.Where(c => c.Sammler.Id == sammlerId).ToList());

        return mockApiService;
    }
}