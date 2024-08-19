namespace FitFeastExplore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class images : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "ImagePath");
        }
    }
}
