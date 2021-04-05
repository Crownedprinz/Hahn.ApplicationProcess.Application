using  HAF.Domain.Entities;

namespace  HAF.Domain.QueryParameters
{
    public class MailingForPageCreation : IQueryParameters<DropscanMailing>
    {
        public MailingForPageCreation(int id) => ID = id;
        public int ID { get; set; }
    }
}