using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models;
using Vroom.Models;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.BikeServicesTests
{
    [TestFixture]
    public class Update
    {
        private Mock<IEfDbRepository<Bike>> mockBikeRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private BikeServiceModel bikeServiceModel;
        private Bike firstbike;
        private BikeService bikeService;

        [SetUp]
        public void Setup()
        {
            this.mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.bikeServiceModel = new BikeServiceModel() { Currency = "BGN", ImagePath="imgPath", Mileage = 1200, ModelId = 2, Price = 2000, SellerId = "Seller", Features = "ABS", Year = 2020};

            this.firstbike = new Bike() { Id = 1, Price = 10 };

            this.mockBikeRepository.Setup(x => x.GetById(this.bikeServiceModel.Id)).Returns(this.firstbike);

            this.bikeService = new BikeService(mockBikeRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParamIsNull()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => this.bikeService.Update(null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessege_WhenParamIsNull()
        {
            // Arrange

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => this.bikeService.Update(null));
            StringAssert.Contains("Parameter bikeServiceModel cannot be null or empty", exception.Message);
        }

        [Test]
        public void ShouldCallGetByIdMethodFromBikeRepository_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            this.mockBikeRepository.Verify(x => x.GetById(this.bikeServiceModel.Id), Times.Once);
        }

        [Test]
        public void ShouldSetBikePropertyCurrencyCorrectly_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            Assert.AreEqual(this.bikeServiceModel.Currency, this.firstbike.Currency);
        }

        [Test]
        public void ShouldSetBikePropertyImagePathCorrectly_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            Assert.AreEqual(this.bikeServiceModel.ImagePath, this.firstbike.ImagePath);
        }

        [Test]
        public void ShouldSetBikePropertyMileageCorrectly_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            Assert.AreEqual(this.bikeServiceModel.Mileage, this.firstbike.Mileage);
        }

        [Test]
        public void ShouldSetBikePropertyModelIdCorrectly_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            Assert.AreEqual(this.bikeServiceModel.ModelId, this.firstbike.ModelId);
        }

        [Test]
        public void ShouldSetBikePropertyPriceCorrectly_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            Assert.AreEqual(this.bikeServiceModel.Price, this.firstbike.Price);
        }

        [Test]
        public void ShouldSetBikePropertySellerIdCorrectly_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            Assert.AreEqual(this.bikeServiceModel.SellerId, this.firstbike.SellerId);
        }


        [Test]
        public void ShouldSetBikePropertyYearCorrectly_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            Assert.AreEqual(this.bikeServiceModel.Year, this.firstbike.Year);
        }

        [Test]
        public void ShouldSetBikePropertyFeaturesCorrectly_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            Assert.AreEqual(this.bikeServiceModel.Features, this.firstbike.Features);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromVroomDbSaveChanges_WhenParamIsNotNull()
        {
            // Arrange

            // Act
            this.bikeService.Update(this.bikeServiceModel);

            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
