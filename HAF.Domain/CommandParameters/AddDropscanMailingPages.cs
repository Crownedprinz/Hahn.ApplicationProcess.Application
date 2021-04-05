using System;
using System.Collections.Generic;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddDropscanMailingPages
    {
        public AddDropscanMailingPages(IEnumerable<DropscanMailingPage> pages, bool removeExistingPages = true)
        {
            Pages = pages ?? throw new ArgumentNullException(nameof(pages));
            RemoveExistingPages = removeExistingPages;
        }

        public IEnumerable<DropscanMailingPage> Pages { get; }
        public bool RemoveExistingPages { get; }
    }
}