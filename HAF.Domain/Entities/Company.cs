using System.Collections.Generic;

namespace HAF.Domain.Entities
{
    public class Company : NamedEntity
    {
        public List<DocumentFlag> AllowedDocumentFlags { get; set; }
        public List<DebitorCreditor> DebitorCreditors { get; set; }
    }
}