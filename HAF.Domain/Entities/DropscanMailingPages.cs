using System;
using System.Collections.Generic;

namespace HAF.Domain.Entities
{
    public class DropscanMailingPages
    {
        public DropscanMailingPages(
            IEnumerable<DropscanMailingPage> mappedPages,
            IEnumerable<DropscanMailingPage> discardedPages,
            IEnumerable<DropscanMailingPage> unmappedPages)
        {
            MappedPages = mappedPages ?? throw new ArgumentNullException(nameof(mappedPages));
            DiscardedPages = discardedPages ?? throw new ArgumentNullException(nameof(discardedPages));
            UnmappedPages = unmappedPages ?? throw new ArgumentNullException(nameof(unmappedPages));
        }

        public IEnumerable<DropscanMailingPage> DiscardedPages { get; }
        public IEnumerable<DropscanMailingPage> MappedPages { get; }
        public IEnumerable<DropscanMailingPage> UnmappedPages { get; }
    }
}