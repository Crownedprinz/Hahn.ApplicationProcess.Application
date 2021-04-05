using System;
using System.Collections.Generic;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddRecipients
    {
        public AddRecipients(IEnumerable<DropscanRecipient> recipientsToAdd)
        {
            RecipientsToAdd = recipientsToAdd ?? throw new ArgumentNullException(nameof(recipientsToAdd));
        }

        public IEnumerable<DropscanRecipient> RecipientsToAdd { get; }
    }
}