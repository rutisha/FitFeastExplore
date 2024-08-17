namespace FitFeastExplore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYouTubeUrlToExercise : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "YouTubeUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exercises", "YouTubeUrl");
        }
    }
}
