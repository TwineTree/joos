namespace Joos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Joos_1_0_2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AbpVote", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AbpVote", "UserId", c => c.Long(nullable: false));
        }
    }
}
