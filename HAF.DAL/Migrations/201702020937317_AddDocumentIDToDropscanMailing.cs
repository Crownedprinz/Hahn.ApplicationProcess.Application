using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddDocumentIDToDropscanMailing : DbMigration
    {
        public override void Down()
        {
            DropColumn("dbo.DropscanMailings", "DocumentID");
        }

        public override void Up()
        {
            AddColumn("dbo.DropscanMailings", "DocumentID", c => c.Int());
        }
    }
}