namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AmendSpellingOnField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrackedTimes", "ScreenTimeGoal", c => c.Double(nullable: false));
            DropColumn("dbo.TrackedTimes", "ScreenTimeGoal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TrackedTimes", "ScreenTimeGoal", c => c.Double(nullable: false));
            DropColumn("dbo.TrackedTimes", "ScreenTimeGoal");
        }
    }
}
