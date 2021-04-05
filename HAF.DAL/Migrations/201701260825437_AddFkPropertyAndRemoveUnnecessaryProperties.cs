using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddFkPropertyAndRemoveUnnecessaryProperties : DbMigration
    {
        public override void Down()
        {
            AddColumn("dbo.Documents", "DebitorCreditor_ID", c => c.Int());
            AddColumn("dbo.Documents", "Company_ID", c => c.Int());
            DropForeignKey("dbo.Documents", "SubFolderID", "dbo.Folders");
            DropIndex("dbo.Documents", new[] { "SubFolderID" });
            AlterColumn("dbo.Documents", "SubFolderID", c => c.Int());
            RenameColumn(table: "dbo.Documents", name: "SubFolderID", newName: "SubFolder_ID");
            CreateIndex("dbo.Documents", "SubFolder_ID");
            CreateIndex("dbo.Documents", "DebitorCreditor_ID");
            CreateIndex("dbo.Documents", "Company_ID");
            AddForeignKey("dbo.Documents", "SubFolder_ID", "dbo.Folders", "ID");
            AddForeignKey("dbo.Documents", "DebitorCreditor_ID", "dbo.DebitorCreditors", "ID");
            AddForeignKey("dbo.Documents", "Company_ID", "dbo.Companies", "ID");
        }

        public override void Up()
        {
            DropForeignKey("dbo.Documents", "Company_ID", "dbo.Companies");
            DropForeignKey("dbo.Documents", "DebitorCreditor_ID", "dbo.DebitorCreditors");
            DropForeignKey("dbo.Documents", "SubFolder_ID", "dbo.Folders");
            DropIndex("dbo.Documents", new[] { "Company_ID" });
            DropIndex("dbo.Documents", new[] { "DebitorCreditor_ID" });
            DropIndex("dbo.Documents", new[] { "SubFolder_ID" });
            RenameColumn(table: "dbo.Documents", name: "SubFolder_ID", newName: "SubFolderID");
            AlterColumn("dbo.Documents", "SubFolderID", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "SubFolderID");
            AddForeignKey("dbo.Documents", "SubFolderID", "dbo.Folders", "ID", cascadeDelete: true);
            DropColumn("dbo.Documents", "Company_ID");
            DropColumn("dbo.Documents", "DebitorCreditor_ID");
        }
    }
}