using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AllowDocumentsWithoutAllData : DbMigration
    {
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "FolderID", "dbo.Folders");
            DropIndex("dbo.Documents", new[] { "FolderID" });
            AlterColumn("dbo.Documents", "FolderID", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "Direction", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "Date", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Documents", "FolderID");
            AddForeignKey("dbo.Documents", "FolderID", "dbo.Folders", "ID", cascadeDelete: true);
        }

        public override void Up()
        {
            DropForeignKey("dbo.Documents", "FolderID", "dbo.Folders");
            DropIndex("dbo.Documents", new[] { "FolderID" });
            AlterColumn("dbo.Documents", "Date", c => c.DateTime());
            AlterColumn("dbo.Documents", "Direction", c => c.Int());
            AlterColumn("dbo.Documents", "FolderID", c => c.Int());
            CreateIndex("dbo.Documents", "FolderID");
            AddForeignKey("dbo.Documents", "FolderID", "dbo.Folders", "ID");
        }
    }
}