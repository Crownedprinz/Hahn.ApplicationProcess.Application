namespace HAF.Domain.Entities
{
    public class DiscardedDropscanMailingPage : Entity
    {
        public DropscanMailing Mailing { get; set; }
        public int MailingID { get; set; }
        public int PageNumber { get; set; }
    }
}