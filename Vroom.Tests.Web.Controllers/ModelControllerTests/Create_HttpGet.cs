using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
using Vroom.Service.Models.Contracts;

namespace Vroom.Tests.Web.Controllers.ModelControllerTests
{
    [TestFixture]
    public class Create_HttpGet
    {
        private List<MakeServiceModel> collectionOfMakeServiceModels;
        private List<MakeViewModel> collectionOfMakeViewModels;
        private List<SelectListItem> collectionOfSelectListItems;
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;
        private ModelController controller;

        [SetUp]
        public void Setup()
        {

            this.collectionOfMakeServiceModels = new List<MakeServiceModel>() { new MakeServiceModel { Id = 3 } };
            this.collectionOfMakeViewModels = new List<MakeViewModel> { new MakeViewModel { Id = "3" } };
            this.collectionOfSelectListItems = new List<SelectListItem> { new SelectListItem { Value = "3", Text = "yoyo" } };

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockMakeService.Setup(x => x.GetAll()).Returns(this.collectionOfMakeServiceModels);
            this.mockMapper.Setup(x => x.Map<IEnumerable<IMakeServiceModel>, IEnumerable<MakeViewModel>>(this.collectionOfMakeServiceModels)).Returns(this.collectionOfMakeViewModels);
            this.mockSelectItemFactory.Setup(x => x.GetSelectList(this.collectionOfMakeViewModels, "0")).Returns(this.collectionOfSelectListItems);

            this.controller = new ModelController(this.mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }

        [Test]
        public void ShouldCallGetAllMethodFromMakeService()
        {
            // Arrange

            // Act
            this.controller.Create();
            // Assert
            this.mockMakeService.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void ShouldCallMapMethodFromMapper()
        {
            // Arrange

            // Act
            this.controller.Create();
            // Assert
            this.mockMapper.Verify(x => x.Map<IEnumerable<IMakeServiceModel>, IEnumerable<MakeViewModel>>(this.collectionOfMakeServiceModels), Times.Once);
        }

        [Test]
        public void ShouldCallGetSelectListMethodFromelectItemFactory()
        {
            // Arrange

            // Act
            this.controller.Create();
            // Assert
            this.mockSelectItemFactory.Setup(x => x.GetSelectList(this.collectionOfMakeViewModels, "0"));
        }

        [Test]
        public void ShouldReturnViewResult()
        {
            // Arrange

            // Act
            var result = this.controller.Create() as ViewResult;
            // Assert

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModel()
        {
            // Arrange

            // Act
            var result = this.controller.Create() as ViewResult;
            var model = result.Model as CreateViewModel;
            // Assert

            Assert.AreEqual(this.collectionOfSelectListItems, model.MakeViewModels);
        }
    }
}
