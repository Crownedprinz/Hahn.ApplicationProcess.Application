using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class AddFolders : DbMigration
    {
        public override void Down()
        {
            AddColumn("dbo.Documents", "SubFolder", c => c.String());
            DropForeignKey("dbo.DebitorCreditors", "Company_ID", "dbo.Companies");
            DropForeignKey("dbo.Folders", "DebitorCreditor_ID", "dbo.DebitorCreditors");
            DropForeignKey("dbo.Folders", "Folder_ID", "dbo.Folders");
            DropForeignKey("dbo.Documents", "SubFolder_ID", "dbo.Folders");
            DropIndex("dbo.Documents", new[] { "SubFolder_ID" });
            DropIndex("dbo.Folders", new[] { "DebitorCreditor_ID" });
            DropIndex("dbo.Folders", new[] { "Folder_ID" });
            DropIndex("dbo.DebitorCreditors", new[] { "Company_ID" });
            DropColumn("dbo.Documents", "SubFolder_ID");
            DropColumn("dbo.DebitorCreditors", "Company_ID");
            DropTable("dbo.Folders");
        }

        public override void Up()
        {
            CreateTable(
                    "dbo.Folders",
                    c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Folder_ID = c.Int(),
                        DebitorCreditor_ID = c.Int()
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Folders", t => t.Folder_ID)
                .ForeignKey("dbo.DebitorCreditors", t => t.DebitorCreditor_ID)
                .Index(t => t.Folder_ID)
                .Index(t => t.DebitorCreditor_ID);

            AddColumn("dbo.DebitorCreditors", "Company_ID", c => c.Int());
            AddColumn("dbo.Documents", "SubFolder_ID", c => c.Int());
            CreateIndex("dbo.DebitorCreditors", "Company_ID");
            CreateIndex("dbo.Documents", "SubFolder_ID");
            AddForeignKey("dbo.Documents", "SubFolder_ID", "dbo.Folders", "ID");
            AddForeignKey("dbo.DebitorCreditors", "Company_ID", "dbo.Companies", "ID");
            DropColumn("dbo.Documents", "SubFolder");
        }
    }
}