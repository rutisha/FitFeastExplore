namespace FitFeastExplore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYouTubeUrlToWorkOutPlan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOutPlans", "YouTubeUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOutPlans", "YouTubeUrl");
        }
    }
}
