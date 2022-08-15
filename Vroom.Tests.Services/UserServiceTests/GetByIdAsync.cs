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
    public class GetByIdAsync
    {
        private string id;
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
            this.appUserServiceModel = new ApplicationUserServiceModel();
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
        public void ShouldThrowArgumentException_WhenParamIdIsEmptyString()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => this.userService.GetByIdAsync(""));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWithCorrectMessege_WhenParamIdIsEmptyString()
        {
            // Arrange
            var expectedResult = "Parameter id cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.userService.GetByIdAsync(""));
            StringAssert.Contains(expectedResult, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParamIdIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.GetByIdAsync(null));
        }

        [Test]
        public void ShouldThrowArgumenNulltExceptionWithCorrectMessege_WhenParamIdIsNull()
        {
            // Arrange
            var expectedResult = "Parameter id cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.GetByIdAsync(null));
            StringAssert.Contains(expectedResult, exception.Message);
        }

        [Test]
        public async Task ShouldCallGetByIdMethodFromUserRepository_WhenParamIdIsNotNullOrEmpty()
        {
            // Arrange

            // Act
            await this.userService.GetByIdAsync(this.id);

            // Assert
            this.mockAppUserRepository.Verify(x => x.GetById(this.id), Times.Once);
        }

        [Test]
        public async Task ShouldCallMapMethodFromMapper_WhenParamIdIsNotNullOrEmpty()
        {
            // Arrange

            // Act
            await this.userService.GetByIdAsync(this.id);

            // Assert
            this.mockedMapper.Verify(x => x.Map<ApplicationUser,ApplicationUserServiceModel>(this.appUser), Times.Once);
        }

        [Test]
        public async Task ShouldReturnInstanceOfApplicationUserServiceModel_WhenParamIdIsNotNullOrEmpty()
        {
            // Arrange

            // Act
            var result = await this.userService.GetByIdAsync(this.id);

            // Assert
            Assert.IsInstanceOf<ApplicationUserServiceModel>(result);
        }

        [Test]
        public async Task ShouldReturnCorrectResult_WhenParamIdIsNotNullOrEmpty()
        {
            // Arrange

            // Act
            var result = await this.userService.GetByIdAsync(this.id);

            // Assert
            Assert.AreEqual(this.appUserServiceModel,result);
        }
    }
}
