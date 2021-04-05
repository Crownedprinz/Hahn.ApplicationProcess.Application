using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class FixParentFolderFk : DbMigration
    {
        public override void Down()
        {
            DropIndex("dbo.Folders", new[] { "ParentFolderID" });
            AlterColumn("dbo.Folders", "ParentFolderID", c => c.Int(nullable: false));
            CreateIndex("dbo.Folders", "ParentFolderID");
        }

        public override void Up()
        {
            DropIndex("dbo.Folders", new[] { "ParentFolderID" });
            AlterColumn("dbo.Folders", "ParentFolderID", c => c.Int());
            CreateIndex("dbo.Folders", "ParentFolderID");
        }
    }
}