using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface ICityRepository
    {
        Task<City> GetCityById(Guid city_Id);
        Task<IEnumerable<City>> GetAllCities();
        Task<City> AddCity(City city);
        Task<City> UpdateCity(City city);
        Task<City> DeleteCity(Guid cityId);
    }
}
