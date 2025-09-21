using BloggerApp.Tests.Integration.Fixtures;
using Xunit;

namespace BloggerApp.Tests.Integration.Controllers;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<DataStoreFixture>, ICollectionFixture<CustomWebApplicationFactory>
{
}