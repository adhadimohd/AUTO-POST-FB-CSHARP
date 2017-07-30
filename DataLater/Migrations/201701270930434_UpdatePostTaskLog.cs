namespace DataLater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePostTaskLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostTaskLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        datecreated = c.DateTime(nullable: false),
                        groupid = c.Int(nullable: false),
                        fbid = c.String(),
                        groupname = c.String(),
                        contentId = c.Int(nullable: false),
                        contenttext = c.String(),
                        poststatus = c.Int(nullable: false),
                        poststatusremark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PostTaskLogs");
        }
    }
}
