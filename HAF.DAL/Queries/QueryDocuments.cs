using System.Data.Entity;
using System.Linq;
using HAF.Domain;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace  HAF.DAL.Queries
{
    public class QueryDocuments : QueryEntities<Document>, IQuery<QueryDocumentWithData, Document>
    {
        public Document Execute(QueryDocumentWithData parameters)
        {
            using (var context = new DatabaseContext())
            {
                var query = CreateQuery(context);
                if (parameters.GetContextData)
                    query = query.Include(x => x.ContextData);
                if (parameters.GetDocumentData)
                    query = query.Include(x => x.DocumentData);

                return query.SingleOrDefault(x => x.ID == parameters.ID);
            }
        }

        protected override IQueryable<Document> CreateQuery(DatabaseContext context)
        {
            return context.Documents.Include(x => x.Folder)
                .Include(x => x.DebitorCreditor.Company)
                .Include(x => x.AssignedFlags);
        }
    }
}