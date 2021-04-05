using System;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddDropscanMailingAsDocument
    {
        public AddDropscanMailingAsDocument(int mailingId, Document document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            MailingID = mailingId;
        }

        public Document Document { get; }
        public int MailingID { get; }
    }
}