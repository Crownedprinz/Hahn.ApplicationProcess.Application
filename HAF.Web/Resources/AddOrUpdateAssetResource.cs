using HAF.Domain.Entities;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace HAF.Web.Resources
{
    public class AddOrUpdateAssetResource
    {
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>Stathoscope</example>
        public string AssetName { get; set; }
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>HQ</example>
        [JsonConverter(typeof(StringEnumConverter))]
        public DepartmentStatus? Department { get; set; }
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>Nigeria</example>
        public string CountryOfDepartment { get; set; }
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>support@haf.com</example>
        public string EMailAdressOfDepartment { get; set; }
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>2021-01-01</example>
        public DateTime? PurchaseDate { get; set; }
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>true</example>
        public bool? broken { get; set; }
    }
}