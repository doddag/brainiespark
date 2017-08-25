namespace brainiespark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPublicAndAllowHtmLonMssage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "IsPublic");
        }
    }
}
