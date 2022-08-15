using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Tests.Data.Mocks;

namespace Vroom.Tests.Data.DbRepositoryTests
{
    [TestFixture]
    public class AllWithDeleted
    {
        [Test]
        public void ShouldReturnDbSet()
        {
            var mockedDbSet = new Mock<DbSet<MockedModel>>();
            var mockedContext = new Mock<IVroomDbContext>();
            mockedContext.Setup(s => s.Set<MockedModel>())
                .Returns(mockedDbSet.Object);

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act
            var all = dbRepository.AllWithDeleted();

            // Assert
            Assert.AreSame(mockedDbSet.Object, all);

        }


    }
}
