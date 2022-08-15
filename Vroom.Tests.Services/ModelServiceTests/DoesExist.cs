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
    public class DoesExist
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

            this.mockModelRepository.Setup(x => x.All()).Returns(new List<Model>() { this.model }.AsQueryable());

            this.mockedMapper.Setup(x => x.Map<ModelServiceModel, Model>(this.modelServiceModel)).Returns(this.model);

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
            Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.DoesExist(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.DoesExist(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }


        [Test]
        public void ShouldCallAllMethodFromBikeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.modelService.DoesExist(this.model.Id);
            // Assert
            this.mockModelRepository.Verify(x => x.All(), Times.Once);
        }


        [Test]
        public void ShouldReturnTypeOfBolean_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            var result = this.modelService.DoesExist(this.model.Id);
            // Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public void ShouldReturnTrue_WhenParameterIdIsPositiveNumberAndThereIsMakeWithThatIdInDb()
        {
            // Arrange

            // Act
            var result = this.modelService.DoesExist(this.model.Id);
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnFalse_WhenParameterIdIsPositiveNumberAndThereIsModelWithThatIdButIsDeletedPropertyIsTrue()
        {
            // Arrange
            this.model.IsDeleted = true;

            // Act
            var result = this.modelService.DoesExist(this.model.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestCase(50)]
        [TestCase(111150)]
        [TestCase(5)]
        public void ShouldReturnFalse_WhenParameterIdIsPositiveNumberAndThereIsNoMakeWithThatIdInDb(int id)
        {
            // Arrange

            // Act
            var result = this.modelService.DoesExist(id);
            // Assert
            Assert.IsFalse(result);
        }
    }
}
