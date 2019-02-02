using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resources.Validators
{
    public class CityUpdateResourceValidator:CityAddOrUpdateResourceValidator<CityUpdateResource>
    {
        public CityUpdateResourceValidator()
        {
            RuleFor(c => c.Description).NotEmpty().WithName("描述").WithMessage("{PropertyName}是必填项");
        }
    }
}
