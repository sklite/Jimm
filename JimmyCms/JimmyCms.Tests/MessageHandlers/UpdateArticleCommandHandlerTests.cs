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
    public class UpdateArticleCommandHandlerTests
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

            var articles = new List<Article>
            {
                new () {Id = id, Body = "Old body", Title = "Old title", CreatedOn = DateTime.Now.AddDays(-30), UpdatedOn = DateTime.Now.AddDays(-2)},
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

            var handler = new UpdateArticleCommandHandler(_mockContext.Object);
            var request = new UpdateArticleCommand(id, "new title", "new body");


            //Act
            var result = await handler.Handle(request, CancellationToken.None);


            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            var updatedArticle = _mockContext.Object.Articles.FirstOrDefault(a => a.Id == id);
            Assert.That(updatedArticle, Is.Not.Null);
            Assert.That(updatedArticle.Id, Is.EqualTo(id));
            Assert.That(updatedArticle.Title, Is.EqualTo("new title"));
            Assert.That(updatedArticle.Body, Is.EqualTo("new body"));
            Assert.That(updatedArticle.UpdatedOn, Is.EqualTo(DateTime.Now).Within(10).Seconds);
            Assert.That(updatedArticle.CreatedOn, Is.EqualTo(DateTime.Now.AddDays(-30)).Within(10).Seconds);
        }

        [Test]
        public void Handle_Fail_ArticleNotFound()
        {
            //Arrange
            var id = Guid.NewGuid();
            var articleId = Guid.NewGuid();

            var articles = new List<Article>
            {
                new () {Id = articleId, Body = "Old body", Title = "Old title", CreatedOn = DateTime.Now.AddDays(-30), UpdatedOn = DateTime.Now.AddDays(-2)},
            }.AsQueryable();


            var mockSet = new Mock<DbSet<Article>>();
            mockSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(articles.Provider);
            mockSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(articles.Expression);
            mockSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(articles.ElementType);
            mockSet.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(articles.GetEnumerator());

            _mockContext
                .Setup(db => db.Articles)
                .Returns(mockSet.Object);

            var handler = new UpdateArticleCommandHandler(_mockContext.Object);
            var request = new UpdateArticleCommand(id, "new title", "new body");


            //Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () => await handler.Handle(request, CancellationToken.None));


            //Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.GetType(), Is.EqualTo(typeof(KeyNotFoundException)));
            Assert.That(exception.Message, Is.EqualTo($"Can't find article with id {request.Id}"));

            var updatedArticle = _mockContext.Object.Articles.FirstOrDefault(a => a.Id == articleId);
            Assert.That(updatedArticle, Is.Not.Null);
            Assert.That(updatedArticle.Id, Is.EqualTo(articleId));
            Assert.That(updatedArticle.Title, Is.EqualTo("Old title"));
            Assert.That(updatedArticle.Body, Is.EqualTo("Old body"));
            Assert.That(updatedArticle.UpdatedOn, Is.EqualTo(DateTime.Now.AddDays(-2)).Within(10).Seconds);
            Assert.That(updatedArticle.CreatedOn, Is.EqualTo(DateTime.Now.AddDays(-30)).Within(10).Seconds);
        }
    }
}