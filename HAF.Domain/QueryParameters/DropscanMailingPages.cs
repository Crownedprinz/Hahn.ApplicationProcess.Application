using HAF.Domain.Entities;

namespace HAF.Domain.QueryParameters
{
    public class PagesOfDropscanMailing : IQueryParameters<DropscanMailingPages>
    {
        public PagesOfDropscanMailing(int mailingId) => MailingID = mailingId;
        public int MailingID { get; }
    }
}