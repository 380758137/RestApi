using Api.Core.DomainModels;
using Api.Core.Interfaces;
using Api.Infrastructure;
using Api.Infrastructure.Repostitories;
using Api.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Value")]
    public class ValueController:Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public ValueController(ICountryRepository countryRepository,IMapper mapper,IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var newCountry = new Country
        //    {
        //        ChineseName = "俄罗斯",
        //        EnglishName = "Russia",
        //        Abbreviation = "Russia"
        //    };
        //    _countryRepository.AddCountry(newCountry);
        //    await _unitOfWork.SaveAsync();
        //    var countries = await _countryRepository.GetCountriesAsync();
        //    var countryResources = _mapper.Map<List<CountryResource>>(countries);
        //    return Ok(countryResources);
        //}
    }
}
