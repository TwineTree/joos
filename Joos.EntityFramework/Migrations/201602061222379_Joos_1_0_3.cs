namespace Joos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Joos_1_0_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpQuestions", "PositiveVotes", c => c.Int(nullable: false));
            AddColumn("dbo.AbpQuestions", "NegativeVotes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpQuestions", "NegativeVotes");
            DropColumn("dbo.AbpQuestions", "PositiveVotes");
        }
    }
}
