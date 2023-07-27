using Oldtimer.Api.Data;

namespace Oldtimer.Api.Service
{
    public interface IApiService
    {
        //Car AddOldtimerToSammler(long id, Car oldtimer);
        Car AddOldtimerToSammler(long sammlerId, CarDto carDto);
        Sammler AddSammler(Sammler sammler);
        void DeleteSammler(long id);
        List<Car> GetAllOldtimer();
        List<Car> GetOldtimerBySammlerId(long id);
        List<Car> GetOldtimerPlusSammlerBySammlerId(long sammlerId);
        List<Sammler> GetSammlerByFirstName(string firstName);
        Sammler GetSammlerById(long id);
        List<Sammler> GetSammlerByNickName(string nickName);
        List<Sammler> GetSammlerByOldtimerBrandAndModel(string brand, string model);
        List<Sammler> GetSammlerBySurName(string surName);
        List<Sammler> GetSammlerByTelephone(string telePhone);
        List<Sammler> GetSammlers();
        void RemoveOldtimer(long id);
        bool SammlerVorhanden(Sammler neuerSammler);
        void UpdateSammler(Sammler sammler);
    }
}