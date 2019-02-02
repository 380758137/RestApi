using Api.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resources
{
    public class CountryResource
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public string Abbreviation { get; set; }
        public ICollection<CityResource> Cities { get; set; }
    }
}
