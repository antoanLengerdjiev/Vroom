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

namespace Vroom.Tests.Services.ModelServiceTests
{
    [TestFixture]
    public class Update
    {
        private int makeId;
        private string newName;
        private Mock<IEfDbRepository<Model>> mockModelRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private ModelServiceModel modelServiceModel;
        private Model model;
        public ModelService modelService;

        [SetUp]
        public void Setup()
        {
            this.makeId = 5;
            this.newName = "newName";
            this.mockModelRepository = new Mock<IEfDbRepository<Model>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();
        
            this.modelServiceModel = new ModelServiceModel();
            this.model = new Model() { Id = 3, Name = "OldName" };

            this.mockModelRepository.Setup(x => x.GetById(this.model.Id)).Returns(this.model);

            this.mockedMapper.Setup(x => x.Map<Model, ModelServiceModel>(this.model)).Returns(this.modelServiceModel);

            this.modelService = new ModelService(mockModelRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeException_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.Update(negativeId, this.newName, this.makeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.Update(negativeId, this.newName, this.makeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParameterIdIsPositiveNumberButNameParamIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.modelService.Update(this.model.Id, null, this.makeId));

        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectExceptionMessege_WhenParameterIdIsPositiveNumberButNameParamIsNull()
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => this.modelService.Update(this.model.Id, null, this.makeId));
            StringAssert.Contains("Parameter name cannot be null or empty", exception.Message);
        }

        [Test]
        public void ShouldThrowArgumentException_WhenParameterIdIsPositiveNumberButNameParamIsEmptyString()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => this.modelService.Update(this.model.Id, "", this.makeId));

        }

        [Test]
        public void ShouldThrowArgumentExceptionWithCorrectExceptionMessege_WhenParameterIdIsPositiveNumberButNameParamIsEmptyString()
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => this.modelService.Update(this.model.Id, "", this.makeId));
            StringAssert.Contains("Parameter name cannot be null or empty", exception.Message);
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeException_WhenParameterMakeIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.Update(this.model.Id, this.newName, negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterMakeIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.Update(this.model.Id, this.newName, negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }

        [Test]
        public void ShouldCallGetByIdMethodFromMakeRepository_WhenParameterIdIsPositiveNumber()
        {
            // Arrange

            // Act
            this.modelService.Update(this.model.Id, this.newName, this.makeId);
            // Assert
            this.mockModelRepository.Verify(x => x.GetById(this.model.Id), Times.Once);
        }

        [Test]
        public void ShouldSetNamePropertyToNewName_WhenParameterIdIsPositiveNumberAndNameParamIsNotNullOrEmptyString()
        {
            // Arrange

            // Act
            this.modelService.Update(this.model.Id, this.newName, this.makeId);
            // Assert
            Assert.AreEqual(this.newName, this.model.Name);
        }

        [Test]
        public void ShouldSetMakeIdPropertyCorrectly_WhenParameterIdIsPositiveNumberAndNameParamIsNotNullOrEmptyString()
        {
            // Arrange

            // Act
            this.modelService.Update(this.model.Id, this.newName, this.makeId);
            // Assert
            Assert.AreEqual(this.makeId, this.model.MakeId);
        }


        [TestCase(23)]
        [TestCase(100)]
        [TestCase(2233)]
        public void ShouldNotCallSaveChangesMethodFromVroomDbSaveChange_WhenParameterIdIsPositiveNumberAndNameParamIsNotNullOrEmptyStringAndThereIsNoSuchMakeWithThatId(int badId)
        {
            // Arrange

            // Act
            this.modelService.Update(badId, this.newName, this.makeId);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromVroomDbSaveChange_WhenParameterIdIsPositiveNumberAndNameParamIsNotNullOrEmptyString()
        {
            // Arrange

            // Act
            this.modelService.Update(this.model.Id, this.newName, this.makeId);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }

    }
}
