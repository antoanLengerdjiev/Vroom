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
    public class Create_HttpPost
    {
        private ModelServiceModel modelServiceModels;
        private CreateViewModel createViewModel;
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;
        private ModelController controller;

        [SetUp]
        public void Setup()
        {

            this.modelServiceModels = new ModelServiceModel { Id = 3 };
            this.createViewModel = new CreateViewModel() { Id = "3" };
            

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockMapper.Setup(x => x.Map<CreateViewModel, ModelServiceModel>(this.createViewModel)).Returns(this.modelServiceModels);
            

            this.controller = new ModelController(this.mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }

        [Test]
        public void ShouldReturnView_WhenModelstateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            var result = this.controller.Create(this.createViewModel) as ViewResult;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.IsInstanceOf<CreateViewModel>(result.Model);
        }

        [Test]
        public void ShouldCallMapMethodFromMapper_WhenModelstateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createViewModel);
            // Assert
            this.mockMapper.Verify(x => x.Map<CreateViewModel, ModelServiceModel>(this.createViewModel), Times.Once);
        }

        [Test]
        public void ShouldCallAddMethodFromModelService_WhenModelstateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createViewModel);
            // Assert
            this.mockModelService.Verify(x => x.Add(this.modelServiceModels), Times.Once);
        }

        [Test]
        public void ShouldRedirectResultRedirectictToIndex_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            var result = this.controller.Create(this.createViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            StringAssert.Contains("Index", result.ActionName);
        }
    }
}
