using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Models;
using Vroom.Service.Data;

namespace Vroom.Tests.Services.MakeServiceTests
{
    [TestFixture]
    public class Constructor
    {

        [Test]
        public void ShouldThrowArgumentNullException_WhenMakeRepositoryIsNull()
        {
            // Arrange
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MakeService(null, mockedMapper.Object, mockSaveChanges.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenMakeRepositoryIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MakeRepositoryNullExceptionMessege;
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new MakeService(null, mockedMapper.Object, mockSaveChanges.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenMapperIsNull()
        {
            // Arrange
            var mockMakeRepository = new Mock<IEfDbRepository<Make>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MakeService(mockMakeRepository.Object, null, mockSaveChanges.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenMapperIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MapperProviderNullExceptionMessege;
            var mockMakeRepository = new Mock<IEfDbRepository<Make>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new MakeService(mockMakeRepository.Object, null, mockSaveChanges.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenVroomDbSaveChangesIsNull()
        {
            // Arrange
            var mockMakeRepository = new Mock<IEfDbRepository<Make>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new MakeService(mockMakeRepository.Object, mockedMapper.Object, null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenVroomDbSaveChangesIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.DbContextSaveChangesNullExceptionMessege;
            var mockMakeRepository = new Mock<IEfDbRepository<Make>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new MakeService(mockMakeRepository.Object, mockedMapper.Object, null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }



        [Test]
        public void ShouldNotThrow_WhenValidArgumentsArePassed()
        {
            // Arrange
            var mockMakeRepository = new Mock<IEfDbRepository<Make>>();
            var mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            var mockedMapper = new Mock<IMapper>();

            // Act and Assert
            Assert.DoesNotThrow(() =>
                new MakeService(mockMakeRepository.Object, mockedMapper.Object, mockSaveChanges.Object));
        }
    }
}
