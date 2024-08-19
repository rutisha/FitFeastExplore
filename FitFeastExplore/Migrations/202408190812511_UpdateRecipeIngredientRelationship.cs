namespace FitFeastExplore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRecipeIngredientRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recipes", "IngredientId", "dbo.Ingredients");
            DropIndex("dbo.Recipes", new[] { "IngredientId" });
            CreateTable(
                "dbo.RecipeIngredients",
                c => new
                    {
                        RecipeId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RecipeId, t.IngredientId })
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .Index(t => t.RecipeId)
                .Index(t => t.IngredientId);
            
            DropColumn("dbo.Recipes", "IngredientId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipes", "IngredientId", c => c.Int(nullable: false));
            DropForeignKey("dbo.RecipeIngredients", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.RecipeIngredients", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.RecipeIngredients", new[] { "IngredientId" });
            DropIndex("dbo.RecipeIngredients", new[] { "RecipeId" });
            DropTable("dbo.RecipeIngredients");
            CreateIndex("dbo.Recipes", "IngredientId");
            AddForeignKey("dbo.Recipes", "IngredientId", "dbo.Ingredients", "IngredientId", cascadeDelete: true);
        }
    }
}
