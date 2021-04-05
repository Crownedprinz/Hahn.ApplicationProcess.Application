using HAF.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAF.Web.Resources
{
    public class AddOrUpdateAssetResource
    {
        /// <summary>
        /// Asset Name
        /// </summary>
        /// <example>Biology Research</example>
        public string AssetName { get; set; }
        public DepartmentStatus? Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EMailAdressOfDepartment { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool? broken { get; set; }
    }
}