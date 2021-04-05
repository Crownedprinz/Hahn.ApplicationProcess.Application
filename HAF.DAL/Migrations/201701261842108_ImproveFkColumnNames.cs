using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class ImproveFkColumnNames : DbMigration
    {
        public override void Down()
        {
            DropForeignKey("dbo.DebitorCreditors", "RootFolderID", "dbo.Folders");
            DropForeignKey("dbo.DropscanRecipients", "CorrespondingCompanyID", "dbo.Companies");
            DropForeignKey("dbo.DropscanMailings", "RecipientID", "dbo.DropscanRecipients");
            DropForeignKey("dbo.DebitorCreditors", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.DocumentFlags", "CompanyID", "dbo.Companies");
            DropIndex("dbo.DropscanRecipients", new[] { "CorrespondingCompanyID" });
            DropIndex("dbo.DropscanMailings", new[] { "RecipientID" });
            DropIndex("dbo.Folders", new[] { "ParentFolderID" });
            DropIndex("dbo.DebitorCreditors", new[] { "RootFolderID" });
            DropIndex("dbo.DebitorCreditors", new[] { "CompanyID" });
            DropIndex("dbo.DocumentFlags", new[] { "CompanyID" });
            AlterColumn("dbo.DropscanRecipients", "CorrespondingCompanyID", c => c.Int());
            AlterColumn("dbo.DropscanMailings", "RecipientID", c => c.Int());
            AlterColumn("dbo.Folders", "ParentFolderID", c => c.Int());
            AlterColumn("dbo.DebitorCreditors", "CompanyID", c => c.Int());
            AlterColumn("dbo.DebitorCreditors", "RootFolderID", c => c.Int());
            AlterColumn("dbo.DocumentFlags", "CompanyID", c => c.Int());
            RenameColumn(table: "dbo.DebitorCreditors", name: "RootFolderID", newName: "Root_ID");
            RenameColumn(
                table: "dbo.DropscanRecipients",
                name: "CorrespondingCompanyID",
                newName: "CorrespondingCompany_ID");
            RenameColumn(table: "dbo.DropscanMailings", name: "RecipientID", newName: "Recipient_ID");
            RenameColumn(table: "dbo.Folders", name: "ParentFolderID", newName: "Folder_ID");
            RenameColumn(table: "dbo.DebitorCreditors", name: "CompanyID", newName: "Company_ID");
            RenameColumn(table: "dbo.DocumentFlags", name: "CompanyID", newName: "Company_ID");
            CreateIndex("dbo.DropscanRecipients", "CorrespondingCompany_ID");
            CreateIndex("dbo.DropscanMailings", "Recipient_ID");
            CreateIndex("dbo.Folders", "Folder_ID");
            CreateIndex("dbo.DebitorCreditors", "Company_ID");
            CreateIndex("dbo.DebitorCreditors", "Root_ID");
            CreateIndex("dbo.DocumentFlags", "Company_ID");
            AddForeignKey("dbo.DebitorCreditors", "Root_ID", "dbo.Folders", "ID");
            AddForeignKey("dbo.DropscanRecipients", "CorrespondingCompany_ID", "dbo.Companies", "ID");
            AddForeignKey("dbo.DropscanMailings", "Recipient_ID", "dbo.DropscanRecipients", "ID");
            AddForeignKey("dbo.DebitorCreditors", "Company_ID", "dbo.Companies", "ID");
            AddForeignKey("dbo.DocumentFlags", "Company_ID", "dbo.Companies", "ID");
        }

        public override void Up()
        {
            DropForeignKey("dbo.DocumentFlags", "Company_ID", "dbo.Companies");
            DropForeignKey("dbo.DebitorCreditors", "Company_ID", "dbo.Companies");
            DropForeignKey("dbo.DropscanMailings", "Recipient_ID", "dbo.DropscanRecipients");
            DropForeignKey("dbo.DropscanRecipients", "CorrespondingCompany_ID", "dbo.Companies");
            DropForeignKey("dbo.DebitorCreditors", "Root_ID", "dbo.Folders");
            DropIndex("dbo.DocumentFlags", new[] { "Company_ID" });
            DropIndex("dbo.DebitorCreditors", new[] { "Root_ID" });
            DropIndex("dbo.DebitorCreditors", new[] { "Company_ID" });
            DropIndex("dbo.Folders", new[] { "Folder_ID" });
            DropIndex("dbo.DropscanMailings", new[] { "Recipient_ID" });
            DropIndex("dbo.DropscanRecipients", new[] { "CorrespondingCompany_ID" });
            RenameColumn(table: "dbo.DocumentFlags", name: "Company_ID", newName: "CompanyID");
            RenameColumn(table: "dbo.DebitorCreditors", name: "Company_ID", newName: "CompanyID");
            RenameColumn(table: "dbo.Folders", name: "Folder_ID", newName: "ParentFolderID");
            RenameColumn(table: "dbo.DropscanMailings", name: "Recipient_ID", newName: "RecipientID");
            RenameColumn(
                table: "dbo.DropscanRecipients",
                name: "CorrespondingCompany_ID",
                newName: "CorrespondingCompanyID");
            RenameColumn(table: "dbo.DebitorCreditors", name: "Root_ID", newName: "RootFolderID");
            AlterColumn("dbo.DocumentFlags", "CompanyID", c => c.Int(nullable: false));
            AlterColumn("dbo.DebitorCreditors", "RootFolderID", c => c.Int(nullable: false));
            AlterColumn("dbo.DebitorCreditors", "CompanyID", c => c.Int(nullable: false));
            AlterColumn("dbo.Folders", "ParentFolderID", c => c.Int(nullable: false));
            AlterColumn("dbo.DropscanMailings", "RecipientID", c => c.Int(nullable: false));
            AlterColumn("dbo.DropscanRecipients", "CorrespondingCompanyID", c => c.Int(nullable: false));
            CreateIndex("dbo.DocumentFlags", "CompanyID");
            CreateIndex("dbo.DebitorCreditors", "CompanyID");
            CreateIndex("dbo.DebitorCreditors", "RootFolderID");
            CreateIndex("dbo.Folders", "ParentFolderID");
            CreateIndex("dbo.DropscanMailings", "RecipientID");
            CreateIndex("dbo.DropscanRecipients", "CorrespondingCompanyID");
            AddForeignKey("dbo.DocumentFlags", "CompanyID", "dbo.Companies", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DebitorCreditors", "CompanyID", "dbo.Companies", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DropscanMailings", "RecipientID", "dbo.DropscanRecipients", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DropscanRecipients", "CorrespondingCompanyID", "dbo.Companies", "ID", cascadeDelete: true);
            AddForeignKey("dbo.DebitorCreditors", "RootFolderID", "dbo.Folders", "ID", cascadeDelete: true);
        }
    }
}