using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace Baskets.Tests.IntegrationTests.ApiFactory
{
    public class BasketApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MongoDbContainer _mongodbContainer;

        public BasketApiApplicationFactory()
        {
            _mongodbContainer = new MongoDbBuilder()
                .WithImage("mongo:latest")
                .WithUsername("admin")
                .WithPassword("admin")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var hostname = _mongodbContainer.Hostname;
            var port = _mongodbContainer.GetMappedPublicPort(27017);

            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IMongoClient>();

                services.TryAddSingleton<IMongoClient>(_ =>
                    new MongoClient($"mongodb://admin:admin@{hostname}:{port}"));
            });
        }

        public async Task InitializeAsync()
        {
            await _mongodbContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await _mongodbContainer.DisposeAsync();
        }
    }
}