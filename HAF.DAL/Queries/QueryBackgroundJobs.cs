using System.Linq;
using HAF.Domain.Entities;

namespace  HAF.DAL.Queries
{
    public class QueryBackgroundJobs : QueryEntities<BackgroundJob>
    {
        protected override IQueryable<BackgroundJob> CreateQuery(DatabaseContext context) => context.BackgroundJobs;
    }
}