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
    public class UpdateName
    {
        private string newName;
        private Mock<IEfDbRepository<Make>> mockMakeRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private MakeServiceModel makeServiceModel;
        private Make make;
        public MakeService makeService;

        [SetUp]
        public void Setup()
        {
            this.newName = "newName";
            this.mockMakeRepository = new Mock<IEfDbRepository<Make>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.makeServiceModel = new MakeServiceModel();
            this.make = new Make() { Id = 3, Name= "OldName" };

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
            Assert.Throws<ArgumentOutOfRangeException>(() => this.makeService.UpdateName(negativeId, this.newName));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.makeService.UpdateName(negativeId, this.newName));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParameterIdIsPositiveNumberButNameParamIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.makeService.UpdateName(this.make.Id, null));

        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectExceptionMessege_WhenParameterIdIsPositiveNumberButNameParamIsNull()
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => this.makeService.UpdateName(this.make.Id, null));
            StringAssert.Contains("Parameter name cannot be null or empty", exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentException_WhenParameterIdIsPositiveNumberButNameParamIsEmptyString()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => this.makeService.UpdateName(this.make.Id, ""));

        }

        [Test]
        public void ShouldThrowArgumentExceptionWithCorrectExceptionMessege_WhenParameterIdIsPositiveNumberButNameParamIsEmptyString()
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => this.makeService.UpdateName(this.make.Id, ""));
            StringAssert.Contains("Parameter name cannot be null or empty", exception.Message);
        }


        [Test]
        public void ShouldCallGetByIdMethodFromMakeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.makeService.UpdateName(this.make.Id, this.newName);
            // Assert
            this.mockMakeRepository.Verify(x => x.GetById(this.make.Id), Times.Once);
        }

        [Test]
        public void ShouldSetNamePropertyToNewName_WhenParameterIdIsPositiveNumberAndNameParamIsNotNullOrEmptyString()
        {
            // Arrange

            // Act
            this.makeService.UpdateName(this.make.Id, this.newName);
            // Assert
            Assert.AreEqual(this.newName, this.make.Name);
        }


        [TestCase(23)]
        [TestCase(100)]
        [TestCase(2233)]
        public void ShouldNotCallSaveChangesMethodFromVroomDbSaveChange_WhenParameterIdIsPositiveNumberAndNameParamIsNotNullOrEmptyStringAndThereIsNoSuchMakeWithThatId(int badId)
        {
            // Arrange

            // Act
            this.makeService.UpdateName(badId, this.newName);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromVroomDbSaveChange_WhenParameterIdIsPositiveNumberAndNameParamIsNotNullOrEmptyString()
        {
            // Arrange

            // Act
            this.makeService.UpdateName(this.make.Id, this.newName);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }

    }
}
