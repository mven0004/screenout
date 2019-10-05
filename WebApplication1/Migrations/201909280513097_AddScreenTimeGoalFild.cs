namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScreenTimeGoalFild : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Children", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Children", new[] { "UserId" });
            AddColumn("dbo.Children", "ScreenTimeGoal", c => c.Double(nullable: false));
            AlterColumn("dbo.Children", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Children", "UserId");
            AddForeignKey("dbo.Children", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Children", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Children", new[] { "UserId" });
            AlterColumn("dbo.Children", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Children", "ScreenTimeGoal");
            CreateIndex("dbo.Children", "UserId");
            AddForeignKey("dbo.Children", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
