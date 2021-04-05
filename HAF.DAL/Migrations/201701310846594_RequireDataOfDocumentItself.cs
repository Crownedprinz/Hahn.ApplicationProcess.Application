using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class RequireDataOfDocumentItself : DbMigration
    {
        public override void Down()
        {
            AlterColumn("dbo.Documents", "DocumentFileExtension", c => c.String());
            AlterColumn("dbo.Documents", "DocumentDataContentType", c => c.String());
            AlterColumn("dbo.Documents", "DocumentData", c => c.Binary());
        }

        public override void Up()
        {
            AlterColumn("dbo.Documents", "DocumentData", c => c.Binary(nullable: false));
            AlterColumn("dbo.Documents", "DocumentDataContentType", c => c.String(nullable: false));
            AlterColumn("dbo.Documents", "DocumentFileExtension", c => c.String(nullable: false));
        }
    }
}