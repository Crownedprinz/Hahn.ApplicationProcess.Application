using HAF.Domain.Entities;
using HAF.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hahn.ApplicationProcess.February2021.Web.Models
{

    public class Asset: Entity
    {
       
        public string AssetName { get; set; }
        public Department? Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EmailAddressOfDepartment { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool broken { get; set; }

        public override string ToString()
        {
            var result = string.Empty;

            if (PurchaseDate != null)
                result += PurchaseDate.Value.ToString("yyyy-MM-dd");

            return result;
        }
    }

}