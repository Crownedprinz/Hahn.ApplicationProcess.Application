using System;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddDebitorCreditor
    {
        public AddDebitorCreditor(DebitorCreditor debitorCreditor)
        {
            DebitorCreditor = debitorCreditor ?? throw new ArgumentNullException(nameof(debitorCreditor));
        }

        public DebitorCreditor DebitorCreditor { get; }
    }
}