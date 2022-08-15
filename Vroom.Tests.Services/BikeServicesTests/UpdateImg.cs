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
    public class UpdateImg
    {
        private Mock<IEfDbRepository<Bike>> mockBikeRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private Bike firstbike;
        private BikeService bikeService;

        [SetUp]
        public void Setup()
        {
            this.mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.firstbike = new Bike() { Id = 3, Price = 10, ImagePath = "imgPath" };

            this.mockBikeRepository.Setup(x => x.GetById(3)).Returns(this.firstbike);

            this.bikeService = new BikeService(mockBikeRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeException_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.bikeService.UpdateImg(negativeId, "relativeImagePath"));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.bikeService.UpdateImg(negativeId,"relativeImagePath"));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }

        [TestCase("")]
        public void ShouldThrowArgumentException_WhenParameterRelativeImgPathIsNullOrEmpty(string relativeImgPath)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => this.bikeService.UpdateImg(3, relativeImgPath)); 
        }

        [TestCase(null)]
        public void ShouldThrowArgumentNullException_WhenParameterRelativeImgPathIsNullOrEmpty(string relativeImgPath)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.bikeService.UpdateImg(3, relativeImgPath));
        }

        [TestCase("")]
        public void ShouldThrowArgumentExceptionWithCorrectExceptionMessege_WhenParameterRelativeImgPathIsNullOrEmpty(string relativeImgPath)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => this.bikeService.UpdateImg(3, relativeImgPath));
            StringAssert.Contains("Parameter relativeImagePath cannot be null or empty", exception.Message);
        }

        [TestCase(null)]
        public void ShouldThrowArgumentNullExceptionWithCorrectExceptionMessege_WhenParameterRelativeImgPathIsNull(string relativeImgPath)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => this.bikeService.UpdateImg(3, relativeImgPath));
            StringAssert.Contains("Parameter relativeImagePath cannot be null or empty", exception.Message);
        }

        [Test]
        public void ShouldCallGetByIdMethodFromBikeRepository_WhenParamsAreValid()
        {
            // Arrange

            // Act
            this.bikeService.UpdateImg(3, "relativeImgPath");
            // Assert
            this.mockBikeRepository.Verify(x => x.GetById(3), Times.Once);
        }

        [Test]
        public void ShouldSetNewRelativeImageToImagePathPropertyOfDbBike_WhenParamsAreValid()
        {
            // Arrange
            var expectedResult = "relativeImgPath";
            // Act
            this.bikeService.UpdateImg(3, expectedResult);
            // Assert
            Assert.AreEqual(expectedResult, this.firstbike.ImagePath);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromVroomDbSaveChanges_WhenParamsAreValid()
        {
            // Arrange

            // Act
            this.bikeService.UpdateImg(3, "relativeImgPath");
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
