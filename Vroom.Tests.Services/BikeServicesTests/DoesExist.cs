using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.BikeServicesTests
{
    [TestFixture]
    public class DoesExist 
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

            this.mockBikeRepository.Setup(x => x.All()).Returns(new List<Bike>() { this.bike }.AsQueryable());

            this.mockedMapper.Setup(x => x.Map<BikeServiceModel, Bike>(this.bikeServiceModel)).Returns(this.bike);

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
            Assert.Throws<ArgumentOutOfRangeException>(() => this.bikeService.DoesExist(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.bikeService.DoesExist(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }


        [Test]
        public void ShouldCallAllMethodFromBikeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.bikeService.DoesExist(this.bike.Id);
            // Assert
            this.mockBikeRepository.Verify(x => x.All(), Times.Once);
        }


        [Test]
        public void ShouldReturnTypeOfBolean_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            var result = this.bikeService.DoesExist(this.bike.Id);
            // Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public void ShouldReturnTrueWhenThereIsBikeWithThatIdInDb_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            var result = this.bikeService.DoesExist(this.bike.Id);
            // Assert
            Assert.IsTrue(result);
        }

        [TestCase(50)]
        [TestCase(111150)]
        [TestCase(5)]
        public void ShouldReturnFalseWhenThereIsNoBikeWithThatIdInDb_WhenParameterIdIsPositiveNumber(int id)
        {
            // Arrange

            // Act
            var result = this.bikeService.DoesExist(id);
            // Assert
            Assert.IsFalse(result);
        }
    }
}
