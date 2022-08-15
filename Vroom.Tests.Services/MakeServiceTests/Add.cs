using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Models;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.MakeServiceTests
{
    [TestFixture]
    public class Add
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

            this.mockedMapper.Setup(x => x.Map<MakeServiceModel, Make>(this.makeServiceModel)).Returns(this.make);

            this.makeService = new MakeService(mockMakeRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParameterModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.makeService.Add(null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectExceptionMessege_WhenParameterModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => this.makeService.Add(null));
            StringAssert.Contains("Parameter model cannot be null or empty", exception.Message);
        }


        [Test]
        public void ShouldCallMapMethodFromMapper_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
           this.makeService.Add(this.makeServiceModel);
            // Assert
            this.mockedMapper.Verify(x => x.Map<MakeServiceModel, Make>(this.makeServiceModel), Times.Once);
        }

        [Test]
        public void ShouldCallAddMethodFromMakeRepository_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            this.makeService.Add(this.makeServiceModel);
            // Assert
            this.mockMakeRepository.Verify(x => x.Add(this.make), Times.Once);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromDbSaveChanges_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            this.makeService.Add(this.makeServiceModel);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }
    
    }
}
