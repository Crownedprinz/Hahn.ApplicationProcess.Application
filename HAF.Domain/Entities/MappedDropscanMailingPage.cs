namespace HAF.Domain.Entities
{
    public class MappedDropscanMailingPage : Entity
    {
        public Document Document { get; set; }
        public int DocumentID { get; set; }
        public DropscanMailing Mailing { get; set; }
        public int MailingID { get; set; }
        public int PageNumber { get; set; }
    }
}