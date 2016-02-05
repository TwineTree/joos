namespace Joos.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Joos_1_0_0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbpQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        PositiveValue = c.String(),
                        NegativeValue = c.String(),
                        ImageUrl = c.String(),
                        VideoUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Question_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpQuestions", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpQuestions", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpQuestions", "CreatorUserId", "dbo.AbpUsers");
            DropIndex("dbo.AbpQuestions", new[] { "CreatorUserId" });
            DropIndex("dbo.AbpQuestions", new[] { "LastModifierUserId" });
            DropIndex("dbo.AbpQuestions", new[] { "DeleterUserId" });
            DropTable("dbo.AbpQuestions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Question_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
