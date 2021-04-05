using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAF.Domain.Entities
{

    public class Asset : NamedEntity
    {
        public string AssetName { get; set; }
        public DepartmentStatus? Department { get; set; }
        public string CountryOfDepartment { get; set; }
        public string EMailAdressOfDepartment { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool? broken { get; set; }
    }

}
