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
    public class Delete
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
            Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.Delete(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.Delete(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }


        [Test]
        public void ShouldCallGetByIdMethodFromBikeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.modelService.Delete(this.model.Id);
            // Assert
            this.mockModelRepository.Verify(x => x.GetById(this.model.Id), Times.Once);
        }

        [Test]
        public void ShouldNotSetIsDeletedBikePropertyToTrue_WhenParameterIdIsPositiveNumberAndThereIsNoSuchMakeWithThatId()
        {
            // Arrange

            // Act
            this.modelService.Delete(1);
            // Assert
            Assert.IsFalse(model.IsDeleted);
        }

        [Test]
        public void ShouldNotCallSaveChangesMethodFromDbSaveChanges_WhenParameterIdIsPositiveNumberAndThereIsNoSuchMakeWithThatId()
        {
            // Arrange

            // Act
            this.modelService.Delete(1);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Never);
        }



        [Test]
        public void ShouldSetIsDeletedBikePropertyToTrue_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.modelService.Delete(this.model.Id);
            // Assert
            Assert.IsTrue(model.IsDeleted);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromDbSaveChanges_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.modelService.Delete(this.model.Id);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
