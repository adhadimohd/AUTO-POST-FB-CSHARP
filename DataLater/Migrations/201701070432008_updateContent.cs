namespace DataLater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "Description", c => c.String());
            AddColumn("dbo.Contents", "mediaurl01", c => c.String());
            AddColumn("dbo.Contents", "mediaurl02", c => c.String());
            AddColumn("dbo.Contents", "mediaurl03", c => c.String());
            AddColumn("dbo.Contents", "mediaurl04", c => c.String());
            AddColumn("dbo.Contents", "mediaurl05", c => c.String());
            AddColumn("dbo.Contents", "isactive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "isactive");
            DropColumn("dbo.Contents", "mediaurl05");
            DropColumn("dbo.Contents", "mediaurl04");
            DropColumn("dbo.Contents", "mediaurl03");
            DropColumn("dbo.Contents", "mediaurl02");
            DropColumn("dbo.Contents", "mediaurl01");
            DropColumn("dbo.Contents", "Description");
        }
    }
}
