using HAF.Domain;
using HAF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace  HAF.DAL.Commands
{
    public class AssetCommand : ICommand
    {
        DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
           .UseInMemoryDatabase(databaseName: "Test")
           .Options;
        public void ExecuteAdd(Asset asset)
        {
           
            using (var context = new DatabaseContext(options))
            {
                context.Asset.Add(asset);
                context.SaveChanges();
            }
        }

        public void ExecuteUpdate(Asset asset)
        {
            using (var context = new DatabaseContext(options))
            {
                context.Asset.Update(asset);
                context.SaveChanges();
            }
        }

        public void ExecuteDelete(Asset asset)
        {
            using (var context = new DatabaseContext(options))
            {
                context.Asset.Remove(asset);
                context.SaveChanges();
            }
        }
    }
}