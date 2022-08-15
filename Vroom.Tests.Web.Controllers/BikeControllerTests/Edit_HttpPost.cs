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
    public class Edit_HttpPost
    {
        private int id;
        private string stringId;
        private int makeId;
        private int modelId;
        private string encryptedMakeId;
        private string encryptedModelId;
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
            this.stringId = "encryptedId";
            this.makeId = 3;
            this.modelId = 3;
            this.encryptedMakeId = "makeId";
            this.encryptedModelId = "ModelId";
            this.userId = "userId";
            this.wwRootPath = "WebRootPath";
            this.relativeImagePath = "RelativeImagePath";
            this.bikeServiceModel = new BikeServiceModel() { Id = this.id, Seller = new ApplicationUserServiceModel() { Id = this.userId } };
            this.bikeViewModel = new BikeViewModel() { Id = this.stringId, MakeId = this.encryptedMakeId, ModelId = this.encryptedModelId};
            this.createBikeViewModel = new CreateBikeViewModel() { Bike = this.bikeViewModel };
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

            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.bikeViewModel.Id)).Returns(this.id);
            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.bikeViewModel.MakeId)).Returns(this.makeId);
            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.bikeViewModel.ModelId)).Returns(this.modelId);

            this.mockMakeService.Setup(x => x.DoesExist(this.makeId)).Returns(true);
            this.mockModelService.Setup(x => x.DoesExist(this.modelId)).Returns(true);
            this.mockBikeService.Setup(x => x.GetById(this.id)).Returns(this.bikeServiceModel);
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
        public void ShouldReturnNotFoundResult_WhenModelstateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            var result = this.controller.Edit(this.createBikeViewModel) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);           
        }

        [Test]
        public void ShouldNotCallGetCurrentUsedIdFromHttpContextProvider_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");
            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetCurrentUsedId(), Times.Never);
        }

        [Test]
        public void ShouldNotCallDecryptIdMethodFromEncryptionProvider_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.stringId), Times.Never);
        }

        [Test]
        public void ShouldNotCallDecryptMakeIdMethodFromEncryptionProvider_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedMakeId), Times.Never);
        }
        [Test]
        public void ShouldNotCallDecryptModelIdMethodFromEncryptionProvider_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedModelId), Times.Never);
        }

        [Test]
        public void ShouldNotCallDoesExistMethodFromMakeService_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockMakeService.Verify(x => x.DoesExist(this.makeId), Times.Never);
        }

        [Test]
        public void ShouldNotCallDoesExistMethodFromModelService_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");
            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockModelService.Verify(x => x.DoesExist(this.modelId), Times.Never);
        }

        [Test]
        public void ShouldNotCallGetByIdFromBikeService_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");
            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.GetById(this.id), Times.Never);
        }

        [Test]
        public void ShouldNotCallMapMethodFromMapperToMapBikeViewModelToBikeServiceModel_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockMapper.Verify(x => x.Map<BikeViewModel, BikeServiceModel>(this.bikeViewModel), Times.Never);
        }

        [Test]
        public void ShouldNotCallWebRootPathFromHostingEnvironment_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");
            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHostingEnvironment.Verify(x => x.WebRootPath, Times.Never);
        }

        [Test]
        public void ShouldNotCallGetPostedFormFilesFromHttpContextProvider_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetPostedFormFiles(), Times.Never);
        }


        [Test]
        public void ShouldNotCallSaveImgFromIOProvider_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object), Times.Never);
        }

        [Test]
        public void ShouldNotSetImgPathToBikeServiceModel_WhenModelStateIsNotValidAndFormFileCollectionIsNotEmpty()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            Assert.AreNotEqual(this.relativeImagePath, this.bikeServiceModel.ImagePath);
        }
        [Test]
        public void ShouldNotCallUpdateFromBikeService_WhenModelStateIsNotValid()
        {
            // Arrange
            this.controller.ModelState.AddModelError("bla", "bla");

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.Update(this.bikeServiceModel), Times.Never);
        }
        [Test]
        public void ShouldCallGetCurrentUsedIdFromHttpContextProvider_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetCurrentUsedId(), Times.Once);
        }

        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProvider_DecryptedBikeId()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.stringId), Times.Once);
        }

        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProvider_DecryptedMakeId()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedMakeId), Times.Once);
        }
        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProvider_DecryptedModelId()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedModelId), Times.Once);
        }

        [Test]
        public void ShouldCallDoesExistMethodFromMakeService_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockMakeService.Verify(x => x.DoesExist(this.makeId), Times.Once);
        }

        [Test]
        public void ShouldCallDoesExistMethodFromModelService_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockModelService.Verify(x => x.DoesExist(this.modelId), Times.Once);
        }

        [Test]
        public void ShouldCallGetByIdFromBikeService_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.GetById(this.id), Times.Once);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenModelstateIsValidButThereIsNoSuchMakeWithThatMakeId()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(makeId)).Returns(false);

            // Act
            var result = this.controller.Edit(this.createBikeViewModel) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }
        [Test]
        public void ShouldNotCallMapMethodFromMapperToMapBikeViewModelToBikeServiceModel_WhenModelstateIsValidButThereIsNoSuchMakeWithThatMakeId()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(makeId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockMapper.Verify(x => x.Map<BikeViewModel, BikeServiceModel>(this.bikeViewModel), Times.Never);
        }

        [Test]
        public void ShouldNotCallWebRootPathFromHostingEnvironment_WhenModelstateIsValidButThereIsNoSuchMakeWithThatMakeId()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(makeId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHostingEnvironment.Verify(x => x.WebRootPath, Times.Never);
        }

        [Test]
        public void ShouldNotCallGetPostedFormFilesFromHttpContextProvider_WhenModelstateIsValidButThereIsNoSuchMakeWithThatMakeId()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(makeId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetPostedFormFiles(), Times.Never);
        }


        [Test]
        public void ShouldNotCallSaveImgFromIOProvider_WhenModelstateIsValidButThereIsNoSuchMakeWithThatMakeId()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(makeId)).Returns(false);
            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object), Times.Never);
        }

        [Test]
        public void ShouldNotSetImgPathToBikeServiceModel_WhenModelstateIsValidButThereIsNoSuchMakeWithThatMakeId()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(makeId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            Assert.AreNotEqual(this.relativeImagePath, this.bikeServiceModel.ImagePath);
        }
        [Test]
        public void ShouldNotCallUpdateFromBikeService_WhenModelstateIsValidButThereIsNoSuchMakeWithThatMakeId()
        {
            // Arrange
            this.mockMakeService.Setup(x => x.DoesExist(makeId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.Update(this.bikeServiceModel), Times.Never);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenModelstateIsValidButThereIsNoSuchModelWithThatModelId()
        {
            // Arrange
            this.mockModelService.Setup(x => x.DoesExist(modelId)).Returns(false);

            // Act
            var result = this.controller.Edit(this.createBikeViewModel) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void ShouldNotCallMapMethodFromMapperToMapBikeViewModelToBikeServiceModel_WhenModelstateIsValidButThereIsNoSuchModelWithThatModelId()
        {
            // Arrange
            this.mockModelService.Setup(x => x.DoesExist(modelId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockMapper.Verify(x => x.Map<BikeViewModel, BikeServiceModel>(this.bikeViewModel), Times.Never);
        }

        [Test]
        public void ShouldNotCallWebRootPathFromHostingEnvironment_WhenModelstateIsValidButThereIsNoSuchModelWithThatModelId()
        {
            // Arrange
            this.mockModelService.Setup(x => x.DoesExist(modelId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHostingEnvironment.Verify(x => x.WebRootPath, Times.Never);
        }

        [Test]
        public void ShouldNotCallGetPostedFormFilesFromHttpContextProvider_WhenModelstateIsValidButThereIsNoSuchModelWithThatModelId()
        {
            // Arrange
            this.mockModelService.Setup(x => x.DoesExist(modelId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetPostedFormFiles(), Times.Never);
        }


        [Test]
        public void ShouldNotCallSaveImgFromIOProvider_WhenModelstateIsValidButThereIsNoSuchModelWithThatModelId()
        {
            // Arrange
            this.mockModelService.Setup(x => x.DoesExist(modelId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object), Times.Never);
        }

        [Test]
        public void ShouldNotSetImgPathToBikeServiceModel_WhenModelstateIsValidButThereIsNoSuchModelWithThatModelId()
        {
            // Arrange
            this.mockModelService.Setup(x => x.DoesExist(modelId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            Assert.AreNotEqual(this.relativeImagePath, this.bikeServiceModel.ImagePath);
        }
        [Test]
        public void ShouldNotCallUpdateFromBikeService_WhenModelstateIsValidButThereIsNoSuchModelWithThatModelId()
        {
            // Arrange
            this.mockModelService.Setup(x => x.DoesExist(modelId)).Returns(false);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.Update(this.bikeServiceModel), Times.Never);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenModelstateIsValidButThereIsNoSuchBikeWithThatBikeId()
        {
            // Arrange
            this.bikeServiceModel = null;
            this.mockBikeService.Setup(x => x.GetById(this.id)).Returns(this.bikeServiceModel);

            // Act
            var result = this.controller.Edit(this.createBikeViewModel) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void ShouldNotCallMapMethodFromMapperToMapBikeViewModelToBikeServiceModel_WhenModelstateIsValidButThereIsNoSuchBikeWithThatBikeId()
        {
            // Arrange
            this.bikeServiceModel = null;
            this.mockBikeService.Setup(x => x.GetById(this.id)).Returns(this.bikeServiceModel);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockMapper.Verify(x => x.Map<BikeViewModel, BikeServiceModel>(this.bikeViewModel), Times.Never);
        }

        [Test]
        public void ShouldNotCallWebRootPathFromHostingEnvironment_WhenModelstateIsValidButThereIsNoSuchBikeWithThatBikeId()
        {
            // Arrange
            this.bikeServiceModel = null;
            this.mockBikeService.Setup(x => x.GetById(this.id)).Returns(this.bikeServiceModel);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHostingEnvironment.Verify(x => x.WebRootPath, Times.Never);
        }

        [Test]
        public void ShouldNotCallGetPostedFormFilesFromHttpContextProvider_WhenModelstateIsValidButThereIsNoSuchBikeWithThatBikeId()
        {
            // Arrange
            this.bikeServiceModel = null;
            this.mockBikeService.Setup(x => x.GetById(this.id)).Returns(this.bikeServiceModel);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetPostedFormFiles(), Times.Never);
        }


        [Test]
        public void ShouldNotCallSaveImgFromIOProvider_WhenModelstateIsValidButThereIsNoSuchBikeWithThatBikeId()
        {
            // Arrange
            this.bikeServiceModel = null;
            this.mockBikeService.Setup(x => x.GetById(this.id)).Returns(this.bikeServiceModel);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object), Times.Never);
        }

        
        [Test]
        public void ShouldNotCallUpdateFromBikeService_WhenModelstateIsValidButThereIsNoSuchBikeWithThatBikeId()
        {
            // Arrange
            this.bikeServiceModel = null;
            this.mockBikeService.Setup(x => x.GetById(this.id)).Returns(this.bikeServiceModel);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.Update(this.bikeServiceModel), Times.Never);
        }

        [Test]
        public void ShouldReturnBadRequestResult_WhenModelstateIsValidButSellerIdPropertyFromBIkeDoesNotMatchCurrentUserId()
        {
            // Arrange
            this.bikeServiceModel.Seller.Id = "userId2";

            // Act
            var result = this.controller.Edit(this.createBikeViewModel) as BadRequestResult;

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Test]
        public void ShouldNotCallMapMethodFromMapperToMapBikeViewModelToBikeServiceModel_WhenModelstateIsValidButSellerIdPropertyFromBIkeDoesNotMatchCurrentUserId()
        {
            // Arrange
            this.bikeServiceModel.Seller.Id = "userId2";

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockMapper.Verify(x => x.Map<BikeViewModel, BikeServiceModel>(this.bikeViewModel), Times.Never);
        }

        [Test]
        public void ShouldNotCallWebRootPathFromHostingEnvironment_WhenModelstateIsValidButSellerIdPropertyFromBIkeDoesNotMatchCurrentUserId()
        {
            // Arrange
            this.bikeServiceModel.Seller.Id = "userId2";

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHostingEnvironment.Verify(x => x.WebRootPath, Times.Never);
        }

        [Test]
        public void ShouldNotCallGetPostedFormFilesFromHttpContextProvider_WhenModelstateIsValidButSellerIdPropertyFromBIkeDoesNotMatchCurrentUserIdd()
        {
            // Arrange
            this.bikeServiceModel.Seller.Id = "userId2";

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetPostedFormFiles(), Times.Never);
        }


        [Test]
        public void ShouldNotCallSaveImgFromIOProvider_WhenModelstateIsValidButSellerIdPropertyFromBIkeDoesNotMatchCurrentUserId()
        {
            // Arrange
            this.bikeServiceModel.Seller.Id = "userId2";

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object), Times.Never);
        }


        [Test]
        public void ShouldNotSetImgPathToBikeServiceModel_WhenModelstateIsValidButSellerIdPropertyFromBIkeDoesNotMatchCurrentUserId()
        {
            // Arrange
            this.bikeServiceModel.Seller.Id = "userId2";

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            Assert.AreNotEqual(this.relativeImagePath, this.bikeServiceModel.ImagePath);
        }


        [Test]
        public void ShouldNotCallUpdateFromBikeService_WhenModelstateIsValidButSellerIdPropertyFromBIkeDoesNotMatchCurrentUserId()
        {
            // Arrange
            this.bikeServiceModel.Seller.Id = "userId2";

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.Update(this.bikeServiceModel), Times.Never);
        }

        [Test]
        public void ShouldCallMapMethodFromMapperToMapBikeViewModelToBikeServiceModel_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockMapper.Verify(x => x.Map<BikeViewModel, BikeServiceModel>(this.bikeViewModel), Times.Once);
        }

        [Test]
        public void ShouldCallWebRootPathFromHostingEnvironment_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHostingEnvironment.Verify(x => x.WebRootPath, Times.Once);
        }

        [Test]
        public void ShouldCallGetPostedFormFilesFromHttpContextProvider_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockHttpContextProvider.Verify(x => x.GetPostedFormFiles(), Times.Once);
        }


        [Test]
        public void ShouldCallSaveImgFromIOProvider_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object), Times.Once);
        }

        [Test]
        public void ShouldSetImgPathToBikeServiceModel_WhenModelStateIsValidAndFormFileCollectionIsNotEmpty()
        {
            // Arrange
            

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            Assert.AreEqual(this.relativeImagePath, this.bikeServiceModel.ImagePath);
        } 

        [Test]
        public void ShouldNotCallSaveImgFromIOProvider_WhenModelStateIsValidAndFormFileCollectionIsEmpty()
        {
            // Arrange
            this.mockFormCollection.Setup(x => x.Count).Returns(0);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockIOprovider.Verify(x => x.SaveImage(this.id, this.wwRootPath, this.mockFormCollection.Object), Times.Never);
        }

        [Test]
        public void ShouldNotSetImgPathToBikeServiceModel_WhenModelStateIsValidAndFormFileCollectionIsEmpty()
        {
            // Arrange
            this.mockFormCollection.Setup(x => x.Count).Returns(0);

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            Assert.AreNotEqual(this.relativeImagePath, this.bikeServiceModel.ImagePath);
        }

        [Test]
        public void ShouldCallUpdateFromBikeService_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            this.controller.Edit(this.createBikeViewModel);

            // Assert
            this.mockBikeService.Verify(x => x.Update(this.bikeServiceModel), Times.Once);
        }

        [Test]
        public void ShouldRedirectResultRedirectictToIndex_WhenModelStateIsValid()
        {
            // Arrange

            // Act
            var result = this.controller.Edit(this.createBikeViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            StringAssert.Contains("Index", result.ActionName);
        }
    }
}
