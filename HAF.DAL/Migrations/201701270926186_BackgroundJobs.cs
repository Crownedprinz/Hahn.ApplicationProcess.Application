using System.Data.Entity.Migrations;

namespace  HAF.DAL.Migrations
{
    public partial class BackgroundJobs : DbMigration
    {
        public override void Down()
        {
            DropTable("dbo.BackgroundJobs");
        }

        public override void Up()
        {
            CreateTable(
                    "dbo.BackgroundJobs",
                    c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobStatus = c.Int(nullable: false),
                        ResultJson = c.String(),
                        ResultTypeString = c.String()
                    })
                .PrimaryKey(t => t.ID);
        }
    }
}