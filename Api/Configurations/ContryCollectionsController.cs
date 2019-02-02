using Api.Core.Interfaces;
using Api.Infrastructure.Repostitories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Configurations
{
    [Route("api/countrycollections")]
    public class ContryCollectionsController:Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public ContryCollectionsController(ICountryRepository countryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
    }
}
