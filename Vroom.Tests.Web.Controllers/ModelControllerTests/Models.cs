using AutoMapper;
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

namespace Vroom.Tests.Web.Controllers.ModelControllerTests
{
    [TestFixture]
    public class Models
    {
        private int makeId;
        private string encryptedMakeId;
        private List<ModelServiceModel> collectionModelServiceModel;
        private List<ModelViewModel> collectionModelViewModel;
        private List<SelectListItem> collectionSelectListItems;
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IModelService> mockModelService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private Mock<ISelectedItemFactory> mockSelectItemFactory;
        private ModelController controller;


        [SetUp]
        public void Setup()
        {

            this.makeId = 5;
            this.encryptedMakeId = "5";

            this.collectionModelServiceModel = new List<ModelServiceModel> { new ModelServiceModel { Id = 3 } };
            this.collectionModelViewModel = new List<ModelViewModel> { new ModelViewModel { Id = "3" } };
            this.collectionSelectListItems = new List<SelectListItem> { new SelectListItem { Text = "yoyo", Value = encryptedMakeId } };

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockModelService = new Mock<IModelService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();
            this.mockSelectItemFactory = new Mock<ISelectedItemFactory>();

            this.mockEncryptionProvider.Setup(x => x.Decrypt(this.encryptedMakeId)).Returns(this.makeId);
            this.mockModelService.Setup(x => x.GetAll(this.makeId)).Returns(this.collectionModelServiceModel);
            this.mockMapper.Setup(x => x.Map<IEnumerable<ModelServiceModel>, IEnumerable<ModelViewModel>>(this.collectionModelServiceModel)).Returns(this.collectionModelViewModel);
            //this.mockSelectItemFactory.Setup(x => x.GetSelectList(this.collectionModelViewModel, this.encryptedMakeId)).Returns(this.collectionSelectListItems);

            this.controller = new ModelController(this.mockMapper.Object, this.mockModelService.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object, this.mockSelectItemFactory.Object);
        }

        [Test]
        public void ShouldCallDecryptMethodFromEncryptionProvider()
        {
            // Arrange

            // Act
            this.controller.Models(this.encryptedMakeId);

            // Assert
            this.mockEncryptionProvider.Verify(x => x.Decrypt(this.encryptedMakeId), Times.Once);
        }

        [Test]
        public void ShouldCallGetAllMethodFromModelService()
        {
            // Arrange

            // Act
            this.controller.Models(this.encryptedMakeId);

            // Assert
            this.mockModelService.Verify(x => x.GetAll(this.makeId), Times.Once);
        }


        [Test]
        public void ShouldCallMapMethodFromMapper()
        {
            // Arrange

            // Act
            this.controller.Models(this.encryptedMakeId);

            // Assert
            this.mockMapper.Verify(x => x.Map<IEnumerable<ModelServiceModel>, IEnumerable<ModelViewModel>>(this.collectionModelServiceModel), Times.Once);
        }

        [Test]
        public void ShouldReturnIEnumerableOfModelViewModel()
        {
            // Arrange

            // Act
            var result = this.controller.Models(this.encryptedMakeId);

            // Assert
            Assert.IsInstanceOf<IEnumerable<ModelViewModel>>( result);
        }

        [Test]
        public void ShouldReturnCorrectResult()
        {
            // Arrange

            // Act
            var result = this.controller.Models(this.encryptedMakeId);

            // Assert
            Assert.AreEqual(this.collectionModelViewModel, result);
        }
    }
}
