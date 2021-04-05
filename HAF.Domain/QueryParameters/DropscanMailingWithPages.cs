using HAF.Domain.Entities;

namespace HAF.Domain.QueryParameters
{
    public class DropscanMailingWithPages : IQueryParameters<DropscanMailing>
    {
        public DropscanMailingWithPages(int mailingId) => MailingID = mailingId;
        public int MailingID { get; }
    }
}