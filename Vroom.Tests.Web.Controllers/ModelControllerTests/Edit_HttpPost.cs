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
    public class Edit_HttpPost
    {
        private int makeId;
        private string encryptedMakeId;
        private ModelServiceModel modelServiceModels;
        private CreateViewModel createViewModel;
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

            this.makeId = 5;
            this.encryptedMakeId = "5";

            this.createViewModel = new CreateViewModel() { Id = this.encryptedId, MakeId = this.encryptedMakeId };

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.encryptedId)).Returns(this.id);
            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.encryptedMakeId)).Returns(this.makeId);
            this.mockModelService.Setup(x => x.DoesExist(this.id)).Returns(true);
            this.mockMakeService.Setup(x => x.DoesExist(this.makeId)).Returns(true);

            this.controller = new ModelController(this.mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenModelstateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            var result = this.controller.Edit(this.createViewModel) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProviderToDecryptId_WhenModelstateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createViewModel);
            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedId), Times.Once);
        }

        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProviderToDecryptMakeId_WhenModelstateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createViewModel);
            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedMakeId), Times.Once);
        }

        [Test]
        public void ShouldCallDoesExistMethodFromModelService_WhenModelstateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createViewModel);
            // Assert
            this.mockModelService.Verify(x => x.DoesExist(this.id), Times.Once);
        }

        [Test]
        public void ShouldCallDoesExistMethodFromMakeService_WhenModelstateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createViewModel);
            // Assert
            this.mockMakeService.Verify(x => x.DoesExist(this.makeId), Times.Once);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenModelstateIsValidAndMakeDoestNotExistWithThatId()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(this.makeId)).Returns(false);

            // Act
            var result = this.controller.Edit(this.createViewModel) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenModelstateIsValidAndModelDoestNotExistWithThatId()
        {
            // Arrange
            this.mockModelService.Setup(x => x.DoesExist(this.id)).Returns(false);

            // Act
            var result = this.controller.Edit(this.createViewModel) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void ShouldCallUpdateMethodFromModelService_WhenModelstateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createViewModel);
            // Assert
            this.mockModelService.Verify(x => x.Update(this.id,this.createViewModel.Name ,this.makeId), Times.Once);
        }

        [Test]
        public void ShouldRedirectResultRedirectictToIndex_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            var result = this.controller.Edit(this.createViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            StringAssert.Contains("Index", result.ActionName);
        }
    }
}
