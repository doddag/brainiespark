namespace brainiespark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDisplayAttributeToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Discriminator");
        }
    }
}
