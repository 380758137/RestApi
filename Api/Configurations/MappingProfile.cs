using Api.Core.DomainModels;
using Api.Resources;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Configurations
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryResource>();
            CreateMap<CountryResource, Country>();
            CreateMap<City, CityResource>();
            CreateMap<CityResource, City>();
            CreateMap<CountryAddResource, Country>();
            CreateMap<Country, CountryAddResource>();
            CreateMap<CityAddResource, City>();
            CreateMap<City, CityAddResource>();
            CreateMap<CityUpdateResource, City>();
            CreateMap<City, CityUpdateResource>();
        }
    }
}
