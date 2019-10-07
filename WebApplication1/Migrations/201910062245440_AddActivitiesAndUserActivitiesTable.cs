namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivitiesAndUserActivitiesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Description = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(),
                        ThemeColor = c.String(),
                        IsFullDay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ActivityId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Users_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Users_Id)
                .Index(t => t.ActivityId)
                .Index(t => t.Users_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserActivities", "Users_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserActivities", "ActivityId", "dbo.Activities");
            DropIndex("dbo.UserActivities", new[] { "Users_Id" });
            DropIndex("dbo.UserActivities", new[] { "ActivityId" });
            DropTable("dbo.UserActivities");
            DropTable("dbo.Activities");
        }
    }
}
