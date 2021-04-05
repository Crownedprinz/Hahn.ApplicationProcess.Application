using FluentValidation;
using HAF.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAF.Domain.Services
{

   

    public class AssetService : IAssetService
    {
        private readonly IValidator<Asset> _validator;
        public AssetService(IValidator<Asset> validator)
        {
            _validator = validator;
        }
        public (bool flag, string errors) validateAsset(Asset dto)
        {
            var res = _validator.Validate(dto, options => options.IncludeRuleSets("all"));
            if (!res.IsValid)
            {
                return (false, res.ToString("~"));
            }
            else
            {
                return (true,string.Empty);
            }
        }
    }
}
