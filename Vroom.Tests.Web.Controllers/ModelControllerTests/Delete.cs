using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Controllers;
using Vroom.Models;
using Vroom.Models.Factories.NewFolder;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;

namespace Vroom.Tests.Web.Controllers.ModelControllerTests
{
    [TestFixture]
    public class Delete
    {
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;
        private ModelController controller;
        private string encryptedId;
        private int id;

        [SetUp]
        public void Setup()
        {
            this.encryptedId = "encryptedId";
            this.id = 3;


            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.encryptedId)).Returns(this.id);
            this.mockModelService.Setup(x => x.DoesExist(this.id)).Returns(true);

            this.controller = new ModelController(this.mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }

        [Test]
        public void ShouldDecryptMethodFromEncryptionProvider()
        {
            // Arrange

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedId), Times.Once);
        }

        [Test]
        public void ShouldDoesExistMethodFromModelService()
        {
            // Arrange

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockModelService.Verify(x => x.DoesExist(this.id), Times.Once);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenThereIsNoSuchModelWithThatId()
        {
            // Arrange

            this.mockModelService.Setup(x => x.DoesExist(this.id)).Returns(false);

            // Act
            var result = this.controller.Edit(this.encryptedId) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void ShouldDeleteMethodFromModelService()
        {
            // Arrange

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockModelService.Verify(x => x.Delete(this.id), Times.Once);
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
        public void ShouldReturnRedirectToActionResultWithActionNameIndex()
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
