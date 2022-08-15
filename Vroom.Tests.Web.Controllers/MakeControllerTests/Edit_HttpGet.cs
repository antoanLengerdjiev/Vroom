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
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;
using Vroom.Service.Models.Contracts;

namespace Vroom.Tests.Web.Controllers.MakeControllerTests
{
    public class Edit_HttpGet
    {
        private string encryptedId;
        private int id;
        private MakeServiceModel makeServiceModel;
        private MakeViewModel makeViewModel;
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private MakeController controller;

        [SetUp]
        public void Setup()
        {
            this.encryptedId = "encryptedId";
            this.id = 3;

            this.makeServiceModel = new MakeServiceModel();
            this.makeViewModel = new MakeViewModel();

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();

            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.encryptedId)).Returns(this.id);
            this.mockMakeService.Setup(x => x.GetById(this.id)).Returns(this.makeServiceModel);
            this.mockMapper.Setup(x => x.Map<IMakeServiceModel,MakeViewModel>(this.makeServiceModel)).Returns(this.makeViewModel);

            this.controller = new MakeController(this.mockMapper.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object);
        }
        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProvider()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedId), Times.Once);
        }

        [Test]
        public void ShouldCallGetByIdMethodFromMakeService()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);

            // Assert
            this.mockMakeService.Verify(x => x.GetById(this.id), Times.Once);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenThereIsNoSuchMakeWithThatId()
        {
            // Arrange
            this.makeServiceModel = null;
            this.mockMakeService.Setup(x => x.GetById(this.id)).Returns(this.makeServiceModel);

            // Act
            var result = this.controller.Edit(this.encryptedId) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);

        }

        [Test]
        public void ShouldCallMapMethodFromMapper()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);

            // Assert
            this.mockMapper.Verify(x => x.Map<IMakeServiceModel, MakeViewModel>(this.makeServiceModel), Times.Once);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModel()
        {
            // Arrange

            // Act
            var result = this.controller.Edit(this.encryptedId) as ViewResult;
            var model = result.Model as MakeViewModel;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.AreEqual(this.makeViewModel, model);
        }
    }
}
