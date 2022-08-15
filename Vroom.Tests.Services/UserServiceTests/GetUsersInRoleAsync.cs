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
    public class GetUsersInRoleAsync
    {
        private ApplicationUserServiceModel appUserServiceModel;
        private ApplicationUser firstAdminUser;
        private ApplicationUser secondAdminUser;
        private ApplicationUser thirdAdminUser;
        private ApplicationUser foruthAdminUser;
        private ApplicationUser fifthAdminUser;
        private ApplicationUser firstExeUser;
        private ApplicationUser secondExeUser;
        private ApplicationUser thirdExeUser;
        private ApplicationUser foruthExeUser;
        private ApplicationUser fifthExeUser;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private Mock<IApplicationUserManager<ApplicationUser>> mockApplicationUserManager;
        private Mock<IEfDbRepository<ApplicationUser>> mockAppUserRepository;
        private List<ApplicationUser> collectionOfAdmins;
        private List<ApplicationUser> collectionOfExecutives;
        private List<ApplicationUserServiceModel> collectionOfAppUserServiceModel;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            this.appUserServiceModel = new ApplicationUserServiceModel();

            this.firstAdminUser = new ApplicationUser() { UserName = "Lenix" };
            this.secondAdminUser = new ApplicationUser() { UserName = "enix" };
            this.thirdAdminUser = new ApplicationUser() { UserName = "Gringe" };
            this.foruthAdminUser = new ApplicationUser() { UserName = "Lenix" };
            this.fifthAdminUser = new ApplicationUser() { UserName = "enix" };

            this.firstExeUser = new ApplicationUser() { UserName = "Lenix" };
            this.secondExeUser = new ApplicationUser() { UserName = "enix" };
            this.thirdExeUser = new ApplicationUser() { UserName = "Gringe" };
            this.foruthExeUser = new ApplicationUser() { UserName = "Lenix" };
            this.fifthExeUser = new ApplicationUser() { UserName = "enix" };

            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();
            this.mockApplicationUserManager = new Mock<IApplicationUserManager<ApplicationUser>>();
            this.mockAppUserRepository = new Mock<IEfDbRepository<ApplicationUser>>();

            this.collectionOfAdmins = new List<ApplicationUser> { this.firstAdminUser, this.secondAdminUser, this.thirdAdminUser, this.foruthAdminUser, this.fifthAdminUser };
            this.collectionOfExecutives = new List<ApplicationUser> { this.firstExeUser, this.secondExeUser, this.thirdExeUser, this.foruthExeUser, this.fifthExeUser };
            this.collectionOfAppUserServiceModel = new List<ApplicationUserServiceModel> { this.appUserServiceModel };

            this.mockApplicationUserManager.Setup(x => x.GetUsersInRoleAsync("Admin")).ReturnsAsync(this.collectionOfAdmins);
            this.mockApplicationUserManager.Setup(x => x.GetUsersInRoleAsync("Executive")).ReturnsAsync(this.collectionOfExecutives);
            this.mockedMapper.Setup(x => x.Map<IList<ApplicationUser>, IList<ApplicationUserServiceModel>>(It.IsAny<List<ApplicationUser>>())).Returns(this.collectionOfAppUserServiceModel);


            this.userService = new UserService(this.mockAppUserRepository.Object, this.mockApplicationUserManager.Object, this.mockSaveChanges.Object, this.mockedMapper.Object);
        }
        [Test]
        public void ShouldThrowArgumentException_WhenParamIdIsEmptyString()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => this.userService.GetUsersInRoleAsync(""));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWithCorrectMessege_WhenParamIdIsEmptyString()
        {
            // Arrange
            var expectedResult = "Parameter admin cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.userService.GetUsersInRoleAsync(""));
            StringAssert.Contains(expectedResult, exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParamIdIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.GetUsersInRoleAsync(null));
        }

        [Test]
        public void ShouldThrowArgumenNulltExceptionWithCorrectMessege_WhenParamIdIsNull()
        {
            // Arrange
            var expectedResult = "Parameter admin cannot be null or empty";
            // Act

            // Assert
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => this.userService.GetUsersInRoleAsync(null));
            StringAssert.Contains(expectedResult, exception.Message);
        }

        [TestCase("Admin")]
        [TestCase("Executive")]
        public async Task ShouldCallGetUsersInRoleAsyncMethodFromUserManager_WhenParamIsValid(string role)
        {
            // Arrange

            // Act
            await this.userService.GetUsersInRoleAsync(role);

            // Assert
            this.mockApplicationUserManager.Verify(x => x.GetUsersInRoleAsync(role), Times.Once);
        }

        [TestCase("Admin")]
        [TestCase("Executive")]
        public async Task ShouldCallMapMethodFromMapper_WhenParamIsValid(string role)
        {
            // Arrange

            // Act
            await this.userService.GetUsersInRoleAsync(role);

            // Assert
            this.mockedMapper.Verify(x => x.Map<IList<ApplicationUser>, IList<ApplicationUserServiceModel>>(It.IsAny<List<ApplicationUser>>()), Times.Once);
        }

        [TestCase("Admin")]
        [TestCase("Executive")]
        public async Task ShouldReturnTypeOfListOfApplicationUserServiceModel_WhenParamIsValid(string role)
        {
            // Arrange

            // Act
            var result = await this.userService.GetUsersInRoleAsync(role);

            // Assert
            Assert.IsInstanceOf<List<ApplicationUserServiceModel>>(result);
        }

        [TestCase("Admin")]
        [TestCase("Executive")]
        public async Task ShouldReturnCorrectResult_WhenParamIsValid(string role)
        {
            // Arrange

            // Act
            var result = await this.userService.GetUsersInRoleAsync(role);

            // Assert
            Assert.AreEqual(this.collectionOfAppUserServiceModel,result);
        }
    }
}
