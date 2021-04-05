using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddDropscanMailingPage : DbMigration
    {
        public override void Down()
        {
            DropForeignKey("dbo.DropscanMailingPages", "MailingID", "dbo.DropscanMailings");
            DropIndex("dbo.DropscanMailingPages", "IX_MailingPageNumber");
            DropTable("dbo.DropscanMailingPages");
        }

        public override void Up()
        {
            CreateTable(
                    "dbo.DropscanMailingPages",
                    c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MailingID = c.Int(nullable: false),
                        PageContentType = c.String(),
                        PageData = c.Binary(),
                        PageNumber = c.Int(nullable: false)
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DropscanMailings", t => t.MailingID, cascadeDelete: true)
                .Index(t => new { t.MailingID, t.PageNumber }, unique: true, name: "IX_MailingPageNumber");
        }
    }
}