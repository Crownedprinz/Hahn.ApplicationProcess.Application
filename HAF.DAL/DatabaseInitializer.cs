using System.Data.Entity;
using  HAF.DAL.Migrations;

namespace  HAF.DAL
{
    public class DatabaseInitializer : IDatabaseInitializer<DatabaseContext>
    {
        private readonly MigrateDatabaseToLatestVersion<DatabaseContext, Configuration> _delegatee =
            new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>();

        public void InitializeDatabase(DatabaseContext context)
        {
            _delegatee.InitializeDatabase(context);
        }
    }
}