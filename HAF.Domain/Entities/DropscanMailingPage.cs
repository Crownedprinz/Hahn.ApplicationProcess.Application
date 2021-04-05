namespace HAF.Domain.Entities
{
    public class DropscanMailingPage : Entity
    {
        public DropscanMailing Mailing { get; set; }
        public int MailingID { get; set; }
        public string PageContentType { get; set; }
        public DocumentData PageData { get; set; }
        public int PageNumber { get; set; }
    }
}