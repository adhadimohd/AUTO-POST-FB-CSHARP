
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBHelper.DataModel
{
    public class Campaign
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class CampaignGroup
    {
        public int Id { get; set; }
        public int campaignId { get; set; }
        public int groupId { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Campaign Campaign { get; set; }
        public int CampaignId { get; set; }
        public int Commentcount { get; set; }

    }

    public class Comment
    {
        public int Id { get; set; }
        public string fbID { get; set; }
        public Friend Friend { get; set; }
        public int FriendId { get; set; }
        public string commentText { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
    }

    public class Friend
    {
        public int Id { get; set; }
        public string fbId { get; set; }
        public string Name { get; set; }
        public int AddAsFriend { get; set; }
        public int MessageEnabled { get; set; }

    }

    public class Content
    {
        public int Id { get; set; }
        public string ContentText { get; set; }
        public Campaign Campaign { get; set; }
        public int CampaignId { get; set; }
        public string Description { get; set; }
        public string mediaurl01 { get; set; }
        public string mediaurl02 { get; set; }
        public string mediaurl03 { get; set; }
        public string mediaurl04 { get; set; }
        public string mediaurl05 { get; set; }
        public bool isactive { get; set; }

    }

    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string url { get; set; }
        public bool subscribed { get; set; }
    }

    public class GroupManager
    {
        public int Id { get; set; }
        public string ManagerName { get; set; }
        public string Description { get; set; }
    }

    public class GroupManagerSet
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int GroupManagerId { get; set; }
    }

    public class Account
    {
        public int Id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string accountname { get; set;}
        public string companyname { get; set;}
    }

    public class GroupRAW
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string fbid { get; set; }
        public int ban { get; set; }
        public int hope { get; set; }
   
    }

    public class GroupTemp { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string fbid { get; set; }
    }


    public class PostTask
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int groupid { get; set; }
        public string fbid { get; set; }
        public string groupname { get; set; }
        public int contentId { get; set; }
        //public DateTime dateupdated { get; set; 
    }

    public class PostTaskLog
    {
        public int Id { get; set; }
        public DateTime datecreated { get; set; }
        public int groupid { get; set; }
        public string fbid { get; set; }
        public string groupname { get; set; }
        public int contentId { get; set; }
        public string contenttext { get; set; }
        public int poststatus { get; set; }
        public string poststatusremark { get; set; }


    }
}
