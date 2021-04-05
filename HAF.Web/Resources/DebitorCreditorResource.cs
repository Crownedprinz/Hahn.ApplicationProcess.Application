namespace HAF.Web.Resources
{
    public class DebitorCreditorResource : NamedEntityResource
    {
        public int CompanyID { get; set; }
        public FolderResource RootFolder { get; set; }
    }
}