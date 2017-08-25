namespace brainiespark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotificationServedColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notifications", "NotificationServed");
            AddColumn("dbo.Notifications", "NotificationServed", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "NotificationServed");
            AddColumn("dbo.Notifications", "NotificationServed", c => c.Boolean(nullable: false));
        }
    }
}
