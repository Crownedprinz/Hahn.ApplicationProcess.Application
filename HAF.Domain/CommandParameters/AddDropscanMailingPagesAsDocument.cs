using System;
using System.Collections.Generic;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddDropscanMailingPagesAsDocument
    {
        public AddDropscanMailingPagesAsDocument(int mailingId, Document document, IEnumerable<int> pageNumbers)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            PageNumbers = pageNumbers ?? throw new ArgumentNullException(nameof(pageNumbers));
            MailingID = mailingId;
        }

        public Document Document { get; }
        public int MailingID { get; }
        public IEnumerable<int> PageNumbers { get; }
    }
}