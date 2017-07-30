namespace DataLater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGroupRAW : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupRAWs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        fbid = c.String(),
                        ban = c.String(),
                        hope = c.Boolean(nullable: false),
                        banDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GroupRAWs");
        }
    }
}
