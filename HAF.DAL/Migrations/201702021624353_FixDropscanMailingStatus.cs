using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class FixDropscanMailingStatus : DbMigration
    {
        public override void Down()
        {
            DropColumn("dbo.DropscanMailings", "MappingStatus");
        }

        public override void Up()
        {
            AddColumn("dbo.DropscanMailings", "MappingStatus", c => c.Int(nullable: false));
        }
    }
}