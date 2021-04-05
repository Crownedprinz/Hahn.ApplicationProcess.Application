using System.Linq;
using HAF.Domain;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace  HAF.DAL.Queries
{
    public class QueryDropscanMailingPages : IQuery<PagesOfDropscanMailing, DropscanMailingPages>
    {
        public DropscanMailingPages Execute(PagesOfDropscanMailing parameters)
        {
            using (var context = new DatabaseContext())
            {
                var allPages = context.DropscanMailingPages.Where(x => x.MailingID == parameters.MailingID).ToArray();
                var mappedPages = context.MappedDropscanMailingPages.Where(x => x.MailingID == parameters.MailingID)
                    .ToArray();
                var discardedPages = context.DiscardedDropscanMailingPages.Where(x => x.MailingID == parameters.MailingID)
                    .ToArray();

                return new DropscanMailingPages(
                    allPages.Where(x => mappedPages.Any(y => y.PageNumber == x.PageNumber)).ToArray(),
                    allPages.Where(x => discardedPages.Any(y => y.PageNumber == x.PageNumber)).ToArray(),
                    allPages.Where(
                            x => mappedPages.All(y => y.PageNumber != x.PageNumber) &&
                                 discardedPages.All(y => y.PageNumber != x.PageNumber))
                        .ToArray());
            }
        }
    }
}