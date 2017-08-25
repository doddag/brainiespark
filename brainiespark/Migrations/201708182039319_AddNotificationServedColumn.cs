namespace brainiespark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationServedColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "NotificationServed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "NotificationServed");
        }
    }
}
