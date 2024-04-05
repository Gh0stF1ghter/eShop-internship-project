using Identity.DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace Identity.Tests.IntegrationTests.ApiFactory
{
    public class IdentityApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const int MsSqlPort = 1433;
        private const string SA_Password = "A&VeryComplex123Password";

        private readonly MsSqlContainer _container;

        public IdentityApiApplicationFactory()
        {
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(MsSqlPort, true)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("MSSQL_SA_PASSWORD", SA_Password)
                .WithEnvironment("SQLCMDPASSWORD", SA_Password)
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            var host = _container.Hostname;
            var port = _container.GetMappedPublicPort(MsSqlPort);

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<IdentityContext>));

                services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    $"Server={host},{port}; Database=Identity; User Id=SA; Password={SA_Password}; Trusted_Connection=False; MultipleActiveResultSets=true; TrustServerCertificate=true"));

                var provider = services.BuildServiceProvider();

                var context = provider.GetService<IdentityContext>();

                context?.Database.Migrate();
            });
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }
    }
}