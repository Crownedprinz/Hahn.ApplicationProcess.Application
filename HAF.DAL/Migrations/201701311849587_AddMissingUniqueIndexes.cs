using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddMissingUniqueIndexes : DbMigration
    {
        public override void Down()
        {
            DropIndex("dbo.MappedDropscanMailingPages", "IX_MappedMailingPageNumber");
            DropIndex("dbo.DiscardedDropscanMailingPages", "IX_DiscardedMailingPageNumber");
            CreateIndex("dbo.MappedDropscanMailingPages", "MailingID");
            CreateIndex("dbo.DiscardedDropscanMailingPages", "MailingID");
        }

        public override void Up()
        {
            DropIndex("dbo.DiscardedDropscanMailingPages", new[] { "MailingID" });
            DropIndex("dbo.MappedDropscanMailingPages", new[] { "MailingID" });
            CreateIndex(
                "dbo.DiscardedDropscanMailingPages",
                new[] { "MailingID", "PageNumber" },
                unique: true,
                name: "IX_DiscardedMailingPageNumber");
            CreateIndex(
                "dbo.MappedDropscanMailingPages",
                new[] { "MailingID", "PageNumber" },
                unique: true,
                name: "IX_MappedMailingPageNumber");
        }
    }
}