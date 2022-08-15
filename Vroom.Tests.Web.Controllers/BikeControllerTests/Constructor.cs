using AutoMapper;
using NUnit.Framework;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using Vroom.Controllers;
using Vroom.Common;
using Vroom.Models.Factories.NewFolder;

namespace Vroom.Tests.Web.Controllers.BikeControllerTests
{
    [TestFixture]
    public class Constructor
    {
        private Mock<IWebHostEnvironment> mockHostingEnvironment;
        private Mock<IMapper> mockMapper;
        private Mock<IBikeService> mockBikeService;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IHttpContextProvider> mockHttpContextProvider;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IIOProvider> mockIOProvider;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;

        [SetUp]
        public void Setup()
        {
            this.mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            this.mockMapper = new Mock<IMapper>();
            this.mockBikeService = new Mock<IBikeService>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockHttpContextProvider = new Mock<IHttpContextProvider>();
            this.mockCacheProvider = new Mock<ICacheProvider>();
            this.mockIOProvider = new Mock<IIOProvider>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();
        }


        [Test]
        public void ShouldThrowArgumentNullException_WhenNullIWebHostEnvironmentIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(null, this.mockMapper.Object, 
                this.mockBikeService.Object, this.mockMakeService.Object, 
                this.mockModelService.Object, this.mockHttpContextProvider.Object, 
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullIWebHostEnvironmentIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter hostingEnvironment cannot be null or empty";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(null, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullMapperIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, null,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullMapperIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MapperProviderNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, null,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullBikeServiceIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                null, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullBikeServiceIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.BikeServiceNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                null, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullMakeServiceIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object,null,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullMakeServiceIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MakeServiceNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, null,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullModelServiceIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                null, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullModelServiceIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.ModelServiceNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                null, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullHttpContextProviderIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, null,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullHttpContextProviderIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter httpContextProvider cannot be null or empty";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, null,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullCacheProviderIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                null, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullCacheProviderIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter cacheProvider cannot be null or empty";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                null, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNulliOProviderIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object,null,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNulliOProviderIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter iOProvider cannot be null or empty";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, null,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullEncryptionProviderIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                null, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullEncryptionProviderIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter encryptionProvider cannot be null or empty";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                null, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullSelectItemFactoryIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullSelectItemFactoryIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter selectedItemFactory cannot be null or empty";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldNotThrowException_WhenNotNullParamsIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.DoesNotThrow(() =>
                new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOProvider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }
    }
}
