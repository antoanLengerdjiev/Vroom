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
    public class Add
    {
        private Mock<IEfDbRepository<Model>> mockModelRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private ModelServiceModel modelServiceModel;
        private Model model;
        public ModelService modelService;

        [SetUp]
        public void Setup()
        {
            this.mockModelRepository = new Mock<IEfDbRepository<Model>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.modelServiceModel = new ModelServiceModel();
            this.model = new Model() { Id = 3 };

            this.mockedMapper.Setup(x => x.Map<ModelServiceModel, Model>(this.modelServiceModel)).Returns(this.model);

            this.modelService = new ModelService(mockModelRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenParameterModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.modelService.Add(null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectExceptionMessege_WhenParameterModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => this.modelService.Add(null));
            StringAssert.Contains("Parameter model cannot be null or empty", exception.Message);
        }


        [Test]
        public void ShouldCallMapMethodFromMapper_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            this.modelService.Add(this.modelServiceModel);
            // Assert
            this.mockedMapper.Verify(x => x.Map<ModelServiceModel, Model>(this.modelServiceModel), Times.Once);
        }

        [Test]
        public void ShouldCallAddMethodFromModelRepository_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            this.modelService.Add(this.modelServiceModel);
            // Assert
            this.mockModelRepository.Verify(x => x.Add(this.model), Times.Once);
        }

        [Test]
        public void ShouldCallSaveChangesMethodFromDbSaveChanges_WhenParameterModelIsNotNull()
        {
            // Arrange

            // Act
            this.modelService.Add(this.modelServiceModel);
            // Assert
            this.mockSaveChanges.Verify(x => x.SaveChanges(), Times.Once);
        }

    }
}
