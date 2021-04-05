using System;
using HAF.Domain;

namespace HAF.Web.Resources
{
    public class AddOrUpdateDocumentResource
    {
        public int[] AssignedFlags { get; set; }
        public DateTime? Date { get; set; }
        public int? DebitorCreditorID { get; set; }
        public Direction? Direction { get; set; }
        public int? FolderID { get; set; }
        public string Name { get; set; }
    }
}