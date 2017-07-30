namespace DataLater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateboolcolum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Friends", "AddAsFriend", c => c.Int(nullable: false));
            AlterColumn("dbo.Friends", "MessageEnabled", c => c.Int(nullable: false));
            AlterColumn("dbo.GroupRAWs", "ban", c => c.Int(nullable: false));
            AlterColumn("dbo.GroupRAWs", "hope", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GroupRAWs", "hope", c => c.Boolean(nullable: false));
            AlterColumn("dbo.GroupRAWs", "ban", c => c.String());
            AlterColumn("dbo.Friends", "MessageEnabled", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Friends", "AddAsFriend", c => c.Boolean(nullable: false));
        }
    }
}
