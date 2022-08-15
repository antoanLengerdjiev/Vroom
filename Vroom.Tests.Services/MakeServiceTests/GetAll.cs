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
using Vroom.Data.Models;
using Vroom.Models;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.MakeServiceTests
{
    [TestFixture]
    public class GetAll
    {
        private Mock<IEfDbRepository<Make>> mockMakeRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private MakeServiceModel makeServiceModel;
        private Make firstMake;
        private Make secondMake;
        private Make thirdMake;

        private List<Make> collectionOfDbMakes;
        private List<MakeServiceModel> collectionOfMake;
        public MakeService makeService;

        [SetUp]
        public void Setup()
        {
            this.mockMakeRepository = new Mock<IEfDbRepository<Make>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.makeServiceModel = new MakeServiceModel();

            this.firstMake = new Make() { Name = "enix" };
            this.secondMake = new Make() { Name = "Lenix" };
            this.thirdMake = new Make() { Name = "Gringe" };



            this.collectionOfDbMakes = new List<Make>() { this.firstMake, this.secondMake, this.thirdMake };
            this.collectionOfMake = new List<MakeServiceModel> { new MakeServiceModel(), new MakeServiceModel() };
            this.mockMakeRepository.Setup(x => x.All()).Returns(this.collectionOfDbMakes.AsQueryable());

            this.mockedMapper.Setup(x => x.Map<IEnumerable<Make>, IEnumerable<MakeServiceModel>>(this.collectionOfDbMakes)).Returns(this.collectionOfMake.AsQueryable());

            this.makeService = new MakeService(mockMakeRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }


        [Test]
        public void ShouldCallAllMethodFromMakeRepository()
        {
            // Arrange

            // Act
            this.makeService.GetAll();

            // Assert
            this.mockMakeRepository.Verify(x => x.All(), Times.Once);
        }


        [Test]
        public void ShouldCallMapMethodFromMapper()
        {
            // Arrange

            // Act
            this.makeService.GetAll();

            // Assert
            this.mockedMapper.Verify(x => x.Map<IEnumerable<Make>, IEnumerable<MakeServiceModel>>(this.collectionOfDbMakes));
        }


        [Test]
        public void ShouldReturnTypeOfIEnumerableOfMakeServiceModel()
        {
            // Arrange

            // Act
            var result = this.makeService.GetAll();
            // Assert
            Assert.IsInstanceOf<IEnumerable<MakeServiceModel>>(result);
        }


        [Test]
        public void ShouldReturnCorrectResult()
        {
            // Arrange

            // Act
            var result = this.makeService.GetAll();

            // Assert
            Assert.AreEqual(this.collectionOfMake, result);
        }
    }
}
