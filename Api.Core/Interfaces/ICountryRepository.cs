using Api.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Repostitories
{
    public interface ICountryRepository
    {
        Task<PaginatedList<Country>> GetCountriesAsync(CountryResourceParamenters paramenters);
        void AddCountry(Country country);
        Task<Country> GetCountryByIdAsync(int id,bool includeCities=false);
        Task<bool> CountryExistAsync(int countryId);
        Task<IEnumerable<Country>> GetCountriesAsync(IEnumerable<int> ids);
        void DeleteCountry(Country country);
    }
}
