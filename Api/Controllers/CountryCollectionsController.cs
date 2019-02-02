using Api.Configurations;
using Api.Core.DomainModels;
using Api.Core.Interfaces;
using Api.Infrastructure.Repostitories;
using Api.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/countrycollections")]
    public class CountryCollectionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryCollectionsController(ICountryRepository countryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountryCollection([FromBody]IEnumerable<CountryAddResource> countries)
        {
            if (countries == null)
            {
                return BadRequest();
            }
            var countriesModel = _mapper.Map<IEnumerable<Country>>(countries);
            foreach(var country in countriesModel)
            {
                _countryRepository.AddCountry(country);
            }
            if(!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "保存出错");
            }
            var countryResources = _mapper.Map<IEnumerable<CountryResource>>(countriesModel);
            var idsStr = string.Join(",", countryResources.Select(x => x.Id));
            return CreatedAtRoute("GetCountryCollection", new { ids = idsStr }, countryResources);
        }

        [HttpGet("({ids})",Name ="GetCountryCollection")]
        public async Task<IActionResult> GetCountryCollection([ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var countries = await _countryRepository.GetCountriesAsync(ids);
            if (ids.Count() != countries.Count())
            {
                return NotFound();
            }
            var countryResources = _mapper.Map<IEnumerable<CountryResource>>(countries);
            return Ok(countryResources);
        }
    }
}
