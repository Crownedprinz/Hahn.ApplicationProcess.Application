using System;
using HAF.Domain;

namespace HAF.Web.Resources
{
    public class DocumentResource : NamedEntityResource
    {
        public DocumentFlagResource[] AssignedFlags { get; set; }
        public int? CompanyID { get; set; }
        public Uri Context { get; set; }
        public string ContextDataContentType { get; set; }
        public string ContextFileExtension { get; set; }
        public DateTime? Date { get; set; }
        public int? DebitorCreditorID { get; set; }
        public Direction Direction { get; set; }
        public Uri Document { get; set; }
        public string DocumentDataContentType { get; set; }
        public string DocumentFileExtension { get; set; }
        public int? FolderID { get; set; }
    }
}