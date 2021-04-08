using System.Collections.Generic;
using System.Linq;
using HAF.Domain;
using HAF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace  HAF.DAL.Queries
{
    
    public class QueryEntities : IQueryAll
    {
        DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
           .UseInMemoryDatabase(databaseName: "Test")
           .Options;
        public IEnumerable<Asset> Execute()
        {
            using (var context = new DatabaseContext(options))
            {
                var items = context.Asset.ToList();
                return items;
         }
    }


        public Asset ExecuteOne(int id)
        {
            using (var context = new DatabaseContext(options))
            {
                var items = context.Asset.Where(x => x.ID == id).FirstOrDefault();
                return items;
            }
        }
    }
}