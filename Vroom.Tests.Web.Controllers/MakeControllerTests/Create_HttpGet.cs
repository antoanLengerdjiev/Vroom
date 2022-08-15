using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Controllers;
using Vroom.Models;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;

namespace Vroom.Tests.Web.Controllers.MakeControllerTests
{
    [TestFixture]
    public class Create_HttpGet
    {
        private Mock<IMapper> mockMapper;
        private Mock<IMakeService> mockMakeService;
        private Mock<IEncryptionProvider> mockEncryptionProvider;
        private MakeController controller;

        [SetUp]
        public void Setup()
        {


            this.mockMapper = new Mock<IMapper>();
            this.mockMakeService = new Mock<IMakeService>();
            this.mockEncryptionProvider = new Mock<IEncryptionProvider>();


            this.controller = new MakeController(this.mockMapper.Object, this.mockMakeService.Object, this.mockEncryptionProvider.Object);
        }


        [Test]
        public void ShouldReturnViewResult()
        {
            // Arrange

            // Act
            var result = this.controller.Create() as ViewResult;

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        
    }
}
