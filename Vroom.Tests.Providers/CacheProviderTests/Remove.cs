using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using Vroom.Providers;

namespace Vroom.Tests.Providers.CacheProviderTests
{
    [TestFixture]
    public class Get
    {
        private Mock<IMemoryCache> mockedMemoryCache;
        private string itemName;
        private CacheProvider cacheProvider;

        [SetUp]
        public void Setup()
        {
            this.mockedMemoryCache = new Mock<IMemoryCache>();

            this.itemName = "itemName";

            this.cacheProvider = new CacheProvider(this.mockedMemoryCache.Object);
        }

        [Test]
        public void ShouldCallRemoveMethodFromMemoryCache()
        {
            // Arrange

            // Act
            this.cacheProvider.Remove(this.itemName);

            // Assert
            this.mockedMemoryCache.Verify(x => x.Remove(this.itemName), Times.Once);
        }
    }
}
