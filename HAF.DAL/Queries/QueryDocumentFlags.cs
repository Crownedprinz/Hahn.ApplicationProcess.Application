using System.Linq;
using HAF.Domain.Entities;

namespace  HAF.DAL.Queries
{
    public class QueryDocumentFlags : QueryEntities<DocumentFlag>
    {
        protected override IQueryable<DocumentFlag> CreateQuery(DatabaseContext context) => context.DocumentFlags;
    }
}