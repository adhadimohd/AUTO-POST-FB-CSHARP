namespace DataLater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostTask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        groupid = c.Int(nullable: false),
                        fbid = c.String(),
                        groupname = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PostTasks");
        }
    }
}
