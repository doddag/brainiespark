namespace brainiespark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AttachmentContent = c.Binary(),
                        EnteredBy = c.String(),
                        DateTimeEntered = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        DateTimeModified = c.DateTime(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Notification_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notifications", t => t.Notification_Id)
                .Index(t => t.Notification_Id);
            
            CreateTable(
                "dbo.AuditDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColumnName = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Audit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audits", t => t.Audit_Id)
                .Index(t => t.Audit_Id);
            
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuditDateTime = c.DateTime(nullable: false),
                        EnteredByUserId = c.String(),
                        TableName = c.String(),
                        Activity = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ByUserId = c.String(),
                        NotificationDate = c.DateTime(nullable: false),
                        ToUserId = c.String(),
                        InDays = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Message = c.String(),
                        EnteredBy = c.String(),
                        DateTimeEntered = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        DateTimeModified = c.DateTime(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
         }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachments", "Notification_Id", "dbo.Notifications");
            DropForeignKey("dbo.AuditDatas", "Audit_Id", "dbo.Audits");
            DropIndex("dbo.AuditDatas", new[] { "Audit_Id" });
            DropIndex("dbo.Attachments", new[] {"Notification_Id"});
            DropTable("dbo.Notifications");
            DropTable("dbo.Audits");
            DropTable("dbo.AuditDatas");
            DropTable("dbo.Attachments");
        }
    }
}
