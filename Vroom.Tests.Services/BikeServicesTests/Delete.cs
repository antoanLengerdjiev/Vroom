using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.BikeServicesTests
{
    [TestFixture]
    public class Delete
    {
        private Mock<IEfDbRepository<Bike>> mockBikeRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private BikeServiceModel bikeServiceModel;
        private Bike bike;
        public BikeService bikeService;

        [SetUp]
        public void Setup()
        {
            this.mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.bikeServiceModel = new BikeServiceModel();
            this.bike = new Bike() { Id = 3 };

            this.mockBikeRepository.Setup(x => x.GetById(this.bike.Id)).Returns(this.bike);

            this.bikeService = new BikeService(mockBikeRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeException_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.bikeService.Delete(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.bikeService.Delete(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }


        [Test]
        public void ShouldCallGetByIdMethodFromBikeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.bikeService.Delete(this.bike.Id);
            // Assert
            this.mockBikeRepository.Verify(x => x.GetById(this.bike.Id), Times.Once);
        }


        [Test]
        public void ShouldSetIsDeletedBikePropertyToTrue_WhenParameterIdIsPositiveNumberAndThereIsNoSuchMakeWithThatId()
        {
            // Arrange

            // Act
            this.bikeService.Delete(1);
            // Assert
            Assert.IsFalse(bike.IsDeleted);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromDbSaveChanges_WhenParameterIdIsPositiveNumberAndThereIsNoSuchMakeWithThatId()
        {
            // Arrange

            // Act
            this.bikeService.Delete(1);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Test]
        public void ShouldSetIsDeletedBikePropertyToTrue_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.bikeService.Delete(this.bike.Id);
            // Assert
            Assert.IsTrue(bike.IsDeleted);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromDbSaveChanges_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.bikeService.Delete(this.bike.Id);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
