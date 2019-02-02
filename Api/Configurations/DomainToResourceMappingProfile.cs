using Api.Core.DomainModels;
using Api.Resources;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Configurations
{
    public class DomainToResourceMappingProfile:Profile
    {
        public override string ProfileName { get; } = "DomainToResourceMappings";

        public DomainToResourceMappingProfile()
        {
            CreateMap<Country, CountryResource>();
        }
    }
}
