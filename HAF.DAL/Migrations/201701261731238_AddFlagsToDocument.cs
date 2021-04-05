using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddFlagsToDocument : DbMigration
    {
        public override void Down()
        {
            DropForeignKey("dbo.Documents2Flags", "DocumentFlagID", "dbo.DocumentFlags");
            DropForeignKey("dbo.Documents2Flags", "DocumentID", "dbo.Documents");
            DropIndex("dbo.Documents2Flags", new[] { "DocumentFlagID" });
            DropIndex("dbo.Documents2Flags", new[] { "DocumentID" });
            DropTable("dbo.Documents2Flags");
        }

        public override void Up()
        {
            CreateTable(
                    "dbo.Documents2Flags",
                    c => new { DocumentID = c.Int(nullable: false), DocumentFlagID = c.Int(nullable: false) })
                .PrimaryKey(t => new { t.DocumentID, t.DocumentFlagID })
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.DocumentFlags", t => t.DocumentFlagID, cascadeDelete: true)
                .Index(t => t.DocumentID)
                .Index(t => t.DocumentFlagID);
        }
    }
}