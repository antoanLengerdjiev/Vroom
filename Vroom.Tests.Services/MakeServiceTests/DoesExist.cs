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
using Vroom.Models;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.MakeServiceTests
{
    [TestFixture]
    public class DoesExist
    {
        private Mock<IEfDbRepository<Make>> mockMakeRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private MakeServiceModel makeServiceModel;
        private Make make;
        public MakeService makeService;

        [SetUp]
        public void Setup()
        {
            this.mockMakeRepository = new Mock<IEfDbRepository<Make>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.makeServiceModel = new MakeServiceModel();
            this.make = new Make() { Id = 3 };

            this.mockMakeRepository.Setup(x => x.All()).Returns(new List<Make>() { this.make }.AsQueryable());

            this.mockedMapper.Setup(x => x.Map<MakeServiceModel, Make>(this.makeServiceModel)).Returns(this.make);

            this.makeService = new MakeService(mockMakeRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeException_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.makeService.DoesExist(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.makeService.DoesExist(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }


        [Test]
        public void ShouldCallAllMethodFromBikeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.makeService.DoesExist(this.make.Id);
            // Assert
            this.mockMakeRepository.Verify(x => x.All(), Times.Once);
        }


        [Test]
        public void ShouldReturnTypeOfBolean_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            var result = this.makeService.DoesExist(this.make.Id);
            // Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public void ShouldReturnTrue_WhenParameterIdIsPositiveNumberAndThereIsMakeWithThatIdInDb()
        {
            // Arrange

            // Act
            var result = this.makeService.DoesExist(this.make.Id);
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldReturnFalse_WhenParameterIdIsPositiveNumberAndThereIsMakeWithThatIdButIsDeletedPropertyIsTrue()
        {
            // Arrange
            this.make.IsDeleted = true;

            // Act
            var result = this.makeService.DoesExist(this.make.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestCase(50)]
        [TestCase(111150)]
        [TestCase(5)]
        public void ShouldReturnFalse_WhenParameterIdIsPositiveNumberAndThereIsNoMakeWithThatIdInDb(int id)
        {
            // Arrange

            // Act
            var result = this.makeService.DoesExist(id);
            // Assert
            Assert.IsFalse(result);
        }
    }
}
