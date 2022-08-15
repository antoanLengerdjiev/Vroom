using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Models;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.ModelServiceTests
{
    [TestFixture]
    public class GetById
    {
        private Mock<IEfDbRepository<Model>> mockModelRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private ModelServiceModel modelServiceModel;
        private Model model;
        public ModelService modelService;

        [SetUp]
        public void Setup()
        {
            this.mockModelRepository = new Mock<IEfDbRepository<Model>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.modelServiceModel = new ModelServiceModel();
            this.model = new Model() { Id = 3 };

            this.mockModelRepository.Setup(x => x.GetById(this.model.Id)).Returns(this.model);

            this.mockedMapper.Setup(x => x.Map<Model, ModelServiceModel>(this.model)).Returns(this.modelServiceModel);

            this.modelService = new ModelService(mockModelRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeException_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.GetById(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.GetById(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }


        [Test]
        public void ShouldCallGetByIdMethodFromModelRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.modelService.GetById(this.model.Id);
            // Assert
            this.mockModelRepository.Verify(x => x.GetById(this.model.Id), Times.Once);
        }

        [Test]
        public void ShouldCallMapMethodFromMapper_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.modelService.GetById(this.model.Id);
            // Assert
            this.mockedMapper.Verify(x => x.Map<Model, ModelServiceModel>(this.model), Times.Once);
        }


        [Test]
        public void ShouldReturnTypeOfModelServiceModel_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            var result = this.modelService.GetById(this.model.Id);
            // Assert
            Assert.IsInstanceOf<ModelServiceModel>(result);
        }

        [Test]
        public void ShouldReturnNull_WhenParameterIdIsPositiveNumberAndThereIsNotModelWithThatIdInDb()
        {
            // Arrange

            // Act
            var result = this.modelService.GetById(5);
            // Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void ShouldReturnCorrectModelServiceModel_WhenParameterIdIsPositiveNumberAndThereIsModelWithThatIdInDb()
        {
            // Arrange

            // Act
            var result = this.modelService.GetById(this.model.Id);
            // Assert
            Assert.AreEqual(this.modelServiceModel, result);
        }
    }
}
