using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Vroom.Controllers;
using Vroom.Models;
using Vroom.Models.Factories.NewFolder;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;

namespace Vroom.Tests.Web.Controllers.ModelControllerTests
{
    [TestFixture]
    public class Index
    {
        private List<ModelServiceModel> collectionOfModelServiceModels;
        private List<ModelViewModel> collectionOfModelViewModels;
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;
        private ModelController controller;

        [SetUp]
        public void Setup()
        {

            this.collectionOfModelServiceModels = new List<ModelServiceModel>() { new ModelServiceModel { Id = 3 } };
            this.collectionOfModelViewModels = new List<ModelViewModel> { new ModelViewModel { Id = "3" } };

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockModelService.Setup(x => x.GetAll()).Returns(this.collectionOfModelServiceModels);
            this.mockMapper.Setup(x => x.Map<List<ModelServiceModel>, List<ModelViewModel>>(this.collectionOfModelServiceModels)).Returns(this.collectionOfModelViewModels);

            this.controller = new ModelController(this.mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }

        [Test]
        public void ShouldCallGetAllMethodFromModelService()
        {
            // Arrange

            // Act
            this.controller.Index();
            // Assert
            this.mockModelService.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void ShouldCallMapMethodFromMapper()
        {
            // Arrange

            // Act
            this.controller.Index();
            // Assert
            this.mockMapper.Verify(x => x.Map<List<ModelServiceModel>, List<ModelViewModel>>(this.collectionOfModelServiceModels), Times.Once);
        }

        [Test]
        public void ShouldReturnViewResult()
        {
            // Arrange

            // Act
            var result = this.controller.Index() as ViewResult;
            // Assert

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModel()
        {
            // Arrange

            // Act
            var result = this.controller.Index() as ViewResult;
            // Assert

            Assert.AreEqual(this.collectionOfModelViewModels,result.Model);
        }

    }
}
