using Baskets.Tests.IntegrationTests.ApiFactory;

namespace Baskets.Tests.IntegrationTests.ControllersTests
{
    [CollectionDefinition(nameof(TestsCollection))]
    public class TestsCollection : ICollectionFixture<BasketApiApplicationFactory>;
}
