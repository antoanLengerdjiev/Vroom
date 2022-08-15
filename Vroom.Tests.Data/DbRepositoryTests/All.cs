using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Tests.Data.Mocks;

namespace Vroom.Tests.Data.DbRepositoryTests
{
    [TestFixture]
    public class All
    {

        [Test]
        public void AllMethodShouldReturnTheRightResult()
        {
            // Arrange
            var pesho = new MockedModel() { Id = 16, IsDeleted = false };
            var list = new List<MockedModel>()
            {
                new MockedModel() { Id =15,IsDeleted = true},
                pesho


            };
            var mockedDbSet = QueryableDbSetMock.GetQueryableMockDbSet<MockedModel>(list);
            var mockedContext = new Mock<IVroomDbContext>();
            mockedContext.Setup(s => s.Set<MockedModel>())
              .Returns(mockedDbSet);


            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act
            var all = dbRepository.All();

            // Assert
            Assert.AreEqual(new List<MockedModel> { pesho }, all);

        }
    }
}
