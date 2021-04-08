using HAF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace  HAF.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<Asset> Asset { get; set; }
    }
}