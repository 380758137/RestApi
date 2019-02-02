using Api.Core.DomainModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Infrastructure.Repostitories
{
    public class CountryRepository: ICountryRepository
    {
        private readonly MyDbContext _myDbContext;
        public CountryRepository(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public void AddCountry(Country country)
        {
            _myDbContext.Countries.Add(country);
        }

        public async Task<bool> CountryExistAsync(int countryId)
        {
            return await _myDbContext.Countries.AnyAsync(x => x.Id == countryId);
        }

        public void DeleteCountry(Country country)
        {
            _myDbContext.Countries.Remove(country);
        }

        public async Task<PaginatedList<Country>> GetCountriesAsync(CountryResourceParamenters paramenters)
        {
            var query = _myDbContext.Countries.OrderBy(x => x.Id);
            var count = await query.CountAsync();
            var items = await query.Skip(paramenters.PageSize * paramenters.PageIndex)
                .Take(paramenters.PageSize).ToListAsync();
            return new PaginatedList<Country>(paramenters.PageIndex, paramenters.PageSize, count, items);
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync(IEnumerable<int> ids)
        {
            return await _myDbContext.Countries.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<Country> GetCountryByIdAsync(int id,bool includeCities=false)
        {
            if (!includeCities)
            {
                return await _myDbContext.Countries.FindAsync(id);
            }
            return await _myDbContext.Countries.Include(x => x.Cities).SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
