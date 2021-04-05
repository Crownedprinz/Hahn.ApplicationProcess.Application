using HAF.Domain.Entities;

namespace HAF.Domain.QueryParameters
{
    public class DropscanMailingPageByPageNumber : IQueryParameters<DropscanMailingPage>
    {
        public DropscanMailingPageByPageNumber(int mailingId, int pageNumber)
        {
            PageNumber = pageNumber;
            MailingID = mailingId;
        }

        public int MailingID { get; }
        public int PageNumber { get; }
    }
}