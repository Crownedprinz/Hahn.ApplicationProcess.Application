namespace HAF.Web.Resources
{
    public class CompanyResource : NamedEntityResource
    {
        public DocumentFlagResource[] AllowedDocumentFlags { get; set; }
        public DebitorCreditorResource[] DebitorCreditors { get; set; }
    }
}