using Api.Core.DomainModels;
using Api.Core.Interfaces;
using Api.Infrastructure.Repostitories;
using Api.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/countries")]
    public class CountryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper, IUnitOfWork unitOfWork,IUrlHelper urlHelper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CountryResourceParamenters paramenters)
        {
            var countries = await _countryRepository.GetCountriesAsync(paramenters);
            var countryResources = _mapper.Map<List<CountryResource>>(countries);
            var previousLink = countries.HasPrevious ? CreateCountryUri(paramenters, PaginationResourceUriType.PreviousPage) : null;
            var nextLink = countries.HasNext ? CreateCountryUri(paramenters, PaginationResourceUriType.NextPage) : null;
            var meta = new
            {
                countries.TotalItemsCount,
                countries.PageinationBase.PageSize,
                countries.PageinationBase.PageIndex,
                countries.PageCount,
                previousLink,
                nextLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));    
            return Ok(countryResources);
        }

        [HttpGet("{id}",Name ="GetCountry")]
        public async Task<IActionResult> GetCountry(int id,bool includeCities=false)
        {
            var country = await _countryRepository.GetCountryByIdAsync(id,includeCities);
            if (country == null)
            {
                return NotFound();
            }
            var countryResource = _mapper.Map<CountryResource>(country);
            return Ok(countryResource);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody]CountryAddResource country)
        {
            if (country == null)
            {
                return BadRequest();
            }
            var countryModel = _mapper.Map<Country>(country);
            _countryRepository.AddCountry(countryModel);
            if(!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "保存数据出错");
            }
            var countryResource = _mapper.Map<CountryResource>(countryModel);
            return CreatedAtRoute("GetCountry", new { id = countryModel.Id }, countryResource);
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> BlockCreatingCountry(int id)
        {
            var country = await _countryRepository.GetCountryByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status409Conflict);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countryRepository.GetCountryByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            _countryRepository.DeleteCountry(country);
            if(!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, $"删除时，id为{id}的数据出错");
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id,[FromBody]CountryUpdateResource countryUpdate)
        {
            if (countryUpdate == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            var country = await _countryRepository.GetCountryByIdAsync(id, true);
            if (country == null)
            {
                return NotFound();
            }

            //Remove
            var countryUpdateCityIds = countryUpdate.Cities.Select(x => x.Id).ToList();
            var removedCities = country.Cities.Where(c => !countryUpdateCityIds.Contains(c.Id)).ToList();
            foreach(var city in removedCities)
            {
                country.Cities.Remove(city);
            }

            //Add
            var addedCityResources = countryUpdate.Cities.Where(x => x.Id == 0);
            var addedCities = _mapper.Map<IEnumerable<City>>(addedCityResources);
            foreach(var city in addedCities)
            {
                country.Cities.Add(city);
            }

            //Update or Unchanged
            var maybeUpdateCities = country.Cities.Where(x => x.Id != 0).ToList();
            foreach(var city in maybeUpdateCities)
            {
                var cityResource = countryUpdate.Cities.Single(x => x.Id == city.Id);
                _mapper.Map(cityResource, city);
            }
            if(!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"更新id为{id}的Country数据时候出错！！");
            }
            return NoContent();
        }

        private string CreateCountryUri(CountryResourceParamenters pageination, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParamenters = pageination.Clone();
                    previousParamenters.PageIndex--;
                    return _urlHelper.Link("GetCountries", previousParamenters);
                case PaginationResourceUriType.NextPage:
                    var nextParamenters = pageination.Clone();
                    nextParamenters.PageIndex++;
                    return _urlHelper.Link("GetCountries", nextParamenters);
                default:
                    return _urlHelper.Link("GetCountries", pageination);
            }
        }
    }
}
