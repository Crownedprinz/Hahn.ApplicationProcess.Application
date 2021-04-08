using FluentValidation;
using HAF.Domain.Entities;
using HAF.Domain.Validators;
using HAF.Domain.Services;

namespace HAF.Domain.Services
{

   

    public class AssetService : IAssetService
    {
        
        public (bool flag, string errors) validateAsset(Asset dto)
        {
            var validator = new AssetValidator();
            var res = validator.Validate(dto, options => options.IncludeRuleSets("all"));
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
