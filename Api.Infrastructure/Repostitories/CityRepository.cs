using Api.Core.DomainModels;
using Api.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Repostitories
{
    public class CityRepository : ICityRepository
    {
        private readonly MyDbContext _myDbContext;
        public CityRepository(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public void AddCity(int countryId, City city)
        {
            city.CountryId = countryId;
            _myDbContext.Cities.Add(city);
        }

        public void DeleteCity(City city)
        {
            _myDbContext.Cities.Remove(city);
        }

        public async Task<List<City>> GetCitiesForCountryAsync(int countryId)
        {
            return await _myDbContext.Cities.Where(x => x.CountryId == countryId).ToListAsync();   
        }

        public async Task<City> GetCityForCountryAsync(int countryId, int cityId)
        {
            return await _myDbContext.Cities.SingleOrDefaultAsync(x => x.CountryId == countryId && x.Id == cityId);
        }

        public void UpdateCityForCountry(City city)
        {
            _myDbContext.Update(city);
        }
    }
}
