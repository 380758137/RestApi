using Api.Core.DomainModels;
using Api.Core.Interfaces;
using Api.Infrastructure.Repostitories;
using Api.Resources;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/countries/{countryId}/cities")]
    public class CityController:Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ICityRepository _cityRepository;
        public CityController(ICountryRepository countryRepository, IMapper mapper, IUnitOfWork unitOfWork,ICityRepository cityRepository)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesForCountry(int countryId)
        {
            if(!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound(); 
            }
            var citiesForCountry = await _cityRepository.GetCitiesForCountryAsync(countryId);
            var citiesResources = _mapper.Map<IEnumerable<CityResource>>(citiesForCountry);
            return Ok(citiesForCountry);
        }

        [HttpGet("{cityId}",Name ="GetCity")]
        public async Task<IActionResult> GetCityForCountry(int countryId,int cityId)
        {
            if(!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }
            var cityForCountry = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (cityForCountry == null)
            {
                return NotFound();
            }
            var cityResource = _mapper.Map<CityResource>(cityForCountry);
            return Ok(cityResource);
        }

        [HttpPost]
        public async Task<IActionResult> CteateCityForCountry(int countryId,[FromBody]CityAddResource city)
        {
            if (city == null)
            {
                return BadRequest();
            }
            if(!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }
            var cityModel = _mapper.Map<City>(city);
            _cityRepository.AddCity(countryId, cityModel);
            if(!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "保存错误");
            }
            var cityResource = _mapper.Map<CityResource>(cityModel);
            return CreatedAtRoute("GetCity", new { countryId, cityId = cityResource.Id }, cityResource);
        }

        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCityForCountry(int countryId,int cityId)
        {
            if(!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }
            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                return NotFound();
            }
            _cityRepository.DeleteCity(city);
            if(!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, $"Deketing city {cityId} for country {countryId} failed when saving");
            }
            return NoContent();
        }

        [HttpPut("{cityId}")]
        public async Task<IActionResult> UpdateCityForCountry(int countryId,int cityId,[FromBody]CityUpdateResource cityUpdate)
        {
            if (cityUpdate == null)
            {
                return BadRequest();
            }
            if(!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }
            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                return NotFound();
            }
            _mapper.Map(cityUpdate, city);
            _cityRepository.UpdateCityForCountry(city);
            if(!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, $"更新Country表中Id为{countryId}的子表City，Id为{cityId}的数据时出错!");
            }
            return NoContent();
        }

        [HttpPatch("{cityId}")]
        public async Task<IActionResult> PartiallyUpdateCityForCountry(int countryId,int cityId,[FromBody]JsonPatchDocument<CityUpdateResource> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            if(!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }
            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                return NotFound();
            }
            var cityToPatch = _mapper.Map<CityUpdateResource>(city);
            //patchDocument.ApplyTo(cityToPatch,ModelState);
            patchDocument.ApplyTo(cityToPatch);
            TryValidateModel(cityToPatch);
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            _mapper.Map(cityToPatch, city);
            _cityRepository.UpdateCityForCountry(city);
            if(!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "保存出错");
            }
            return NoContent();
        }
    }
}
