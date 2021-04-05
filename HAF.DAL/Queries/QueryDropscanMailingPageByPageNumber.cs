using System.Data.Entity;
using System.Linq;
using HAF.Domain;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace  HAF.DAL.Queries
{
    public class QueryDropscanMailingPageByPageNumber : IQuery<DropscanMailingPageByPageNumber, DropscanMailingPage>
    {
        public DropscanMailingPage Execute(DropscanMailingPageByPageNumber parameters)
        {
            using (var context = new DatabaseContext())
            {
                return context.DropscanMailingPages.Include(x => x.PageData)
                    .SingleOrDefault(x => x.MailingID == parameters.MailingID && x.PageNumber == parameters.PageNumber);
            }
        }
    }
}