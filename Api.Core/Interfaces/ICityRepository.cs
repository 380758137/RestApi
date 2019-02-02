using Api.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Interfaces
{
    public interface ICityRepository
    {
        Task<List<City>> GetCitiesForCountryAsync(int countryId);
        Task<City> GetCityForCountryAsync(int countryId, int cityId);
        void AddCity(int countryId, City city);
        void DeleteCity(City city);
        void UpdateCityForCountry(City city);
    }
}
