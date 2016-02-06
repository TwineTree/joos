namespace Joos.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Joos_1_0_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbpVote",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        QuestionId = c.Int(),
                        Value = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Vote_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.CreatorUserId)
                .ForeignKey("dbo.AbpUsers", t => t.DeleterUserId)
                .ForeignKey("dbo.AbpUsers", t => t.LastModifierUserId)
                .ForeignKey("dbo.AbpQuestions", t => t.QuestionId)
                .Index(t => t.QuestionId)
                .Index(t => t.DeleterUserId)
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpVote", "QuestionId", "dbo.AbpQuestions");
            DropForeignKey("dbo.AbpVote", "LastModifierUserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpVote", "DeleterUserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpVote", "CreatorUserId", "dbo.AbpUsers");
            DropIndex("dbo.AbpVote", new[] { "CreatorUserId" });
            DropIndex("dbo.AbpVote", new[] { "LastModifierUserId" });
            DropIndex("dbo.AbpVote", new[] { "DeleterUserId" });
            DropIndex("dbo.AbpVote", new[] { "QuestionId" });
            DropTable("dbo.AbpVote",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Vote_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
