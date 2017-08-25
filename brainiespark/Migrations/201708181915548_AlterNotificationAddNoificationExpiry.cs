namespace brainiespark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotificationAddNoificationExpiry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "NotificationExpiry", c => c.DateTime(nullable: false));
            DropColumn("dbo.Notifications", "InDays");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "InDays", c => c.Int(nullable: false));
            DropColumn("dbo.Notifications", "NotificationExpiry");
        }
    }
}
