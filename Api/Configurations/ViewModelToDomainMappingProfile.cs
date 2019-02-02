using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Configurations
{
    public class ViewModelToDomainMappingProfile:Profile
    {
        public override string ProfileName => "ViewModelToDomainMappings";
        public ViewModelToDomainMappingProfile()
        {
            //CreateMap<ProductViewModel, Product>();
        }
    }
}
