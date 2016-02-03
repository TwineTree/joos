using System.Data.Entity.Migrations;
using Joos.Migrations.SeedData;

namespace Joos.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Joos.EntityFramework.JoosDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Joos";
        }

        protected override void Seed(Joos.EntityFramework.JoosDbContext context)
        {
            new InitialDataBuilder(context).Build();
        }
    }
}
