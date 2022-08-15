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
    [TestFixture]
    public class Edit_HttpPost
    {
        private string encryptedId;
        private int id;
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

            this.makeViewModel = new MakeViewModel() { Id = this.encryptedId, Name = "Name"};

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();

            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.encryptedId)).Returns(this.id);
            this.mockMakeService.Setup(x => x.DoesExist(this.id)).Returns(true);

            this.controller = new MakeController(this.mockMapper.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object);
        }

        [Test]
        public void ShouldReturnViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bbla", "blla");

            // Act
            var result = this.controller.Edit(this.makeViewModel) as ViewResult;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModel_WhenModelStateIsInvalid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bbla", "blla");

            // Act
            var result = this.controller.Edit(this.makeViewModel) as ViewResult;
            var model = result.Model as MakeViewModel;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.AreEqual(this.makeViewModel, model);
        }

        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProvider()
        {
            // Arrange

            // Act
            this.controller.Edit(this.makeViewModel);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedId), Times.Once);
        }

        [Test]
        public void ShouldCallDoesExistMethodFromMakeService_WhenModelIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.makeViewModel);

            // Assert
            this.mockMakeService.Verify(x => x.DoesExist(this.id), Times.Once);
        }

        [Test]
        public void ShouldReturnViewResult_WhenModelIsValidAndMakeWithThatIdDoesNotExist()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(this.id)).Returns(false);
            // Act
            var result = this.controller.Edit(this.makeViewModel) as ViewResult;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModel_WhenModelIsValidAndMakeWithThatIdDoesNotExist()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(this.id)).Returns(false);

            // Act
            var result = this.controller.Edit(this.makeViewModel) as ViewResult;
            var model = result.Model as MakeViewModel;
            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.AreEqual(this.makeViewModel, model);
        }

        [Test]
        public void ShouldCallUpdateNameMethodFromMakeService_WhenModelIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.makeViewModel);

            // Assert
            this.mockMakeService.Verify(x => x.UpdateName(this.id, this.makeViewModel.Name), Times.Once);
        }

        [Test]
        public void ShouldReturnRedirectToActionResult_WhenModelStateIsInvalid()
        {
            // Arrange


            // Act
            var result = this.controller.Edit(this.makeViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Test]
        public void ShouldReturnRedirectToActionResultWithCorrectModel_WhenModelStateIsInvalid()
        {
            // Arrange

            // Act
            var result = this.controller.Edit(this.makeViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.AreEqual("Index", result.ActionName);
        }

    }
}
