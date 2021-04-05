using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddDropscanRecipient : DbMigration
    {
        public override void Down()
        {
            AddColumn("dbo.DropscanMailings", "Recipient", c => c.String());
            DropForeignKey("dbo.DropscanMailings", "Recipient_ID", "dbo.DropscanRecipients");
            DropForeignKey("dbo.DropscanRecipients", "CorrespondingCompany_ID", "dbo.Companies");
            DropIndex("dbo.DropscanRecipients", new[] { "CorrespondingCompany_ID" });
            DropIndex("dbo.DropscanMailings", new[] { "Recipient_ID" });
            DropColumn("dbo.DropscanMailings", "Recipient_ID");
            DropTable("dbo.DropscanRecipients");
        }

        public override void Up()
        {
            CreateTable(
                    "dbo.DropscanRecipients",
                    c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExternalID = c.Int(nullable: false),
                        Name = c.String(),
                        CorrespondingCompany_ID = c.Int()
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.CorrespondingCompany_ID)
                .Index(t => t.CorrespondingCompany_ID);

            AddColumn("dbo.DropscanMailings", "Recipient_ID", c => c.Int());
            CreateIndex("dbo.DropscanMailings", "Recipient_ID");
            AddForeignKey("dbo.DropscanMailings", "Recipient_ID", "dbo.DropscanRecipients", "ID");
            DropColumn("dbo.DropscanMailings", "Recipient");
        }
    }
}