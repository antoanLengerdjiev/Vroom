using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Controllers;
using Vroom.Models.Factories.NewFolder;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;

namespace Vroom.Tests.Web.Controllers.BikeControllerTests
{
    [TestFixture]
    public class Delete
    {
        private int id;
        private string encryptedId;
        private Mock<IIOProvider> mockIOprovider;
        private Mock<IWebHostEnvironment> mockHostingEnvironment;
        private Mock<IMapper> mockMapper;
        private Mock<IBikeService> mockBikeService;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IHttpContextProvider> mockHttpContextProvider;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;
        private BikeController controller;

        [SetUp]
        public void Setup()
        {
            this.id = 3;
            this.encryptedId = "encryptedId";

            this.mockIOprovider = new Mock<IIOProvider>();
            this.mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            this.mockMapper = new Mock<IMapper>();
            this.mockBikeService = new Mock<IBikeService>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockHttpContextProvider = new Mock<IHttpContextProvider>();
            this.mockCacheProvider = new Mock<ICacheProvider>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockBikeService.Setup(x => x.DoesExist(this.id)).Returns(true);
            this.mockEncryptionProvider.Setup(x => x.Decrypt(encryptedId)).Returns(this.id);

            this.controller = new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOprovider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }


        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProvider()
        {
            // Arrange

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedId), Times.Once);
        }

        [Test]
        public void ShouldCallDoesExistFromBikeService()
        {
            // Arrange

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockBikeService.Verify(x => x.DoesExist(this.id), Times.Once);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenThereIsNoSuchBIkewithThatId()
        {
            // Arrange
            this.mockBikeService.Setup(x => x.DoesExist(this.id)).Returns(false);

            // Act
            var result = this.controller.Delete(this.encryptedId) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void ShouldNotCallDeleteFromBikeService_WhenThereIsNoSuchBIkewithThatId()
        {
            // Arrange
            this.mockBikeService.Setup(x => x.DoesExist(this.id)).Returns(false);

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockBikeService.Verify(x => x.Delete(this.id), Times.Never);
        }

        [Test]
        public void ShouldCallDeleteFromBikeService_WhenThereIsSuchBIkewithThatId()
        {
            // Arrange

            // Act
            this.controller.Delete(this.encryptedId);

            // Assert
            this.mockBikeService.Verify(x => x.Delete(this.id), Times.Once);
        }

        [Test]
        public void ShouldRedirectResultRedirectictToIndex__WhenThereIsSuchBIkewithThatId()
        {
            // Arrange

            // Act
            var result = this.controller.Delete(this.encryptedId) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            StringAssert.Contains("Index", result.ActionName);
        }
    }
}
