using Catalogs.Tests.IntegrationTests.ApiFactory;

namespace Catalogs.Tests.IntegrationTests.ControllersTests.CatalogCollection
{
    [CollectionDefinition(nameof(CatalogCollection))]
    public class CatalogCollection : ICollectionFixture<CatalogApiApplicationFactory>;
}
