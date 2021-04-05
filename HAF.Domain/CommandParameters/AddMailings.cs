using System;
using System.Collections.Generic;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddMailings
    {
        public AddMailings(IEnumerable<DropscanMailing> mailingsToAdd)
        {
            MailingsToAdd = mailingsToAdd ?? throw new ArgumentNullException(nameof(mailingsToAdd));
        }

        public IEnumerable<DropscanMailing> MailingsToAdd { get; }
    }
}