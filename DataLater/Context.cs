using System.Data.Entity;
using FBHelper.DataModel;


namespace FBHelper.Data
{
    public class FB:DbContext
    {
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignGroup> CampaignGroups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTask> PostTasks { get; set; }
        public DbSet<PostTaskLog> PostTaskLogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupManager> GroupManagers { get; set; }
        public DbSet<GroupManagerSet> GroupManagerSets { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<GroupRAW> GroupRAWS { get; set; }
        public DbSet<GroupTemp> GroupTemps { get; set; }
    }

}
