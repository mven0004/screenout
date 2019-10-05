namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrackedTimeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrackedTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChildId = c.Int(nullable: false),
                        TrackedDate = c.DateTime(nullable: false),
                        FamilyTime = c.Double(nullable: false),
                        ScreenTime = c.Double(nullable: false),
                        ScreenTimeGoal = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ChildId, cascadeDelete: true)
                .Index(t => t.ChildId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrackedTimes", "ChildId", "dbo.Children");
            DropIndex("dbo.TrackedTimes", new[] { "ChildId" });
            DropTable("dbo.TrackedTimes");
        }
    }
}
