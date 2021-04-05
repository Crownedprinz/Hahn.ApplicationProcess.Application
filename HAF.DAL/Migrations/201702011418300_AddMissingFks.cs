using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddMissingFks : DbMigration
    {
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "DebitorCreditorID", "dbo.DebitorCreditors");
            DropIndex("dbo.Documents", new[] { "DebitorCreditorID" });
            DropColumn("dbo.Documents", "DebitorCreditorID");
        }

        public override void Up()
        {
            AddColumn("dbo.Documents", "DebitorCreditorID", c => c.Int());
            CreateIndex("dbo.Documents", "DebitorCreditorID");
            AddForeignKey("dbo.Documents", "DebitorCreditorID", "dbo.DebitorCreditors", "ID");
        }
    }
}