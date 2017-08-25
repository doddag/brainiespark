namespace brainiespark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotificationServedColumnasNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notifications", "NotificationServed", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "NotificationServed", c => c.DateTime(nullable: false));
        }
    }
}
