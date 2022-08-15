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
    public class AddToRoleAsync
    {
        private string newRole;
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
            this.newRole = "newRole";
            this.appUserServiceModel = new ApplicationUserServiceModel() { Id = "UserId"};
            this.appUser = new ApplicationUser();

            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();
            this.mockApplicationUserManager = new Mock<IApplicationUserManager<ApplicationUser>>();
            this.mockAppUserRepository = new Mock<IEfDbRepository<ApplicationUser>>();

            this.mockAppUserRepository.Setup(x => x.GetById(this.appUserServiceModel.Id)).Returns(this.appUser);
            

            this.userService = new UserService(this.mockAppUserRepository.Object, this.mockApplicationUserManager.Object, this.mockSaveChanges.Object, this.mockedMapper.Object);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParamsApplicationUserServiceModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.AddToRoleAsync(null, this.newRole));
        }


        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectErrorMessege_WhenParamsApplicationUserServiceModelIsNull()
        {
            // Arrange
            var expectedMsg = "Parameter user cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.AddToRoleAsync(null, this.newRole));
            StringAssert.Contains(expectedMsg, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentException_WhenParamsNewRoleIsEmpty()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => this.userService.AddToRoleAsync(this.appUserServiceModel, ""));
        }


        [Test]
        public void ShouldThrowArgumentExceptionWithCorrectErrorMessege_WhenParamsNewRoleIsEmpty()
        {
            // Arrange
            var expectedMsg = "Parameter newRole cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.userService.AddToRoleAsync(this.appUserServiceModel, ""));
            StringAssert.Contains(expectedMsg, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParamsNewRoleIsEmpty()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.AddToRoleAsync(this.appUserServiceModel, null));
        }


        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectErrorMessege_WhenParamsNewRoleIsEmptyString()
        {
            // Arrange
            var expectedMsg = "Parameter newRole cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.AddToRoleAsync(this.appUserServiceModel, null));
            StringAssert.Contains(expectedMsg, exception.Message);
        }

        [Test]
        public async Task ShouldCallGetByIdMethodFromUserRepository_WhenParamsAreValid()
        {
            // Arrange

            // Act
            await this.userService.AddToRoleAsync(this.appUserServiceModel, this.newRole);

            // Assert
            this.mockAppUserRepository.Verify(x => x.GetById(this.appUserServiceModel.Id), Times.Once);
        }

        [Test]
        public async Task ShouldCallAddToRoleAsyncMethodFromUserManager_WhenParamsAreValid()
        {
            // Arrange

            // Act
            await this.userService.AddToRoleAsync(this.appUserServiceModel, this.newRole);

            // Assert
            this.mockApplicationUserManager.Verify(x => x.AddToRoleAsync(this.appUser, this.newRole), Times.Once);
        }
    }
}
