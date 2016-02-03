using Joos.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Joos.Migrations.SeedData
{
    public class InitialDataBuilder
    {
        private readonly JoosDbContext _context;

        public InitialDataBuilder(JoosDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            _context.DisableAllFilters();

            new DefaultEditionsBuilder(_context).Build();
            new DefaultTenantRoleAndUserBuilder(_context).Build();
            new DefaultLanguagesBuilder(_context).Build();
        }
    }
}
