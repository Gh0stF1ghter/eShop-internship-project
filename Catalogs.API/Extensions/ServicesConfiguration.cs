using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.API.Extensions
{
    public static class ServicesConfiguration
    {
        public static void AddContext(this IServiceCollection services, IConfiguration configuration) =>
    services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("Identity.DataAccess")));

    }
}
