using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using Vroom.Providers;

namespace Vroom.Tests.Providers.HttpContextProviderTests
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void ShouldThrowArgumentNullException_WhenIHttpContextAccessorIsNull()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new HttpContextProvider(null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenIHttpContextAccessorIsNull()
        {
            // Arrange
            var expectedExMessage = "Parameter _httpContextAccessor cannot be null or empty";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new HttpContextProvider(null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldNotThrowException_WhenIHttpContextAccessorNotNull()
        {
            // Arrange
            var mockedHttpContextAccessor = new Mock<IHttpContextAccessor>();
            // Act

            // Assert
            Assert.DoesNotThrow(() => new HttpContextProvider(mockedHttpContextAccessor.Object));
        }
    }
}
