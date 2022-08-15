using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using Vroom.Common;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models;
using Vroom.Service.Data;

namespace Vroom.Tests.Services.BikeServicesTests
{
    [TestFixture]
    public class Constructor
    {

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullBikeRepositoryIsPassed()
        {
            // Arrange
            var mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeService(null, mockedMapper.Object, mockSaveChanges.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullBikeRepositoryIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.BikeRepositoryNullExceptionMessege;
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeService(null, mockedMapper.Object, mockSaveChanges.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullMapperIsPassed()
        {
            // Arrange
            var mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeService(mockBikeRepository.Object, null, mockSaveChanges.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullMapperIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MapperProviderNullExceptionMessege;
            var mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeService(mockBikeRepository.Object, null, mockSaveChanges.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenNullVroomDbSaveChangesIsPassed()
        {
            // Arrange
            var mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new BikeService(mockBikeRepository.Object, mockedMapper.Object, null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullVroomDbSaveChangesIsPassed()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.DbContextSaveChangesNullExceptionMessege;
            var mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new BikeService(mockBikeRepository.Object, mockedMapper.Object, null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

       

        [Test]
        public void ShouldNotThrow_WhenValidArgumentsArePassed()
        {
            // Arrange
            var mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            Assert.DoesNotThrow(() =>
                new BikeService(mockBikeRepository.Object, mockedMapper.Object, mockSaveChanges.Object));
        }
    }
}
