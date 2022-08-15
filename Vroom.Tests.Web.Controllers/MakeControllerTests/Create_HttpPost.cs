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

namespace Vroom.Tests.Web.Controllers.MakeControllerTests
{
    [TestFixture]
    public class Create_HttpPost
    {
        private MakeServiceModel makeServiceModel;
        private MakeViewModel makeViewModel;
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private MakeController controller;

        [SetUp]
        public void Setup()
        {
            this.makeServiceModel = new MakeServiceModel();
            this.makeViewModel = new MakeViewModel();

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();

            this.mockMapper.Setup(x => x.Map<MakeViewModel, MakeServiceModel>(this.makeViewModel)).Returns(this.makeServiceModel);

            this.controller = new MakeController(this.mockMapper.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object);
        }


        [Test]
        public void ShouldReturnViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bbla", "blla");

            // Act
            var result = this.controller.Create(this.makeViewModel) as ViewResult;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModel_WhenModelStateIsInvalid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bbla", "blla");

            // Act
            var result = this.controller.Create(this.makeViewModel) as ViewResult;
            var model = result.Model as MakeViewModel;
            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.AreEqual(this.makeViewModel, model);
        }

        [Test]
        public void ShouldCallMapMethodFromMapper_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.makeViewModel);

            // Assert
            this.mockMapper.Verify(x => x.Map<MakeViewModel, MakeServiceModel>(this.makeViewModel), Times.Once);
        }

        [Test]
        public void ShouldCallAddMethodFromMakeService_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.makeViewModel);
            // Assert
            this.mockMakeService.Verify(x => x.Add(this.makeServiceModel), Times.Once);
        }


        [Test]
        public void ShouldReturnRedirectToActionResultWithCorrectActionName_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            var result = this.controller.Create(this.makeViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
