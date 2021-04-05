using System;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddDocument
    {
        public AddDocument(Document document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public Document Document { get; }
    }
}