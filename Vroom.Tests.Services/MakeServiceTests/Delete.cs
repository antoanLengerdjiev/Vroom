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
    public class Delete
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
            Assert.Throws<ArgumentOutOfRangeException>(() => this.makeService.Delete(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.makeService.Delete(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }


        [Test]
        public void ShouldCallGetByIdMethodFromBikeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.makeService.Delete(this.make.Id);
            // Assert
            this.mockMakeRepository.Verify(x => x.GetById(this.make.Id), Times.Once);
        }

        [Test]
        public void ShouldNotSetIsDeletedBikePropertyToTrue_WhenParameterIdIsPositiveNumberAndThereIsNoSuchMakeWithThatId()
        {
            // Arrange

            // Act
            this.makeService.Delete(1);
            // Assert
            Assert.IsFalse(make.IsDeleted);
        }

        [Test]
        public void ShouldNotCallSaveChangesMethodFromDbSaveChanges_WhenParameterIdIsPositiveNumberAndThereIsNoSuchMakeWithThatId()
        {
            // Arrange

            // Act
            this.makeService.Delete(1);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Never);
        }



        [Test]
        public void ShouldSetIsDeletedBikePropertyToTrue_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.makeService.Delete(this.make.Id);
            // Assert
            Assert.IsTrue(make.IsDeleted);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromDbSaveChanges_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.makeService.Delete(this.make.Id);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
