using Microsoft.EntityFrameworkCore.Design;

namespace SalesApplication.Database
{
    public class DesignContextFactory : IDesignTimeDbContextFactory<GeneralContext>
    {
        public GeneralContext CreateDbContext(string[] args)
        {
            return new GeneralContext(ContextOptions.Postgres());
        }
    }
}