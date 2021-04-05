﻿using System;
using System.Collections.Generic;

namespace HAF.Domain.CommandParameters
{
    public class UndiscardPages
    {
        public UndiscardPages(int mailingId, IEnumerable<int> pageNumbers)
        {
            MailingID = mailingId;
            PageNumbers = pageNumbers ?? throw new ArgumentNullException(nameof(pageNumbers));
        }

        public int MailingID { get; }
        public IEnumerable<int> PageNumbers { get; }
    }
}