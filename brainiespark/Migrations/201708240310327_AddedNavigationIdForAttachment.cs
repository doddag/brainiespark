namespace brainiespark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNavigationIdForAttachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "Notification_Id1", c => c.Int());
            AddColumn("dbo.Notifications", "Attachment_Id", c => c.Int());
            CreateIndex("dbo.Attachments", "Notification_Id1");
            CreateIndex("dbo.Notifications", "Attachment_Id");
            AddForeignKey("dbo.Notifications", "Attachment_Id", "dbo.Attachments", "Id");
            AddForeignKey("dbo.Attachments", "Notification_Id1", "dbo.Notifications", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "Notification_Id1", "dbo.Notifications");
            DropForeignKey("dbo.Notifications", "Attachment_Id", "dbo.Attachments");
            DropIndex("dbo.Notifications", new[] { "Attachment_Id" });
            DropIndex("dbo.Attachments", new[] { "Notification_Id1" });
            DropColumn("dbo.Notifications", "Attachment_Id");
            DropColumn("dbo.Attachments", "Notification_Id1");
        }
    }
}
