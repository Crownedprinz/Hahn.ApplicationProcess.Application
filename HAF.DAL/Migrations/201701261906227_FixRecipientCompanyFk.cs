using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class FixRecipientCompanyFk : DbMigration
    {
        public override void Down()
        {
            DropForeignKey("dbo.DropscanRecipients", "CorrespondingCompanyID", "dbo.Companies");
            DropIndex("dbo.DropscanRecipients", new[] { "CorrespondingCompanyID" });
            AlterColumn("dbo.DropscanRecipients", "CorrespondingCompanyID", c => c.Int(nullable: false));
            CreateIndex("dbo.DropscanRecipients", "CorrespondingCompanyID");
            AddForeignKey("dbo.DropscanRecipients", "CorrespondingCompanyID", "dbo.Companies", "ID", cascadeDelete: true);
        }

        public override void Up()
        {
            DropForeignKey("dbo.DropscanRecipients", "CorrespondingCompanyID", "dbo.Companies");
            DropIndex("dbo.DropscanRecipients", new[] { "CorrespondingCompanyID" });
            AlterColumn("dbo.DropscanRecipients", "CorrespondingCompanyID", c => c.Int());
            CreateIndex("dbo.DropscanRecipients", "CorrespondingCompanyID");
            AddForeignKey("dbo.DropscanRecipients", "CorrespondingCompanyID", "dbo.Companies", "ID");
        }
    }
}