using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddFileExtensionForContext : DbMigration
    {
        public override void Down()
        {
            RenameColumn("dbo.Documents", "DocumentFileExtension", "FileExtension");
            DropColumn("dbo.Documents", "ContextFileExtension");
        }

        public override void Up()
        {
            AddColumn("dbo.Documents", "ContextFileExtension", c => c.String());
            RenameColumn("dbo.Documents", "FileExtension", "DocumentFileExtension");
        }
    }
}