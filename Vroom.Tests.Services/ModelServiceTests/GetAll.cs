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
    public class GetAll
    {
        private Mock<IEfDbRepository<Model>> mockModelRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private ModelServiceModel modelServiceModel;
        private Model firstModel;
        private Model secondModel;
        private Model thirdModel;

        private List<Model> collectionOfDbModel;
        private List<ModelServiceModel> collectionOfModel;
        public ModelService modelService;

        [SetUp]
        public void Setup()
        {
            this.mockModelRepository = new Mock<IEfDbRepository<Model>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.modelServiceModel = new ModelServiceModel();

            this.firstModel = new Model() { Name = "enix", MakeId = 3 };
            this.secondModel = new Model() { Name = "Lenix", MakeId = 5 };
            this.thirdModel = new Model() { Name = "Gringe", MakeId = 3 };



            this.collectionOfDbModel = new List<Model>() { this.firstModel, this.secondModel, this.thirdModel };
            this.collectionOfModel = new List<ModelServiceModel> { new ModelServiceModel(), new ModelServiceModel() };
            this.mockModelRepository.Setup(x => x.All()).Returns(this.collectionOfDbModel.AsQueryable());

            this.mockedMapper.Setup(x => x.Map<IEnumerable<Model>, IEnumerable<ModelServiceModel>>(this.collectionOfDbModel)).Returns(this.collectionOfModel.AsQueryable());

            this.modelService = new ModelService(mockModelRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }


        [Test]
        public void ShouldCallAllMethodFromModelRepository()
        {
            // Arrange

            // Act
            this.modelService.GetAll();

            // Assert
            this.mockModelRepository.Verify(x => x.All(), Times.Once);
        }


        [Test]
        public void ShouldCallMapMethodFromMapper()
        {
            // Arrange

            // Act
            this.modelService.GetAll();

            // Assert
            this.mockedMapper.Verify(x => x.Map<IEnumerable<Model>, IEnumerable<ModelServiceModel>>(this.collectionOfDbModel));
        }


        [Test]
        public void ShouldReturnTypeOfIEnumerableOfModelServiceModel()
        {
            // Arrange

            // Act
            var result = this.modelService.GetAll();
            // Assert
            Assert.IsInstanceOf<IEnumerable<ModelServiceModel>>(result);
        }


        [Test]
        public void ShouldReturnCorrectResult()
        {
            // Arrange

            // Act
            var result = this.modelService.GetAll();

            // Assert
            Assert.AreEqual(this.collectionOfModel, result);
        }


        //// GetAll with Parameter
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeException_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.GetAll(negativeId));
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentOutOfRangeExceptionWithCorrectExceptionMessege_WhenParameterIdIsLessThanZero(int negativeId)
        {
            // Arrange

            // Act

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => this.modelService.GetAll(negativeId));
            StringAssert.Contains("Cannot be zero or less", exception.Message);
        }

        [Test]
        public void ShouldCallAllMethodFromModelRepository_WhenMethodWithParamsIsInvoked()
        {
            // Arrange

            // Act
            this.modelService.GetAll(this.firstModel.MakeId);

            // Assert
            this.mockModelRepository.Verify(x => x.All(), Times.Once);
        }


        [Test]
        public void ShouldCallMapMethodFromMapper_WhenMethodWithParamsIsInvoked()
        {
            // Arrange
            this.mockedMapper.Setup(x => x.Map<IEnumerable<Model>, IEnumerable<ModelServiceModel>>(It.IsAny<IEnumerable<Model>>())).Returns(this.collectionOfModel);
            
            // Act
            this.modelService.GetAll(this.firstModel.MakeId);

            // Assert
            this.mockedMapper.Verify(x => x.Map<IEnumerable<Model>, IEnumerable<ModelServiceModel>>(It.IsAny<IEnumerable<Model>>()));
        }


        [Test]
        public void ShouldReturnTypeOfIEnumerableOfModelServiceModel_WhenMethodWithParamsIsInvoked()
        {
            // Arrange

            // Act
            var result = this.modelService.GetAll(this.firstModel.MakeId);

            // Assert
            Assert.IsInstanceOf<IEnumerable<ModelServiceModel>>(result);
        }


        [Test]
        public void ShouldReturnCorrectResult_WhenMethodWithParamsIsInvoked()
        {
            // Arrange
            this.mockedMapper.Setup(x => x.Map<IEnumerable<Model>, IEnumerable<ModelServiceModel>>(It.IsAny<IEnumerable<Model>>())).Returns(this.collectionOfModel);
            // Act
            var result = this.modelService.GetAll(this.firstModel.MakeId);

            // Assert
            Assert.AreEqual(this.collectionOfModel, result);
        }
    }
}
