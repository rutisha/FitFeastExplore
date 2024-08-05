namespace FitFeastExplore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientId = c.Int(nullable: false, identity: true),
                        IngredientName = c.String(),
                        Quantity = c.String(),
                    })
                .PrimaryKey(t => t.IngredientId);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeId = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        RecipeName = c.String(),
                        Instructions = c.String(),
                        Protein = c.String(),
                        Calories = c.String(),
                        CookingTime = c.String(),
                        IngredientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RecipeId)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .Index(t => t.IngredientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "IngredientId", "dbo.Ingredients");
            DropIndex("dbo.Recipes", new[] { "IngredientId" });
            DropTable("dbo.Recipes");
            DropTable("dbo.Ingredients");
        }
    }
}
