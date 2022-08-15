using Moq;
using NUnit.Framework;
using System;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Tests.Data.Mocks;

namespace Vroom.Tests.Data.DbRepositoryTests
{
    [TestFixture]
    public class Add
    {
        [Test]
        public void ShouldCallDbContextAddMethod()
        {
            // Arrange 
            var mockedModel = new MockedModel();
            var mockedContext = new Mock<IVroomDbContext>();
            mockedContext.Setup(x => x.Set<MockedModel>().Add(mockedModel)).Verifiable();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act
            dbRepository.Add(mockedModel);

            // Assert
            mockedContext.Verify(x => x.Set<MockedModel>().Add(mockedModel), Times.Once);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenPassedParameterIsNull()
        {
            // Arrange 
            var mockedContext = new Mock<IVroomDbContext>();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dbRepository.Add(null));

        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenPassedParameterIsNull()
        {
            // Arrange
            var expectedMessage = "Cannot Add null object.";
            var mockedContext = new Mock<IVroomDbContext>();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => dbRepository.Add(null));

            StringAssert.Contains(expectedMessage, exception.Message);

        }
    }
}
