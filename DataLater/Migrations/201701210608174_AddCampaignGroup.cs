namespace DataLater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        campaignId = c.Int(nullable: false),
                        groupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CampaignGroups");
        }
    }
}
