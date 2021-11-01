using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Domain.Messaging.Articles.Commands;
using JimmyCms.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace JimmyCms.Tests.MessageHandlers
{
    [TestFixture]
    public class CreateArticleCommandHandlerTests
    {
        private Mock<IArticleContext> _mockContext;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IArticleContext>();
            _mockContext
                .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);
        }

        [Test]
        public async Task Handle_Success_ArticleCreated()
        {
            //Arrange
            var articles = new List<Article>().AsQueryable();

            var mockSet = new Mock<DbSet<Article>>();
            mockSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(articles.Provider);
            mockSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(articles.Expression);
            mockSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(articles.ElementType);
            mockSet.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(articles.GetEnumerator());

            _mockContext
                .Setup(db => db.Articles)
                .Returns(mockSet.Object);

            var handler = new CreateArticleCommandHandler(_mockContext.Object);
            var request = new CreateArticleCommand("new title", "new body");


            //Act
            var result = await handler.Handle(request, CancellationToken.None);


            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            var createdArticle = result.Value as Article;

            Assert.That(createdArticle, Is.Not.Null);
            Assert.That(createdArticle.Title, Is.EqualTo("new title"));
            Assert.That(createdArticle.Body, Is.EqualTo("new body"));
            Assert.That(createdArticle.CreatedOn, Is.EqualTo(DateTime.Now).Within(10).Seconds);
        }
    }
}