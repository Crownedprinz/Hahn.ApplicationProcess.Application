using FluentValidation;
using HAF.Domain.Connectors;
using HAF.Domain.Entities;
using System;
using System.Collections.Generic;

namespace HAF.Domain.Validators
{
    public class AssetValidator: AbstractValidator<Asset> 
    {
        public AssetValidator()
        {
            RuleSet("all", () =>
            {
                RuleFor(asset => asset.AssetName).MinimumLength(5).WithMessage("Minimum Length for Asset Name is 5");
                RuleFor(asset => asset.Department).NotNull().WithMessage("Department is not valid").IsInEnum().WithMessage("Department is not valid");
                RuleFor(asset => asset.EMailAdressOfDepartment).EmailAddress().WithMessage("Email Address is not a valid Email");
                RuleFor(asset => asset.broken).Must(x => x == null || (x == true || x == false)).WithMessage("Broken is required");
                RuleFor(asset => asset.PurchaseDate).Must(IsValidPurchaseDate).WithMessage("Purchase date must not be more than 1 year old");
                RuleFor(asset => asset.CountryOfDepartment).Must(IsValidCountry).WithMessage("Please specify a valid country");
            });
        }

        private bool IsValidCountry(string country)
        {
            var result =  CountryApi.GetCountriesByName(country);
            if (result != null) return true;
            return false;
        }

        private bool IsValidPurchaseDate(DateTime? date)
        {
            if (date >= DateTime.Today.AddYears(-1)) return true;
            return false;
        }
    }

}
