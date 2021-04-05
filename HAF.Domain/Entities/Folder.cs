using System.Collections.Generic;

namespace HAF.Domain.Entities
{
    public class Folder : NamedEntity
    {
        public List<Document> Files { get; set; }
        public List<Folder> Folders { get; set; }
        public Folder ParentFolder { get; set; }
        public int? ParentFolderID { get; set; }
    }
}