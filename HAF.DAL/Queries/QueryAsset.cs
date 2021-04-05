using HAF.DAL;
using HAF.DAL.Queries;
using HAF.Domain;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAF.Domain.Queries
{

    public class QueryAsset : QueryEntities<Asset>, IQuery<QueryAssetWithData, Asset>
    {
        public Asset Execute(QueryAssetWithData parameters)
        {
            using (var context = new DatabaseContext())
            {
                var query = CreateQuery(context);

                return query.SingleOrDefault(x => x.ID == parameters.ID);
            }
        }

        protected override IQueryable<Asset> CreateQuery(DatabaseContext context)
        {
            List<Asset> assets = new List<Asset>()
            {


            };
            IQueryable<Asset> query = assets.AsQueryable();
            return query;
        }
    }

}
