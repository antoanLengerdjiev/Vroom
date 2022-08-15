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
using Vroom.Data.Models;
using Vroom.Providers.Contracts;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.UserServiceTests
{
    [TestFixture]
    public class RemoveFromRoleAsync
    {
        private string id;
        private string role;
        private ApplicationUserServiceModel appUserServiceModel;
        private ApplicationUser appUser;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private Mock<IApplicationUserManager<ApplicationUser>> mockApplicationUserManager;
        private Mock<IEfDbRepository<ApplicationUser>> mockAppUserRepository;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            this.id = "Id";
            this.role = "role";
            this.appUserServiceModel = new ApplicationUserServiceModel() { Id = this.id };
            this.appUser = new ApplicationUser();

            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();
            this.mockApplicationUserManager = new Mock<IApplicationUserManager<ApplicationUser>>();
            this.mockAppUserRepository = new Mock<IEfDbRepository<ApplicationUser>>();

            this.mockAppUserRepository.Setup(x => x.GetById(this.id)).Returns(this.appUser);
            this.mockedMapper.Setup(x => x.Map<ApplicationUser, ApplicationUserServiceModel>(this.appUser)).Returns(this.appUserServiceModel);

            this.userService = new UserService(this.mockAppUserRepository.Object, this.mockApplicationUserManager.Object, this.mockSaveChanges.Object, this.mockedMapper.Object);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParamsApplicationUserServiceModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.RemoveFromRoleAsync(null, this.role));
        }


        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectErrorMessege_WhenParamsApplicationUserServiceModelIsNull()
        {
            // Arrange
            var expectedMsg = "Parameter user cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.RemoveFromRoleAsync(null, this.role));
            StringAssert.Contains(expectedMsg, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentException_WhenParamsNewRoleIsEmpty()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => this.userService.RemoveFromRoleAsync(this.appUserServiceModel, ""));
        }


        [Test]
        public void ShouldThrowArgumentExceptionWithCorrectErrorMessege_WhenParamsNewRoleIsEmpty()
        {
            // Arrange
            var expectedMsg = "Parameter currentRole cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.userService.RemoveFromRoleAsync(this.appUserServiceModel, ""));
            StringAssert.Contains(expectedMsg, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParamsNewRoleIsEmpty()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.RemoveFromRoleAsync(this.appUserServiceModel, null));
        }


        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectErrorMessege_WhenParamsNewRoleIsEmptyString()
        {
            // Arrange
            var expectedMsg = "Parameter currentRole cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.RemoveFromRoleAsync(this.appUserServiceModel, null));
            StringAssert.Contains(expectedMsg, exception.Message);
        }

        [Test]
        public async Task ShouldCallGetByIdMethodFromUserRepository_WhenParamIdIsNotNullOrEmpty()
        {
            // Arrange

            // Act
            await this.userService.RemoveFromRoleAsync(this.appUserServiceModel, this.role);

            // Assert
            this.mockAppUserRepository.Verify(x => x.GetById(this.id), Times.Once);
        }


        [Test]
        public void ShouldThrowArgumentNullException_WhenThereIsNoSuchDbUser()
        {
            // Arrange
            this.appUserServiceModel.Id = "123";
            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.RemoveFromRoleAsync(this.appUserServiceModel, this.role));
        }


        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectErrorMessege_WhenThereIsNoSuchDbUser()
        {
            // Arrange
            this.appUserServiceModel.Id = "123";
            var expectedMsg = "there is no such user";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.RemoveFromRoleAsync(this.appUserServiceModel, this.role));
            StringAssert.Contains(expectedMsg, exception.Message);
        }

        [Test]
        public async Task ShouldCallRemoveFromRoleAsyncMethodFromUserManager_WhenParamIdIsNotNullOrEmpty()
        {
            // Arrange

            // Act
            await this.userService.RemoveFromRoleAsync(this.appUserServiceModel, this.role);

            // Assert
            this.mockApplicationUserManager.Verify(x => x.RemoveFromRoleAsync(this.appUser, this.role), Times.Once);
        }
    }
}
