using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class ChangeStructure : DbMigration
    {
        public override void Down()
        {
            AddColumn("dbo.Folders", "DebitorCreditor_ID", c => c.Int());
            DropForeignKey("dbo.DebitorCreditors", "Root_ID", "dbo.Folders");
            DropIndex("dbo.DebitorCreditors", new[] { "Root_ID" });
            DropColumn("dbo.DebitorCreditors", "Root_ID");
            CreateIndex("dbo.Folders", "DebitorCreditor_ID");
            AddForeignKey("dbo.Folders", "DebitorCreditor_ID", "dbo.DebitorCreditors", "ID");
        }

        public override void Up()
        {
            DropForeignKey("dbo.Folders", "DebitorCreditor_ID", "dbo.DebitorCreditors");
            DropIndex("dbo.Folders", new[] { "DebitorCreditor_ID" });
            AddColumn("dbo.DebitorCreditors", "Root_ID", c => c.Int());
            CreateIndex("dbo.DebitorCreditors", "Root_ID");
            AddForeignKey("dbo.DebitorCreditors", "Root_ID", "dbo.Folders", "ID");
            DropColumn("dbo.Folders", "DebitorCreditor_ID");
        }
    }
}