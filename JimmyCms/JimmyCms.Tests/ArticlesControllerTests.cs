using System;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Controllers;
using JimmyCms.Domain.Messaging.Articles;
using JimmyCms.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace JimmyCms.Tests
{
    public class ArticlesControllerTests
    {
        private Mock<IMediator> _mockMediatr;

        [SetUp]
        public void Setup()
        {
            _mockMediatr = new Mock<IMediator>();
        }

        [Test]
        public async Task Get_Success_EmptyResponse()
        {
            //Arrange
            var controller = new ArticlesController(_mockMediatr.Object);

            _mockMediatr.Setup(m => m.Send(It.IsAny<IRequest<BasicResponse>>(), default))
                .ReturnsAsync(new BasicResponse());

            //Act
            var result = await controller.Get(Guid.NewGuid());

            //Assert
            Assert.That(result, Is.Not.Null);

            var objResult = result as ObjectResult;
            Assert.That(objResult, Is.Not.Null);
            Assert.That(objResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task Get_Fails_CustomStatusCode()
        {
            //Arrange
            var controller = new ArticlesController(_mockMediatr.Object);

            _mockMediatr.Setup(m => m.Send(It.IsAny<IRequest<BasicResponse>>(), default))
                .ReturnsAsync(new BasicResponse() {Success = false, ResponseCode = 400});

            //Act
            var result = await controller.Get(Guid.NewGuid());

            //Assert
            Assert.That(result, Is.Not.Null);

            var objResult = result as ObjectResult;
            Assert.That(objResult, Is.Not.Null);
            Assert.That(objResult.StatusCode, Is.EqualTo(400));
        }
    }
}