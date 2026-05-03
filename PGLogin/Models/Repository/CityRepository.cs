using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Models.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly MydBContext _mydBContext;
        public CityRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }

        public async Task<City> AddCity(City city)
        {
            city.City_Id = Guid.NewGuid();

            // Convert AreaName → Area entities
            foreach (var area in city.AreaName!)
            {
                city.Areas!.Add(new Area
                {
                    AreaId = Guid.NewGuid(),
                    AreaName = area,
                    City_Id = city.City_Id
                });
            }

            _mydBContext.cities.Add(city);
            _mydBContext.SaveChanges();
            return city;
        }


        public async Task<City> DeleteCity(Guid cityId)
        {
            var result = await _mydBContext.cities.FirstOrDefaultAsync(x => x.City_Id == cityId);
            if (result != null)
            {
                _mydBContext.Remove(result);
                await _mydBContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<City>> GetAllCities()
        {
            var cities = _mydBContext.cities
                    .Include(c => c.Areas)
                    .ToList();

            // 🔥 Map Areas → AreaName (for View usage)
            foreach (var city in cities)
            {
                city.AreaName = city.Areas?
                    .Select(a => a.AreaName)
                    .ToList();
            }

            return cities;
        }

        public async Task<City> GetCityById(Guid city_Id)
        {
            return await _mydBContext.cities.FirstOrDefaultAsync(x => x.City_Id == city_Id);
        }

        public async Task<City> UpdateCity(City city)
        {
            var result = await _mydBContext.cities.FirstOrDefaultAsync(x => x.City_Id == city.City_Id);
            if (result != null)
            {
                result.City_Id = city.City_Id;
                result.City_Name = city.City_Name;
                result.Areas = city.Areas;
                await _mydBContext.SaveChangesAsync();
            }
            return result;
        }
    }
}
