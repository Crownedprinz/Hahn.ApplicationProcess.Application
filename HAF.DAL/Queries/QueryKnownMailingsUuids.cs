using System.Collections.Generic;
using System.Linq;
using HAF.Domain;
using HAF.Domain.QueryParameters;

namespace  HAF.DAL.Queries
{
    public class QueryKnownMailingsUuids : IQuery<KnownMailingsUuids, IEnumerable<string>>
    {
        public IEnumerable<string> Execute(KnownMailingsUuids parameters)
        {
            using (var context = new DatabaseContext())
            {
                return context.DropscanMailings.AsQueryable().Select(x => x.Uuid).ToList();
            }
        }
    }
}