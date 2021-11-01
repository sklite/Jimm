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
    public class DeleteArticleCommandHandlerTests
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
        public async Task Handle_Success_ArticleFound()
        {
            //Arrange
            var id = Guid.NewGuid();
            var articleToRemove = new Article()
            {
                Id = id
            };

            var articles = new List<Article>
            {
                articleToRemove,
                new () {Id = Guid.NewGuid()},
                new () {Id = Guid.NewGuid()},
            }.AsQueryable();


            var mockSet = new Mock<DbSet<Article>>();
            mockSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(articles.Provider);
            mockSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(articles.Expression);
            mockSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(articles.ElementType);
            mockSet.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(articles.GetEnumerator());

            _mockContext
                .Setup(db => db.Articles)
                .Returns(mockSet.Object);

            var handler = new DeleteArticleCommandHandler(_mockContext.Object);
            var request = new DeleteArticleCommand(id);


            //Act
            var result = await handler.Handle(request, CancellationToken.None);


            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            mockSet.Verify(s => s.Remove(articleToRemove), Times.Once);
        }

        [Test]
        public void Handle_Fail_ArticleNotFound()
        {
            //Arrange
            var id = Guid.NewGuid();
            var articles = Enumerable.Repeat(0, 10).Select(_ => new Article()).AsQueryable();

            var mockSet = new Mock<DbSet<Article>>();
            mockSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(articles.Provider);
            mockSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(articles.Expression);
            mockSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(articles.ElementType);
            mockSet.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(articles.GetEnumerator());

            _mockContext
                .Setup(db => db.Articles)
                .Returns(mockSet.Object);

            var handler = new DeleteArticleCommandHandler(_mockContext.Object);
            var request = new DeleteArticleCommand(id);


            //Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(request, CancellationToken.None));


            //Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.GetType(), Is.EqualTo(typeof(KeyNotFoundException)));
            Assert.That(exception.Message, Is.EqualTo($"Can't find article with id {request.Id}"));

            mockSet.Verify(s => s.Remove(It.IsAny<Article>()), Times.Never);
        }
    }
}