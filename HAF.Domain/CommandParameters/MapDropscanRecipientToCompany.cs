using System;

namespace HAF.Domain.CommandParameters
{
    public class MapDropscanRecipientToCompany
    {
        public MapDropscanRecipientToCompany(string recipientName, string companyName)
        {
            CompanyName = companyName ?? throw new ArgumentNullException(nameof(companyName));
            RecipientName = recipientName ?? throw new ArgumentNullException(nameof(recipientName));
        }

        public string CompanyName { get; }
        public string RecipientName { get; }
    }
}