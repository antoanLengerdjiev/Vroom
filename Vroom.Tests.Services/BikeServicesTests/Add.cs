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
    public class Add
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

            this.mockedMapper.Setup(x => x.Map<BikeServiceModel, Bike>(this.bikeServiceModel)).Returns(this.bike);

            this.bikeService = new BikeService(mockBikeRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParameterModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.bikeService.Add(null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectExceptionMessege_WhenParameterModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => this.bikeService.Add(null));
            StringAssert.Contains("Parameter model cannot be null or empty", exception.Message);
        }


        [Test]
        public void ShouldCallMapMethodFromMapper_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            var result = this.bikeService.Add(this.bikeServiceModel);
            // Assert
            this.mockedMapper.Verify(x => x.Map<BikeServiceModel, Bike>(this.bikeServiceModel), Times.Once);
        }

        [Test]
        public void ShouldCallAddMethodFromBikeRepository_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            var result = this.bikeService.Add(this.bikeServiceModel);
            // Assert
            this.mockBikeRepository.Verify(x => x.Add(this.bike), Times.Once);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromDbSaveChanges_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Add(this.bikeServiceModel);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void ShouldReturnInt_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            var result = this.bikeService.Add(this.bikeServiceModel);
            // Assert
            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void ShouldReturnBikeId_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            var result = this.bikeService.Add(this.bikeServiceModel);
            // Assert
            Assert.AreEqual(bike.Id,result);
        }
    }
}
