using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resources
{
    public class CityUpdateResource:CityAddOrUpdateResource
    {
        public override string Description { get; set; }
    }
}
