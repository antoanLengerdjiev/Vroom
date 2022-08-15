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
    public class Delete
    {
        [Test]
        public void ShouldSetIsDeletedToTrueOnTheModel()
        {
            // Arrange
            var mockedModel = new MockedModel() { IsDeleted = false };
            var mockedContext = new Mock<IVroomDbContext>();


            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act
            dbRepository.Delete(mockedModel);

            // Assert
            Assert.IsTrue(mockedModel.IsDeleted);
        }
        [Test]
        public void ShouldThrowArgumentNullException_WhenPassedParameterIsNull()
        {
            // Arrange 
            var mockedContext = new Mock<IVroomDbContext>();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dbRepository.Delete(null));

        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWithCorrectMessage_WhenPassedParameterIsNull()
        {
            // Arrange
            var expectedMessage = "Cannot Delete null object.";
            var mockedContext = new Mock<IVroomDbContext>();

            var dbRepository = new EfDbRepository<MockedModel>(mockedContext.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => dbRepository.Delete(null));

            StringAssert.Contains(expectedMessage, exception.Message);

        }
    }
}
