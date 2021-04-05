using System;
using System.Data.Entity;
using System.Data.SqlClient;

namespace  HAF.DAL
{
    public class ForceDropCreateDatabaseInitializer : IDatabaseInitializer<DatabaseContext>
    {
        private readonly DropCreateDatabaseAlways<DatabaseContext> _delegatee =
            new DropCreateDatabaseAlways<DatabaseContext>();

        public void InitializeDatabase(DatabaseContext context)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(context.Database.Connection.ConnectionString))
                {
                    var cmd = new SqlCommand(
                        "ALTER DATABASE [" + GetDatabaseName(context) + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
                        sqlConnection);
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't alter database");
                Console.WriteLine(e.Message);
            }

            context.Database.CommandTimeout = 300;
            _delegatee.InitializeDatabase(context);
        }

        private static string GetDatabaseName(DbContext context) =>
            new SqlConnectionStringBuilder(context.Database.Connection.ConnectionString).InitialCatalog;
    }
}