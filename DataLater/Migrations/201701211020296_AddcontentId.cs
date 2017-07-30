namespace DataLater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddcontentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostTasks", "contentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostTasks", "contentId");
        }
    }
}
