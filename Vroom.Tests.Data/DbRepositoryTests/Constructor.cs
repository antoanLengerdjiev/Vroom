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
    public class Constructor
    {
        [Test]
        public void ShouldThrowArgumentNullException_WhenNullContextIsPassed()
        {
            // Arrange, Act and Assert
            Assert.Throws<ArgumentNullException>(() => new EfDbRepository<MockedModel>(null));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenNullContextIsPassed()
        {
            // Arrange
            var expectedExMessage = "Database context cannot be null.";

            // Act and Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
            new EfDbRepository<MockedModel>(null));
            StringAssert.Contains(expectedExMessage, exception.Message);
        }

        [Test]
        public void ShouldCallSetMethodOfContext_WhenValidContextIsPassed()
        {
            // Arrange
            var mockedContext = new Mock<IVroomDbContext>();

            // Act
            var genericRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Asert
            mockedContext.Verify(c => c.Set<MockedModel>(), Times.Once);
        }
    }
}
