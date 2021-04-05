using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HAF.Domain;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace  HAF.DAL.Queries
{
    public class QueryDropscanMailings : QueryEntities<DropscanMailing>,
                                         IQuery<DropscanMailingsByStatus, IEnumerable<DropscanMailing>>,
                                         IQuery<DropscanMailingWithPages, DropscanMailing>,
                                         IQuery<MailingForPageCreation, DropscanMailing>,
                                         IQuery<QueryMailingForFile, DropscanMailing>,
                                         IQuery<QueryMailingWithData, DropscanMailing>
    {
        public IEnumerable<DropscanMailing> Execute(DropscanMailingsByStatus parameters)
        {
            using (var context = new DatabaseContext())
            {
                return CreateQuery(context)
                    .Where(x => parameters.DropscanMailingStatuses.Contains(x.MappingStatus))
                    .ToList();
            }
        }

        public DropscanMailing Execute(DropscanMailingWithPages parameters)
        {
            using (var context = new DatabaseContext())
            {
                return context.DropscanMailings.Include(x => x.Recipient.CorrespondingCompany)
                    .Include(x => x.Pages)
                    .Include(x => x.DiscardedPages)
                    .Include(x => x.MappedPages)
                    .SingleOrDefault(x => x.ID == parameters.MailingID);
            }
        }

        public DropscanMailing Execute(MailingForPageCreation parameters)
        {
            using (var context = new DatabaseContext())
            {
                return context.DropscanMailings.Include(x => x.FileData)
                    .Include(x => x.Pages)
                    .SingleOrDefault(x => x.ID == parameters.ID);
            }
        }

        public DropscanMailing Execute(QueryMailingForFile parameters)
        {
            using (var context = new DatabaseContext())
            {
                return context.DropscanMailings.Include(x => x.Recipient.CorrespondingCompany)
                    .Include(x => x.Pages)
                    .Include(x => x.DiscardedPages)
                    .Include(x => x.MappedPages)
                    .Include(x => x.FileData)
                    .SingleOrDefault(x => x.ID == parameters.ID);
            }
        }

        public DropscanMailing Execute(QueryMailingWithData parameters)
        {
            using (var context = new DatabaseContext())
            {
                var query = CreateQuery(context);
                if (parameters.GetEnvelopeData)
                    query = query.Include(x => x.EnvelopeData);
                if (parameters.GetFileData)
                    query = query.Include(x => x.FileData);

                return query.SingleOrDefault(x => x.ID == parameters.ID);
            }
        }

        protected override IQueryable<DropscanMailing> CreateQuery(DatabaseContext context)
        {
            return context.DropscanMailings.Include(x => x.Recipient.CorrespondingCompany);
        }
    }
}