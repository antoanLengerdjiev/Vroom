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
    public class HardDelete
    {
        [Test]
        public void ShouldCallDbContextRemoveMethod()
        {
            // Arrange 
            var mockedModel = new MockedModel();
            var mockedDbset = new Mock<DbSet<MockedModel>>();
            var mockedContext = new Mock<IVroomDbContext>();

            mockedContext.Setup(x => x.Set<MockedModel>().Remove(mockedModel)).Verifiable();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);


            // Act
            dbRepository.HardDelete(mockedModel);

            // Assert
            mockedContext.Verify(x => x.Set<MockedModel>().Remove(mockedModel), Times.Once);
        }

        [Test]
        public void ShouldThrowArgumentNullException_WhenPassedParameterIsNull()
        {
            // Arrange 
            var mockedContext = new Mock<IVroomDbContext>();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dbRepository.HardDelete(null));

        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenPassedParameterIsNull()
        {
            // Arrange
            var expectedMessage = "Cannot Hard Delete null object.";
            var mockedContext = new Mock<IVroomDbContext>();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => dbRepository.HardDelete(null));

            StringAssert.Contains(expectedMessage, exception.Message);

        }
    }
}
