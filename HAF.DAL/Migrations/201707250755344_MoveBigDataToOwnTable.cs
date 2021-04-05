using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class MoveBigDataToOwnTable : DbMigration
    {
        public override void Down()
        {
            AddColumn("dbo.DropscanMailingPages", "PageData", c => c.Binary());
            AddColumn("dbo.DropscanMailings", "FileData", c => c.Binary());
            AddColumn("dbo.DropscanMailings", "EnvelopeData", c => c.Binary());
            AddColumn("dbo.Documents", "DocumentData", c => c.Binary(nullable: false));
            AddColumn("dbo.Documents", "ContextData", c => c.Binary());
            DropForeignKey("dbo.DropscanMailingPages", "PageData_ID", "dbo.DocumentData");
            DropForeignKey("dbo.DropscanMailings", "FileData_ID", "dbo.DocumentData");
            DropForeignKey("dbo.DropscanMailings", "EnvelopeData_ID", "dbo.DocumentData");
            DropForeignKey("dbo.Documents", "DocumentData_ID", "dbo.DocumentData");
            DropForeignKey("dbo.Documents", "ContextData_ID", "dbo.DocumentData");
            DropIndex("dbo.DropscanMailingPages", new[] { "PageData_ID" });
            DropIndex("dbo.DropscanMailings", new[] { "FileData_ID" });
            DropIndex("dbo.DropscanMailings", new[] { "EnvelopeData_ID" });
            DropIndex("dbo.Documents", new[] { "DocumentData_ID" });
            DropIndex("dbo.Documents", new[] { "ContextData_ID" });
            DropColumn("dbo.DropscanMailingPages", "PageData_ID");
            DropColumn("dbo.DropscanMailings", "FileData_ID");
            DropColumn("dbo.DropscanMailings", "EnvelopeData_ID");
            DropColumn("dbo.DropscanMailings", "PageCount");
            DropColumn("dbo.Documents", "DocumentData_ID");
            DropColumn("dbo.Documents", "ContextData_ID");
            DropTable("dbo.DocumentData");
        }

        public override void Up()
        {
            CreateTable("dbo.DocumentData", c => new { ID = c.Int(nullable: false, identity: true), Data = c.Binary() })
                .PrimaryKey(t => t.ID);

            AddColumn("dbo.Documents", "ContextData_ID", c => c.Int());
            AddColumn("dbo.Documents", "DocumentData_ID", c => c.Int());
            AddColumn("dbo.DropscanMailings", "PageCount", c => c.Int());
            AddColumn("dbo.DropscanMailings", "EnvelopeData_ID", c => c.Int(nullable: false));
            AddColumn("dbo.DropscanMailings", "FileData_ID", c => c.Int(nullable: false));
            AddColumn("dbo.DropscanMailingPages", "PageData_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "ContextData_ID");
            CreateIndex("dbo.Documents", "DocumentData_ID");
            CreateIndex("dbo.DropscanMailings", "EnvelopeData_ID");
            CreateIndex("dbo.DropscanMailings", "FileData_ID");
            CreateIndex("dbo.DropscanMailingPages", "PageData_ID");
            AddForeignKey("dbo.Documents", "ContextData_ID", "dbo.DocumentData", "ID");
            AddForeignKey("dbo.Documents", "DocumentData_ID", "dbo.DocumentData", "ID");
            AddForeignKey("dbo.DropscanMailings", "EnvelopeData_ID", "dbo.DocumentData", "ID");
            AddForeignKey("dbo.DropscanMailings", "FileData_ID", "dbo.DocumentData", "ID");
            AddForeignKey("dbo.DropscanMailingPages", "PageData_ID", "dbo.DocumentData", "ID");
            DropColumn("dbo.Documents", "ContextData");
            DropColumn("dbo.Documents", "DocumentData");
            DropColumn("dbo.DropscanMailings", "EnvelopeData");
            DropColumn("dbo.DropscanMailings", "FileData");
            DropColumn("dbo.DropscanMailingPages", "PageData");
        }
    }
}