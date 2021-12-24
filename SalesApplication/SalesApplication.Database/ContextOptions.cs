using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace SalesApplication.Database
{
    public class ContextOptions
    {
        public static DbContextOptions<GeneralContext> Postgres()
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = "localhost",
                Port = 5432,
                Username = "postgres",
                Password = "123",
                Database = "salesapp"
            };

            DbContextOptionsBuilder<GeneralContext> optionsBuilder = new();
            optionsBuilder.UseNpgsql(connectionStringBuilder.ConnectionString);
            return optionsBuilder.Options;
        }

        public static DbContextOptions<GeneralContext> InMemory()
        {
            DbContextOptionsBuilder<GeneralContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            return optionsBuilder.Options;
        }
    }
}
