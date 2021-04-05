using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class RenameProperty : DbMigration
    {
        public override void Down()
        {
            RenameIndex(table: "dbo.Documents", name: "IX_FolderID", newName: "IX_SubFolderID");
            RenameColumn(table: "dbo.Documents", name: "FolderID", newName: "SubFolderID");
        }

        public override void Up()
        {
            RenameColumn(table: "dbo.Documents", name: "SubFolderID", newName: "FolderID");
            RenameIndex(table: "dbo.Documents", name: "IX_SubFolderID", newName: "IX_FolderID");
        }
    }
}