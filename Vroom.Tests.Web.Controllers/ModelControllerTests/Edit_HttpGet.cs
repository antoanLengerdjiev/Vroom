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
    public class Edit_HttpGet
    {
        private string encryptedId;
        private int id;
        private int makeId;
        private string encryptedMakeId;
        private ModelServiceModel modelServiceModel;
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
            this.encryptedId = "encryptedId";
            this.id = 3;

            this.makeId = 5;
            this.encryptedMakeId = "5";

            this.modelServiceModel = new ModelServiceModel { Id = this.id, MakeId = this.makeId };
            this.collectionOfMakeServiceModels = new List<MakeServiceModel>() { new MakeServiceModel { Id = 3 } };
            this.collectionOfMakeViewModels = new List<MakeViewModel> { new MakeViewModel { Id = "3" } };
            this.collectionOfSelectListItems = new List<SelectListItem> { new SelectListItem { Value = "3", Text = "yoyo" } };

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.encryptedId)).Returns(this.id);
            this.mockEncryptionProvider.Setup(x => x.Encrypt(this.id)).Returns(this.encryptedId);
            this.mockEncryptionProvider.Setup(x => x.Encrypt(this.makeId)).Returns(this.encryptedMakeId);
            this.mockModelService.Setup(x => x.GetById(this.id)).Returns(this.modelServiceModel);
            this.mockMakeService.Setup(x => x.GetAll()).Returns(this.collectionOfMakeServiceModels);
            this.mockMapper.Setup(x => x.Map<IEnumerable<IMakeServiceModel>, IEnumerable<MakeViewModel>>(this.collectionOfMakeServiceModels)).Returns(this.collectionOfMakeViewModels);
            this.mockSelectItemFactory.Setup(x => x.GetSelectList(this.collectionOfMakeViewModels, this.encryptedMakeId)).Returns(this.collectionOfSelectListItems);

            this.controller = new ModelController(this.mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }

        [Test]
        public void ShouldCallDecryptIdMethodFromEncryptionProvider()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);
            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedId), Times.Once);
        }

        [Test]
        public void ShouldCallGetByIdMethodFromModelService()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);
            // Assert
            this.mockModelService.Verify(x => x.GetById(this.id), Times.Once);
        }

        [Test]
        public void ShouldReturnNotFoundResult_WhenThereIsNoSuchModelWithThatId()
        {
            // Arrange
            this.modelServiceModel = null;
            this.mockModelService.Setup(x => x.GetById(this.id)).Returns(this.modelServiceModel);

            // Act
            var result = this.controller.Edit(this.encryptedId) as NotFoundResult;

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void ShouldCallGetAllMethodFromMakeService_WhenThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);
            // Assert
            this.mockMakeService.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void ShouldCallMapMethodFromMapper_WhenThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);
            // Assert
            this.mockMapper.Verify(x => x.Map<IEnumerable<IMakeServiceModel>, IEnumerable<MakeViewModel>>(this.collectionOfMakeServiceModels), Times.Once);
        }

        [Test]
        public void ShouldCallEncryptMethodFromEncryptionProvider_WhenEncryptingMakeIdAndThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);
            // Assert
            this.mockEncryptionProvider.Verify(x => x.Encrypt(this.makeId), Times.Once);
        }

        [Test]
        public void ShouldCallEncryptMethodFromEncryptionProvider_WhenEncryptingIdAndThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);
            // Assert
            this.mockEncryptionProvider.Verify(x => x.Encrypt(this.id), Times.Once);
        }

        [Test]
        public void ShouldCallGetSelectListMethodFromelectItemFactory_WhenThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            this.controller.Edit(this.encryptedId);
            // Assert
            this.mockSelectItemFactory.Setup(x => x.GetSelectList(this.collectionOfMakeViewModels, this.encryptedMakeId));
        }

        [Test]
        public void ShouldReturnViewResult_WhenThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            var result = this.controller.Edit(this.encryptedId) as ViewResult;
            // Assert

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModel_WhenThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            var result = this.controller.Edit(this.encryptedId) as ViewResult;
            var model = result.Model as CreateViewModel;
            // Assert

            Assert.AreEqual(this.collectionOfSelectListItems, model.MakeViewModels);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModelName_WhenThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            var result = this.controller.Edit(this.encryptedId) as ViewResult;
            var model = result.Model as CreateViewModel;
            // Assert

            Assert.AreEqual(this.modelServiceModel.Name, model.Name);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModelMakeId_WhenThereIsSuchModelWithThatId()
        {
            // Arrange

            // Act
            var result = this.controller.Edit(this.encryptedId) as ViewResult;
            var model = result.Model as CreateViewModel;
            // Assert

            Assert.AreEqual(this.encryptedMakeId, model.MakeId);
        }
    }
}
