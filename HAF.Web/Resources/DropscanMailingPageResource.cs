using System;

namespace HAF.Web.Resources
{
    public class DropscanMailingPageResource
    {
        public int MailingID { get; set; }
        public Uri PageLink { get; set; }
        public int PageNumber { get; set; }
        public PageType PageType { get; set; }
    }
}