using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "DebitorCreditor_ID", "dbo.DebitorCreditors");
            DropForeignKey("dbo.Documents", "Company_ID", "dbo.Companies");
            DropForeignKey("dbo.DocumentFlags", "Company_ID", "dbo.Companies");
            DropIndex("dbo.Documents", new[] { "DebitorCreditor_ID" });
            DropIndex("dbo.Documents", new[] { "Company_ID" });
            DropIndex("dbo.DocumentFlags", new[] { "Company_ID" });
            DropTable("dbo.DropscanMailings");
            DropTable("dbo.Documents");
            DropTable("dbo.DebitorCreditors");
            DropTable("dbo.DocumentFlags");
            DropTable("dbo.Companies");
        }

        public override void Up()
        {
            CreateTable("dbo.Companies", c => new { ID = c.Int(nullable: false, identity: true), Name = c.String() })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.DocumentFlags",
                    c => new { ID = c.Int(nullable: false, identity: true), Name = c.String(), Company_ID = c.Int() })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.Company_ID)
                .Index(t => t.Company_ID);

            CreateTable("dbo.DebitorCreditors", c => new { ID = c.Int(nullable: false, identity: true), Name = c.String() })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Documents",
                    c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContextData = c.Binary(),
                        ContextDataContentType = c.String(),
                        Date = c.DateTime(nullable: false),
                        Direction = c.Int(nullable: false),
                        DocumentData = c.Binary(),
                        DocumentDataContentType = c.String(),
                        FileExtension = c.String(),
                        Name = c.String(),
                        SubFolder = c.String(),
                        Company_ID = c.Int(),
                        DebitorCreditor_ID = c.Int()
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.Company_ID)
                .ForeignKey("dbo.DebitorCreditors", t => t.DebitorCreditor_ID)
                .Index(t => t.Company_ID)
                .Index(t => t.DebitorCreditor_ID);

            CreateTable(
                    "dbo.DropscanMailings",
                    c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DocumentData = c.Binary(),
                        DownloadedAt = c.DateTime(),
                        EnvelopeData = c.Binary(),
                        ForwardedAt = c.DateTime(),
                        ForwardRequestedAt = c.DateTime(),
                        ReceivedAt = c.DateTime(nullable: false),
                        Recipient = c.String(),
                        ScanboxId = c.Int(nullable: false),
                        ScannedAt = c.DateTime(),
                        ScanRequestedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Uuid = c.String()
                    })
                .PrimaryKey(t => t.ID);
        }
    }
}