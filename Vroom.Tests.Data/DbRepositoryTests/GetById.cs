using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Tests.Data.Mocks;

namespace Vroom.Tests.Data.DbRepositoryTests
{
    [TestFixture]
    public class GetById
    {
        [TestCase(42)]
        [TestCase(6)]
        public void ShouldCallFindMethodOfDbSet_WhenValidIdIsPassed(int id)
        {
            //Arrange
            var mockedDbSet = new Mock<DbSet<MockedModel>>();
            mockedDbSet.Setup(s => s.Find(It.IsAny<int>())).Returns(new MockedModel() { IsDeleted = true }).Verifiable();

            var mockedContext = new Mock<IVroomDbContext>();
            mockedContext.Setup(c => c.Set<MockedModel>())
                .Returns(mockedDbSet.Object);

            var efRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act
            var entity = efRepository.GetById(id);

            // Assert
            mockedDbSet.Verify(s => s.Find(id), Times.Once);
        }

        [TestCase(15, "Emma")]
        [TestCase(23, "Watson")]
        public void ShouldReturnExactEntity_WhenValidIdIsPassedAndIsNotDeleted(int id, string name)
        {
            //Arrange
            var mockedModel = new MockedModel() { Id = id, Name = name, IsDeleted = false };

            var mockedDbSet = new Mock<DbSet<MockedModel>>();
            mockedDbSet.Setup(s => s.Find(id)).Returns(mockedModel);

            var mockedContext = new Mock<IVroomDbContext>();
            mockedContext.Setup(c => c.Set<MockedModel>())
                .Returns(mockedDbSet.Object);

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act
            var entity = dbRepository.GetById(id);

            // Assert
            Assert.AreSame(mockedModel, entity);
        }

        [TestCase(15, "Emma")]
        [TestCase(23, "Watson")]
        public void ShouldReturnNull_WhenValidIdIsPassedAndIsDeleted(int id, string name)
        {
            //Arrange
            var mockedModel = new MockedModel() { Id = id, Name = name, IsDeleted = true };

            var mockedDbSet = new Mock<DbSet<MockedModel>>();
            mockedDbSet.Setup(s => s.Find(id)).Returns(mockedModel);

            var mockedContext = new Mock<IVroomDbContext>();
            mockedContext.Setup(c => c.Set<MockedModel>())
                .Returns(mockedDbSet.Object);

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act
            var entity = dbRepository.GetById(id);

            // Assert
            Assert.AreSame(null, entity);
        }

        public void ShouldReturnNull_WhenIdIsNull()
        {
            //Arrange
            var mockedContext = new Mock<IVroomDbContext>();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act
            var entity = dbRepository.GetById(null);

            // Assert
            Assert.AreSame(null, entity);
        }
    }
}
