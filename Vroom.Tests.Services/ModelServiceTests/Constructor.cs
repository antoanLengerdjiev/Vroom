using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using Vroom.Common;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Models;
using Vroom.Service.Data;

namespace Vroom.Tests.Services.ModelServiceTests
{
    [TestFixture]
    public class Constructor
    {

        [Test]
        public void ShouldThrowArgumentNullException_WhenModelRepositoryIsNull()
        {
            // Arrange
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ModelService(null, mockedMapper.Object, mockSaveChanges.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenModelRepositoryIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.ModelRepositoryNullExceptionMessege;
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ModelService(null, mockedMapper.Object, mockSaveChanges.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenMapperIsNull()
        {
            // Arrange
            var mockMakeRepository = new Mock<IEfDbRepository<Model>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ModelService(mockMakeRepository.Object, null, mockSaveChanges.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenMapperIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MapperProviderNullExceptionMessege;
            var mockModelRepository = new Mock<IEfDbRepository<Model>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ModelService(mockModelRepository.Object, null, mockSaveChanges.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenVroomDbSaveChangesIsNull()
        {
            // Arrange
            var mockModelRepository = new Mock<IEfDbRepository<Model>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ModelService(mockModelRepository.Object, mockedMapper.Object, null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenVroomDbSaveChangesIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.DbContextSaveChangesNullExceptionMessege;
            var mockModelRepository = new Mock<IEfDbRepository<Model>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ModelService(mockModelRepository.Object, mockedMapper.Object, null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }



        [Test]
        public void ShouldNotThrowExceptions_WhenValidArgumentsArePassed()
        {
            // Arrange
            var mockModelRepository = new Mock<IEfDbRepository<Model>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            Assert.DoesNotThrow(() =>
                new ModelService(mockModelRepository.Object, mockedMapper.Object, mockSaveChanges.Object));
        }
    }
}
