using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.DomainModels
{
    public class CountryResourceParamenters:PageinationBase
    {
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }

        public CountryResourceParamenters Clone()
        {
            return new CountryResourceParamenters
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                MaxPageSize = MaxPageSize,
                EnglishName = EnglishName,
                ChineseName = ChineseName
            };
        }
    }
}
