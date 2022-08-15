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
    public class GetById
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

            this.mockMakeRepository.Setup(x => x.GetById(this.make.Id)).Returns(this.make);

            this.mockedMapper.Setup(x => x.Map<Make, MakeServiceModel>(this.make)).Returns(this.makeServiceModel);

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
            Assert.Throws<ArgumentOutOfRangeException>(() => this.makeService.GetById(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.makeService.GetById(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }


        [Test]
        public void ShouldCallGetByIdMethodFromMakeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.makeService.GetById(this.make.Id);
            // Assert
            this.mockMakeRepository.Verify(x => x.GetById(this.make.Id), Times.Once);
        }

        [Test]
        public void ShouldCallMapMethodFromMapper_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.makeService.GetById(this.make.Id);
            // Assert
            this.mockedMapper.Verify(x => x.Map<Make, MakeServiceModel>(this.make), Times.Once);
        }


        [Test]
        public void ShouldReturnTypeOfMakeServiceModel_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            var result = this.makeService.GetById(this.make.Id);
            // Assert
            Assert.IsInstanceOf<MakeServiceModel>(result);
        }

        [Test]
        public void ShouldReturnNull_WhenParameterIdIsPositiveNumberAndThereIsNotMakeWithThatIdInDb()
        {
            // Arrange

            // Act
            var result = this.makeService.GetById(5);
            // Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void ShouldReturnCorrectMakeServiceModel_WhenParameterIdIsPositiveNumberAndThereIsMakeWithThatIdInDb()
        {
            // Arrange

            // Act
            var result = this.makeService.GetById(this.make.Id);
            // Assert
            Assert.AreEqual(this.makeServiceModel, result);
        }
    }
}
