using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Controllers;
using Vroom.Models.Factories.NewFolder;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;

namespace Vroom.Tests.Web.Controllers.ModelControllerTests
{
    public class Constructor
    {
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;

        [SetUp]
        public void Setup()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();
        }


        [Test]
        public void ShouldThrowArgumentNullException_WhenNullMapperIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ModelController(null, this.mockModelService.Object, mockMakeService.Object, mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullIMapperIsPassed()
        {
            // Arrange
            var expectedExMessage = "Mapper provider cannot be null";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ModelController(null, 
                this.mockModelService.Object, mockMakeService.Object, 
                mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullModelServiceIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ModelController(mockMapper.Object,
                null, mockMakeService.Object,
                mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullModelServiceIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.ModelServiceNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ModelController(mockMapper.Object,null, mockMakeService.Object,
                mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullMakeServiceIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                 new ModelController(mockMapper.Object, this.mockModelService.Object, null,
                mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullMakeServiceIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MakeServiceNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ModelController(mockMapper.Object, this.mockModelService.Object, null,
                mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullEncryptionProviderIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ModelController(mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object,
                null, this.mockSelectItemFactory.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullEncryptionProviderIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter encryptionProvider cannot be null";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
            new ModelController(mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object,
                null, this.mockSelectItemFactory.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullSelectItemFactoryIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ModelController(mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object,
                this.mockEncryptionProvider.Object, null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullSelectItemFactoryIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter selectedItemFactory cannot be null";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ModelController(mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object,
                this.mockEncryptionProvider.Object, null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldNotThrowException_WhenNotNullParamsIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.DoesNotThrow(() =>
                new ModelController(mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object));
        }
    }
}
