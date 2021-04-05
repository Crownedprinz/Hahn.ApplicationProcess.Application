using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HAF.Web.Controllers;
using HAF.Web.Resources;
using HAF.Domain.Entities;

namespace HAF.Web
{
    public static class DropscanMailingPagesResourceFactory
    {
        public static IEnumerable<DropscanMailingPageResource>
            Create(DropscanMailingPages pages, HttpRequestMessage request) =>
            Map(request, pages.DiscardedPages, PageType.Discarded)
                .Concat(Map(request, pages.MappedPages, PageType.Mapped))
                .Concat(Map(request, pages.UnmappedPages, PageType.Unmapped));

        private static DropscanMailingPageResource CreateDropscanMailingPageResource(
            HttpRequestMessage request,
            DropscanMailingPage page,
            PageType pageType)
        {
            return new DropscanMailingPageResource
            {
                MailingID = page.MailingID,
                PageLink = UrlHelperEx.GetLink<DropscanMailingsController>(
                    request,
                    c => c.GetPage(page.MailingID, page.PageNumber)),
                PageNumber = page.PageNumber,
                PageType = pageType
            };
        }

        private static IEnumerable<DropscanMailingPageResource> Map(
            HttpRequestMessage request,
            IEnumerable<DropscanMailingPage> pages,
            PageType pageType)
        {
            return pages.Select(x => CreateDropscanMailingPageResource(request, x, pageType));
        }
    }
}