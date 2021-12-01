using System;
using Microsoft.EntityFrameworkCore;

namespace SalesApplication.Database
{
    public class ContextOptions
    {
        private const string CONNECTIONSTRING = @"Server=localhost;Port=5432;User Id=postgres;Password=123;";
        public static DbContextOptions<GeneralContext> Postgres()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeneralContext>();
            optionsBuilder.UseNpgsql(CONNECTIONSTRING);
            return optionsBuilder.Options;
        }

        public static DbContextOptions<GeneralContext> InMemory()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeneralContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            return optionsBuilder.Options;
        }
    }
}
