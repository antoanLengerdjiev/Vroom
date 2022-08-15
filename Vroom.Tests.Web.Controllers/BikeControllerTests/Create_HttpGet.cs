using AutoMapper;
using Microsoft.AspNetCore.Hosting;
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

namespace Vroom.Tests.Web.Controllers.BikeControllerTests
{
    [TestFixture]
    public class Create_HttpGet
    {
        private int id;
        private BikeServiceModel bikeServiceModel;
        private BikeViewModel bikeViewModel;
        private List<MakeServiceModel> collectionOfMakes;
        private List<ModelServiceModel> collectionOfModles;
        private List<MakeViewModel> collectionOfMakesViewModels;
        private List<ModelViewModel> collectionOfModlesViewModels;
        private List<SelectListItem> selectMakeItemList;
        private List<SelectListItem> selectModelItemList;
        private Mock<IWebHostEnvironment> mockHostingEnvironment;
        private Mock<IMapper> mockMapper;
        private Mock<IBikeService> mockBikeService;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IHttpContextProvider> mockHttpContextProvider;
        private Mock<ICacheProvider> mockCacheProvider;
        private Mock<IIOProvider> mockIOprovider;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;
        private BikeController controller;

        [SetUp]
        public void Setup()
        {
            this.id = 3;
            this.bikeServiceModel = new BikeServiceModel();
            this.bikeViewModel = new BikeViewModel();

            this.collectionOfMakes = new List<MakeServiceModel>() { new MakeServiceModel() { Id = 3 } };
            this.collectionOfModles = new List<ModelServiceModel>() { new ModelServiceModel() { Id = 3 } };

            this.collectionOfMakesViewModels = new List<MakeViewModel>() { new MakeViewModel() { Id = "3" } };
            this.collectionOfModlesViewModels = new List<ModelViewModel>() { new ModelViewModel() { Id = "3" } };
            this.selectMakeItemList = new List<SelectListItem>() { new SelectListItem() { Text = "make", Value= "123"} };
            this.selectModelItemList = new List<SelectListItem>() { new SelectListItem() { Text = "model", Value = "321" } };


            this.mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            this.mockMapper = new Mock<IMapper>();
            this.mockBikeService = new Mock<IBikeService>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockHttpContextProvider = new Mock<IHttpContextProvider>();
            this.mockCacheProvider = new Mock<ICacheProvider>();
            this.mockIOprovider = new Mock<IIOProvider>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockBikeService.Setup(x => x.GetById(this.id)).Returns(this.bikeServiceModel);
            this.mockMapper.Setup(x => x.Map<BikeServiceModel, BikeViewModel>(this.bikeServiceModel)).Returns(this.bikeViewModel);

            this.mockMakeService.Setup(x => x.GetAll()).Returns(this.collectionOfMakes);
            this.mockMapper.Setup(x => x.Map<List<MakeServiceModel>, List<MakeViewModel>>(this.collectionOfMakes)).Returns(this.collectionOfMakesViewModels);

            this.mockModelService.Setup(x => x.GetAll()).Returns(this.collectionOfModles);
            this.mockMapper.Setup(x => x.Map<List<ModelServiceModel>, List<ModelViewModel>>(this.collectionOfModles)).Returns(this.collectionOfModlesViewModels);

            this.mockSelectItemFactory.Setup(x => x.GetSelectList(this.collectionOfMakesViewModels,"0")).Returns(this.selectMakeItemList);
            this.mockSelectItemFactory.Setup(x => x.GetSelectList(this.collectionOfModlesViewModels, "0")).Returns(this.selectModelItemList);

            this.controller = new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOprovider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
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
        public void ShouldCallMapMethodFromMapper_WhenMappingMakes()
        {
            // Arrange 

            // Act
            this.controller.Create();
            // Assert
            this.mockMapper.Verify(x => x.Map<List<MakeServiceModel>, List<MakeViewModel>>(this.collectionOfMakes), Times.Once);
        }

        [Test]
        public void ShouldCallGetAllMethodFromModelService()
        {
            // Arrange 

            // Act
            this.controller.Create();
            // Assert
            this.mockModelService.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void ShouldCallMapMethodFromMapper_WhenMappingModels()
        {
            // Arrange 

            // Act
            this.controller.Create();
            // Assert
            this.mockMapper.Verify(x => x.Map<List<ModelServiceModel>, List<ModelViewModel>>(this.collectionOfModles), Times.Once);
        }

        [Test]
        public void ShouldCallGetSelectListMethodFromSelectItemFactory_WhenTurningMakeViewModelIntoSelectListItem()
        {
            // Arrange 

            // Act
            this.controller.Create();
            // Assert
            this.mockSelectItemFactory.Verify(x => x.GetSelectList(this.collectionOfMakesViewModels, "0"), Times.Once);
        }

        [Test]
        public void ShouldCallGetSelectListMethodFromSelectItemFactory_WhenTurningModelViewModelIntoSelectListItem()
        {
            // Arrange 

            // Act
            this.controller.Create();
            // Assert
            this.mockSelectItemFactory.Verify(x => x.GetSelectList(this.collectionOfModlesViewModels, "0"), Times.Once);
        }

        [Test]
        public void ShouldReturnViewResult()
        {
            // Arrange 

            // Act
            var result = this.controller.Create();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectTypeOfModel()
        {
            // Arrange 

            // Act
            var result = this.controller.Create() as ViewResult;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.IsInstanceOf<CreateBikeViewModel>(result.Model);
        }


        [Test]
        public void ShouldSetMakePropertyOfModel()
        {
            // Arrange 

            // Act
            var result = this.controller.Create() as ViewResult;
            var model = result.Model as CreateBikeViewModel;
            // Assert
            Assert.AreEqual(this.selectMakeItemList, model.Makes);
        }

        [Test]
        public void ShouldSetModelPropertyOfModel()
        {
            // Arrange 

            // Act
            var result = this.controller.Create() as ViewResult;
            var model = result.Model as CreateBikeViewModel;
            // Assert
            Assert.AreEqual(this.selectModelItemList, model.Models);
        }
    }
}
