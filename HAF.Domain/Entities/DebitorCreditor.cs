namespace HAF.Domain.Entities
{
    public class DebitorCreditor : NamedEntity
    {
        public Company Company { get; set; }
        public int CompanyID { get; set; }
        public Folder RootFolder { get; set; }
        public int RootFolderID { get; set; }
    }
}