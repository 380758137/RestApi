using Api.Core.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

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
            var query = _myDbContext.Countries.AsQueryable();
            if (!string.IsNullOrEmpty(paramenters.EnglishName))
            {
                var englishNameClause = paramenters.EnglishName.Trim().ToLowerInvariant();
                query = query.Where(x => x.EnglishName.ToLowerInvariant() == englishNameClause);
            }
            if(!string.IsNullOrEmpty(paramenters.ChineseName)){
                var chineseNameClause = paramenters.ChineseName.Trim().ToLowerInvariant();
                query = query.Where(x => x.ChineseName.ToLowerInvariant() == chineseNameClause);
            }
            var propertiesMap = new Dictionary<string, Expression<Func<Country, object>>>
            {
                { "id",c=>c.Id},
                { "EnglishName",c=>c.EnglishName},
                { "ChineseName",c=>c.ChineseName},
                { "Abbreviation",c=>c.Abbreviation}
            };
            if (!string.IsNullOrEmpty(paramenters.OrderBy))
            {
                var isDescending = paramenters.OrderBy.EndsWith(" desc");
                //var property = isDescending ? paramenters.OrderBy.Split(" ")[0] : paramenters.OrderBy;
                //query = query.OrderBy(property + (isDescending ? "descending" : " ascending"));
            }
            //if (!string.IsNullOrEmpty(paramenters.OrderBy))
            //{
            //    if(paramenters.OrderBy.EndsWith(" desc"))
            //    {
            //        var property = paramenters.OrderBy.Split(" ")[0];
            //        switch (property)
            //        {
            //            case "Id":
            //                query = query.OrderByDescending(x => x.Id);
            //                break;
            //            case "EnglishName":
            //                query = query.OrderByDescending(x => x.EnglishName);
            //                break;
            //            case "ChineseName":
            //                query = query.OrderByDescending(x => x.ChineseName);
            //                break;
            //            case "Abbreviation":
            //                query = query.OrderByDescending(x => x.Abbreviation);
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        var property = paramenters.OrderBy;
            //        switch (property)
            //        {
            //            case "Id":
            //                query = query.OrderBy(x => x.Id);
            //                break;
            //            case "EnglishName":
            //                query = query.OrderBy(x => x.EnglishName);
            //                break;
            //            case "ChineseName":
            //                query = query.OrderBy(x => x.ChineseName);
            //                break;
            //            case "Abbreviation":
            //                query = query.OrderBy(x => x.Abbreviation);
            //                break;
            //        }
            //    }
            //}
            query = query.OrderBy(x => x.Id);
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
