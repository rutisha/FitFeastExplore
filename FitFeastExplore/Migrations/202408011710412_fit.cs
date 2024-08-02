namespace FitFeastExplore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exercises",
                c => new
                    {
                        ExerciseId = c.Int(nullable: false, identity: true),
                        ExerciseName = c.String(),
                        Reps = c.Int(nullable: false),
                        sets = c.Int(nullable: false),
                        BodyPart = c.String(),
                    })
                .PrimaryKey(t => t.ExerciseId);
            
            CreateTable(
                "dbo.WorkOutPlans",
                c => new
                    {
                        WorkOutPlanID = c.Int(nullable: false, identity: true),
                        ExerciseName = c.String(),
                        Reps = c.Int(nullable: false),
                        sets = c.Int(nullable: false),
                        BodyPart = c.String(),
                        Notes = c.String(),
                        ExerciseId = c.Int(nullable: false),
                        WorkOutId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkOutPlanID)
                .ForeignKey("dbo.Exercises", t => t.ExerciseId, cascadeDelete: true)
                .ForeignKey("dbo.WorkOuts", t => t.WorkOutId, cascadeDelete: true)
                .Index(t => t.ExerciseId)
                .Index(t => t.WorkOutId);
            
            CreateTable(
                "dbo.WorkOuts",
                c => new
                    {
                        WorkOutId = c.Int(nullable: false, identity: true),
                        WorkOutName = c.String(),
                    })
                .PrimaryKey(t => t.WorkOutId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOutPlans", "WorkOutId", "dbo.WorkOuts");
            DropForeignKey("dbo.WorkOutPlans", "ExerciseId", "dbo.Exercises");
            DropIndex("dbo.WorkOutPlans", new[] { "WorkOutId" });
            DropIndex("dbo.WorkOutPlans", new[] { "ExerciseId" });
            DropTable("dbo.WorkOuts");
            DropTable("dbo.WorkOutPlans");
            DropTable("dbo.Exercises");
        }
    }
}
