using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Vroom.Controllers;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;

namespace Vroom.Tests.Web.Controllers.MakeControllerTests
{
    [TestFixture]
    public class Delete
    {
        private string encryptedId;
        private int id;

        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private MakeController controller;

        [SetUp]
        public void Setup()
        {
            this.encryptedId = "encryptedId";
            this.id = 3;



            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();

            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.encryptedId)).Returns(this.id);
            this.mockMakeService.Setup(x => x.DoesExist(this.id)).Returns(true);
            
            this.controller = new MakeController(this.mockMapper.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object);
        }

        [Test]
        public void ShouldCallDecryptIdMethodEncryptionProvider()
        {
            // Arrang 

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedId), Times.Once);
        }

        [Test]
        public void ShouldCallDoesExistMethodMakeService()
        {
            // Arrang 

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockMakeService.Verify(x => x.DoesExist(this.id), Times.Once);
        }

        [Test]
        public void ShouldCallDeleteMethodMakeService()
        {
            // Arrang 

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockMakeService.Verify(x => x.Delete(this.id), Times.Once);
        }

        [Test]
        public void ShouldNotCallDeleteMethodFromMakeService_WhenThereIsNotMakeWithThatId()
        {

            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(this.id)).Returns(false);

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockMakeService.Verify(x => x.Delete(this.id), Times.Never);
        }

        [Test]
        public void ShouldReturnRedirectToActionResult()
        {
            // Arrange


            // Act
            var result = this.controller.Delete(this.encryptedId) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Test]
        public void ShouldReturnRedirectToActionResultWithCorrectModel()
        {
            // Arrange

            // Act
            var result = this.controller.Delete(this.encryptedId) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
