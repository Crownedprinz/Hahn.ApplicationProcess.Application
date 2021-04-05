namespace HAF.Domain.Entities
{
    public class DropscanRecipient : NamedEntity
    {
        public Company CorrespondingCompany { get; set; }
        public int? CorrespondingCompanyID { get; set; }
        public int ExternalID { get; set; }
    }
}