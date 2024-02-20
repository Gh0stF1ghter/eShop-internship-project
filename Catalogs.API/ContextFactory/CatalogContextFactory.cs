using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalogs.API.ContextFactory
{
    public class CatalogContextFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b=> b.MigrationsAssembly("Catalogs.API"));

            return new CatalogContext(builder.Options);
        }
    }
}
