using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.DomainModels
{
    public class Country
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public string Abbreviation { get; set; }
        public ICollection<City> Cities { get; set; }
    }
    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
}
