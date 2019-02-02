using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resources
{
    public abstract class CityAddOrUpdateResource
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}
