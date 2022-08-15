using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

namespace Vroom.Tests.Web.Controllers.BikeControllerTests
{
    [TestFixture]
    public class Create_HttpPost
    {
        private int id;
        private string userId;
        private string wwRootPath;
        private string relativeImagePath;
        private BikeServiceModel bikeServiceModel;
        private BikeViewModel bikeViewModel;
        private CreateBikeViewModel createBikeViewModel;
        private Mock<IFormFileCollection> mockFormCollection;
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
            this.userId = "userId";
            this.wwRootPath = "WebRootPath";
            this.relativeImagePath = "RelativeImagePath";
            this.bikeServiceModel = new BikeServiceModel();
            this.bikeViewModel = new BikeViewModel();
            this.createBikeViewModel = new CreateBikeViewModel() { Bike = this.bikeViewModel};
            this.mockFormCollection = new Mock<IFormFileCollection>();



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

            this.mockFormCollection.Setup(x => x.Count).Returns(1);
            this.mockFormCollection.Setup(x => x[0].FileName).Returns("FileName");

            this.mockBikeService.Setup(x => x.Add(this.bikeServiceModel)).Returns(this.id);
            this.mockMapper.Setup(x => x.Map<BikeViewModel, BikeServiceModel>(this.bikeViewModel)).Returns(this.bikeServiceModel);
            this.mockHttpContextProvider.Setup(x => x.GetCurrentUsedId()).Returns(this.userId);
            this.mockHostingEnvironment.Setup(x => x.WebRootPath).Returns(this.wwRootPath);
            this.mockHttpContextProvider.Setup(x => x.GetPostedFormFiles()).Returns(this.mockFormCollection.Object);
            this.mockIOprovider.Setup(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object)).Returns(this.relativeImagePath);

            this.controller = new BikeController(this.mockHostingEnvironment.Object, this.mockMapper.Object,
                this.mockBikeService.Object, this.mockMakeService.Object,
                this.mockModelService.Object, this.mockHttpContextProvider.Object,
                this.mockCacheProvider.Object, this.mockIOprovider.Object,
                this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }
        [Test]
        public void ShouldReturnView_WhenModelstateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            var result = this.controller.Create(this.createBikeViewModel) as ViewResult;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            Assert.IsInstanceOf<CreateBikeViewModel>(result.Model);
        }
        [Test]
        public void ShouldCallMapMethodFromMapperToMapBikeViewModelToBikeServiceModel_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockMapper.Verify(x => x.Map<BikeViewModel, BikeServiceModel>(this.bikeViewModel), Times.Once);
        }

        [Test]
        public void ShouldCallGetCurrentUsedIdFromHttpContextProvider_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetCurrentUsedId(), Times.Once);
        }

        [Test]
        public void ShouldSetSellerIdOfBikeServiceModelToUserId_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            Assert.AreEqual(this.userId, this.bikeServiceModel.SellerId);
        }

        [Test]
        public void ShouldCallAddFromBikeService_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.Add(this.bikeServiceModel), Times.Once);
        }

        [Test]
        public void ShouldCallWebRootPathFromHostingEnvironment_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockHostingEnvironment.Verify(x => x.WebRootPath, Times.Once);
        }

        [Test]
        public void ShouldCallGetPostedFormFilesFromHttpContextProvider_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetPostedFormFiles(), Times.Once);
        }


        [Test]
        public void ShouldCallSaveImgFromIOProvider_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id,this.wwRootPath,this.mockFormCollection.Object), Times.Once);
        }

        [Test]
        public void ShouldCallUpdateImgFromBikeService_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.UpdateImg(this.id, this.relativeImagePath), Times.Once);
        }

        [Test]
        public void ShouldNotCallSaveImgFromIOProvider_WhenModelStateIsValidAndFormFileCollectionIsEmpty()
        {
            // Arrange
            this.mockFormCollection.Setup(x => x.Count).Returns(0);

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object), Times.Never);
        }

        [Test]
        public void ShouldNotCallUpdateImgFromBikeService_WhenModelStateIsValidAndFormFileCollectionIsEmpty()
        {
            // Arrange
            this.mockFormCollection.Setup(x => x.Count).Returns(0);

            // Act
            this.controller.Create(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.UpdateImg(this.id, this.relativeImagePath), Times.Never);
        }

        [Test]
        public void ShouldRedirectResultRedirectictToIndex_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            var result = this.controller.Create(this.createBikeViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            StringAssert.Contains("Index",result.ActionName);
        }
    }
}
