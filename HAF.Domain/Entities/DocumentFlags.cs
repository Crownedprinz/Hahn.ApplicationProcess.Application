namespace HAF.Domain.Entities
{
    public class DocumentFlag : NamedEntity
    {
        public Company Company { get; set; }
        public int CompanyID { get; set; }
    }
}