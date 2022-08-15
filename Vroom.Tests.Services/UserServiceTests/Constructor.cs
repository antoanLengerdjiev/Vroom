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
using Vroom.Data.Models;
using Vroom.Providers.Contracts;
using Vroom.Service.Data;

namespace Vroom.Tests.Services.UserServiceTests
{
    [TestFixture]
    public class Constructor
    {
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private Mock<IApplicationUserManager<ApplicationUser>> mockApplicationUserManager;
        private Mock<IEfDbRepository<ApplicationUser>> mockAppUserRepository;

        [SetUp]
        public void Setup()
        {
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();
            this.mockApplicationUserManager = new Mock<IApplicationUserManager<ApplicationUser>>();
            this.mockAppUserRepository = new Mock<IEfDbRepository<ApplicationUser>>();
        }
        [Test]
        public void ShouldThrowArgumentNullException_WhenApplicationUserRepositoryIsNull()
        {
            // Arrange
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UserService(null, mockApplicationUserManager.Object, mockSaveChanges.Object, mockedMapper.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenApplicationUserRepositoryIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.UserRepositoryNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new UserService(null, mockApplicationUserManager.Object, mockSaveChanges.Object, mockedMapper.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenApplicationUserManagerIsNull()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UserService(this.mockAppUserRepository.Object, null, this.mockSaveChanges.Object, this.mockedMapper.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenApplicationUserManagerIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.UserManagerNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new UserService(this.mockAppUserRepository.Object, null, this.mockSaveChanges.Object, this.mockedMapper.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenMapperIsNull()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UserService(mockAppUserRepository.Object, mockApplicationUserManager.Object, mockSaveChanges.Object, null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenMapperIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.MapperProviderNullExceptionMessege;

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new UserService(mockAppUserRepository.Object, mockApplicationUserManager.Object, mockSaveChanges.Object, null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenVroomDbSaveChangesIsNull()
        {
            // Arrange
            

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UserService(this.mockAppUserRepository.Object, this.mockApplicationUserManager.Object, null, this.mockedMapper.Object));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenVroomDbSaveChangesIsNull()
        {
            // Arrange
            var expectedExMessage = GlobalConstants.DbContextSaveChangesNullExceptionMessege;
           

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new UserService(this.mockAppUserRepository.Object, this.mockApplicationUserManager.Object, null, this.mockedMapper.Object));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }



        [Test]
        public void ShouldNotThrowExceptions_WhenValidArgumentsArePassed()
        {
            // Arrange
           

            // Act and Assert
            Assert.DoesNotThrow(() =>
                new UserService(this.mockAppUserRepository.Object, this.mockApplicationUserManager.Object, this.mockSaveChanges.Object, this.mockedMapper.Object));
        }
    }
}
