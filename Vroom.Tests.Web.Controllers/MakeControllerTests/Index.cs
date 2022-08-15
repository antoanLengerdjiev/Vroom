using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Vroom.Controllers;
using Vroom.Models;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;
using Vroom.Service.Models.Contracts;

namespace Vroom.Tests.Web.Controllers.MakeControllerTests
{
    [TestFixture]
    public class Index
    {
        private List<MakeServiceModel> collectionMakeServiceModels;
        private List<MakeViewModel> collectionMakeViewModels;
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private MakeController controller;

        [SetUp]
        public void Setup()
        {

            this.collectionMakeServiceModels = new List<MakeServiceModel>() { new MakeServiceModel() { Id = 3 } };
            this.collectionMakeViewModels = new List<MakeViewModel>() { new MakeViewModel() { Id = "3"} };

            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();

            this.mockMakeService.Setup(x => x.GetAll()).Returns(this.collectionMakeServiceModels);
            this.mockMapper.Setup(x => x.Map<IEnumerable<IMakeServiceModel>, List<MakeViewModel>>(this.collectionMakeServiceModels)).Returns(this.collectionMakeViewModels);


            this.controller = new MakeController(this.mockMapper.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object);
        }

        [Test]
        public void ShouldCallGetAllMethodFromMakeService()
        {
            // Arrange

            // Act
            this.controller.Index();
            // Assert
            this.mockMakeService.Verify(x => x.GetAll());
        }

        [Test]
        public void ShouldCallMapMethodFromMapper()
        {
            // Arrange

            // Act
            this.controller.Index();
            // Assert
            this.mockMapper.Verify(x => x.Map<IEnumerable<IMakeServiceModel>, List<MakeViewModel>>(this.collectionMakeServiceModels), Times.Once);
        }

        [Test]
        public void ShouldReturnViewResult()
        {
            // Arrange

            // Act
           var result =  this.controller.Index() as ViewResult;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Test]
        public void ShouldReturnViewResultWithCorrectModel()
        {
            // Arrange

            // Act
            var result = this.controller.Index() as ViewResult;
            var model = result.Model as List<MakeViewModel>;
            // Assert
            Assert.IsInstanceOf<List<MakeViewModel>>(model);
        }


    }
}
