using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class PagesMapping : DbMigration
    {
        public override void Down()
        {
            RenameColumn("dbo.DropscanMailings", "FileData", "DocumentData");
            DropForeignKey("dbo.MappedDropscanMailingPages", "MailingID", "dbo.DropscanMailings");
            DropForeignKey("dbo.MappedDropscanMailingPages", "DocumentID", "dbo.Documents");
            DropForeignKey("dbo.DiscardedDropscanMailingPages", "MailingID", "dbo.DropscanMailings");
            DropIndex("dbo.MappedDropscanMailingPages", new[] { "MailingID" });
            DropIndex("dbo.MappedDropscanMailingPages", new[] { "DocumentID" });
            DropIndex("dbo.DiscardedDropscanMailingPages", new[] { "MailingID" });
            DropTable("dbo.MappedDropscanMailingPages");
            DropTable("dbo.DiscardedDropscanMailingPages");
        }

        public override void Up()
        {
            CreateTable(
                    "dbo.DiscardedDropscanMailingPages",
                    c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MailingID = c.Int(nullable: false),
                        PageNumber = c.Int(nullable: false)
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DropscanMailings", t => t.MailingID, cascadeDelete: true)
                .Index(t => t.MailingID);

            CreateTable(
                    "dbo.MappedDropscanMailingPages",
                    c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DocumentID = c.Int(nullable: false),
                        MailingID = c.Int(nullable: false),
                        PageNumber = c.Int(nullable: false)
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.DropscanMailings", t => t.MailingID, cascadeDelete: true)
                .Index(t => t.DocumentID)
                .Index(t => t.MailingID);

            RenameColumn("dbo.DropscanMailings", "DocumentData", "FileData");
        }
    }
}