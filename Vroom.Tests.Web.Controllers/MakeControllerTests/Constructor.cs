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
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;

namespace Vroom.Tests.Web.Controllers.MakeControllerTests
{
    [TestFixture]
    public class Constructor
    {
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;

        [SetUp]
        public void Setup()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
        }


        [Test]
        public void ShouldThrowArgumentNullException_WhenNullMapperIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MakeController(null, this.mockMakeService.Object, this.mockEncryptionProvider.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullIMapperIsPassed()
        {
            // Arrange
            var expectedExMessage = "Mapper provider cannot be null";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new MakeController(null, this.mockMakeService.Object, this.mockEncryptionProvider.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }


        [Test]
        public void ShouldThrowArgumentNullException_WhenNullMakeServiceIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MakeController(this.mockMapper.Object, null, this.mockEncryptionProvider.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullMakeServiceIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MakeServiceNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new MakeController(this.mockMapper.Object, null, this.mockEncryptionProvider.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullEncryptionProviderIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MakeController(this.mockMapper.Object, mockMakeService.Object, null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullEncryptionProviderIsPassed()
        {
            // Arrange
            var expectedExMessage = "Parameter encryptionProvider cannot be null";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new MakeController(this.mockMapper.Object, mockMakeService.Object, null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }



        [Test]
        public void ShouldNotThrowException_WhenNotNullParamsIsPassed()
        {
            // Arrange

            // Act & Assert
            Assert.DoesNotThrow(() =>
                new MakeController(this.mockMapper.Object, mockMakeService.Object, this.mockEncryptionProvider.Object));


        }
    }
}
