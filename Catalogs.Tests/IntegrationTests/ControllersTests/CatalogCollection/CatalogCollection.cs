using Catalogs.Tests.IntegrationTests.ApiFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Tests.IntegrationTests.ControllersTests.CatalogCollection
{
    [CollectionDefinition(nameof(CatalogCollection))]
    public class CatalogCollection : ICollectionFixture<CatalogApiApplicationFactory>;
}
