
using HAF.Domain.Entities;
using Swagger.Net.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAF.Web.Resources
{
    public class AssetExample: IExamplesProvider
    {
        public object GetExamples()
        {
            return new AddOrUpdateAssetResource
            {
                AssetName = "Robot",
                broken = true,
                CountryOfDepartment = "Nigeria",
                Department = DepartmentStatus.HQ,
                EMailAdressOfDepartment = "example@example.com",
                PurchaseDate = DateTime.Today.AddMonths(-4) 
            };
        }
    }
}