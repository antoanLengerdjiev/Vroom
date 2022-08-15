using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using Vroom.Providers;

namespace Vroom.Tests.Providers.CacheProviderTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void ShouldThrowArgumentNullException_WhenIMemoryCacheIsNull()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new CacheProvider(null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenMemoryCacheIsNull()
        {
            // Arrange
            var expectedExMessage = "Parameter memoryCache cannot be null or empty";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CacheProvider(null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldNotThrowException_WhenMemoryCacheIsNotNull()
        {
            // Arrange
            var mockedMemoryCache = new Mock<IMemoryCache>();
            // Act

            // Assert
            Assert.DoesNotThrow(() => new CacheProvider(mockedMemoryCache.Object));
        }

    }
}
