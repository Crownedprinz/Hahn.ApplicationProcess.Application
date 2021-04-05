using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HAF.Domain;
using HAF.Domain.Entities;

namespace  HAF.DAL.Queries
{
    public abstract class QueryEntities<TEntity> : IQueryAll<TEntity>, IQuerySingle<TEntity> where TEntity : Entity
    {
        public virtual IEnumerable<TEntity> Execute()
        {
            using (var context = new DatabaseContext())
            {
                var items = CreateQuery(context).AsNoTracking().AsEnumerable().Select(PostProcess).ToList();
                Detach(context, items);
                return items;
            }
        }

        public virtual TEntity Execute(int id)
        {
            using (var context = new DatabaseContext())
            {
                var result = CreateQuery(context).SingleOrDefault(x => x.ID == id);
                if (result != null)
                    result = PostProcess(result);
                return result;
            }
        }

        protected abstract IQueryable<TEntity> CreateQuery(DatabaseContext context);
        protected virtual TEntity PostProcess(TEntity entity) => entity;

        private static void Detach(DbContext context, IEnumerable<TEntity> items)
        {
            foreach (var item in items)
                context.Entry(item).State = EntityState.Detached;
        }
    }
}