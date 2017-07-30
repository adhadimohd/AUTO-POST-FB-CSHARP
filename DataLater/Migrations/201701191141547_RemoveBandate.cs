namespace DataLater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBandate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GroupRAWs", "banDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GroupRAWs", "banDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
