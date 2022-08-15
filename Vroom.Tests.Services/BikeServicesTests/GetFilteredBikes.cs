using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models;
using Vroom.Models;
using Vroom.Service.Data;
using Vroom.Service.Models;

namespace Vroom.Tests.Services.BikeServicesTests
{
    [TestFixture]
    public class GetFilteredBikes
    {
        private Mock<IEfDbRepository<Bike>> mockBikeRepository;
        private Mock<IVroomDbContextSaveChanges> mockSaveChanges;
        private Mock<IMapper> mockedMapper;
        private BikeServiceModel bikeServiceModel;
        private Make firstMake;
        private Make secondMake;
        private Make thirdMake;
        private Bike firstbike;
        private Bike secondbike;
        private Bike thirdbike;
        private Bike fourtbike;
        private Bike fifthbike;
        private List<Bike> collectionOfDbBikes;
        private List<BikeServiceModel> collectionOfBikes;
        public BikeService bikeService;

        [SetUp]
        public void Setup()
        {
            this.mockBikeRepository = new Mock<IEfDbRepository<Bike>>();
            this.mockSaveChanges = new Mock<IVroomDbContextSaveChanges>();
            this.mockedMapper = new Mock<IMapper>();

            this.bikeServiceModel = new BikeServiceModel();

            this.firstMake = new Make() { Name = "enix" };
            this.secondMake = new Make() { Name = "Lenix" };
            this.thirdMake = new Make() { Name = "Gringe" };



            this.firstbike = new Bike() { Id = 1, Model = new Model { Make = this.firstMake }, Price = 10 };
            this.secondbike = new Bike() { Id = 2, Model = new Model { Make = this.secondMake }, Price = 100 };
            this.thirdbike = new Bike() { Id = 3, Model = new Model { Make = this.thirdMake }, Price = 100 };
            this.fourtbike = new Bike() { Id = 4, Model = new Model { Make = this.firstMake }, Price = 90 };
            this.fifthbike = new Bike() { Id = 5, Model = new Model { Make = this.secondMake }, Price = 9 };
            this.collectionOfDbBikes = new List<Bike>() { this.firstbike, this.secondbike, this.thirdbike, this.fourtbike, this.fifthbike };
            this.collectionOfBikes = new List<BikeServiceModel> { new BikeServiceModel(), new BikeServiceModel() };
            this.mockBikeRepository.Setup(x => x.All()).Returns(this.collectionOfDbBikes.AsQueryable());

            this.mockedMapper.Setup(x => x.Map<IEnumerable<Bike>, IEnumerable<BikeServiceModel>>(It.IsAny<IEnumerable<Bike>>())).Returns(this.collectionOfBikes);

            this.bikeService = new BikeService(mockBikeRepository.Object, this.mockedMapper.Object, this.mockSaveChanges.Object);
        }


        [Test]
        public void ShouldCallAllMethodFromBikeRepository_WhenParamsAreValid()
        {
            // Arrange

            // Act
            this.bikeService.GetFilteredBikes(1, 2, "", "");

            // Assert
            this.mockBikeRepository.Verify(x => x.All(), Times.Once);
        }


        [Test]
        public void ShouldCallMapMethodFromMapper_WhenParamsAreValid()
        {
            // Arrange

            // Act
            this.bikeService.GetFilteredBikes(1, 2, "", "");

            // Assert
            this.mockedMapper.Verify(x => x.Map<IEnumerable<Bike>, IEnumerable<BikeServiceModel>>(It.IsAny<IEnumerable<Bike>>()));
        }

        [TestCase(1,2,"enix",4)]
        [TestCase(1, 2, "Grin", 1)]
        [TestCase(1,2,"len",2)]
        public void ShouldReturnCorrectTotalSize_WhenParamsAreValid(int pageNumber, int pageSize,string searchPattern, int expectedResult)
        {
            // Arrange

            // Act
            var result = this.bikeService.GetFilteredBikes(pageNumber, pageSize, searchPattern, "");

            // Assert
            Assert.AreEqual(expectedResult, result.TotalSize);
        }

        [Test]
        public void ShouldReturnTypeOfPageBikeServiceModel_WhenParamsAreValid()
        {
            // Arrange

            // Act
            var result = this.bikeService.GetFilteredBikes(1, 2, "", "");

            // Assert
            Assert.IsInstanceOf<PagedBikeServiceModel>(result);
        }


        [Test]
        public void ShouldReturnCorrectResult_WhenParamsAreValid()
        {
            // Arrange

            // Act
            var result = this.bikeService.GetFilteredBikes(1, 2, "", "");

            // Assert
            Assert.AreEqual(this.collectionOfBikes, result.Bikes);
        }
    }

    public class BikeComparer<T> : IComparer<T> where T : Bike
    {
       
        public int Compare(T x, T y)
        {
            if (x != null || y != null)
            {
                var xMakeName = x.Model.Make.Name;
                var yMakeName = y.Model.Make.Name;
                var leght = xMakeName.Length < yMakeName.Length ? xMakeName.Length : yMakeName.Length;

                for (int i = 0; i < leght; i++)
                {
                    if(xMakeName[i] != yMakeName[i])
                    {
                        return ((int) xMakeName[i]).CompareTo(((int)yMakeName[i]));
                    }
                }
            }

            return 0;
        }
    }
}
