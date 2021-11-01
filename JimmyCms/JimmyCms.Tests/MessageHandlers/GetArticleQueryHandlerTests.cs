using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Domain.Messaging.Articles.Commands;
using JimmyCms.Domain.Messaging.Articles.Queries;
using JimmyCms.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace JimmyCms.Tests.MessageHandlers
{
    [TestFixture]
    public class GetArticleQueryHandlerTests
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
        public async Task Handle_Success_AscOrder()
        {
            //Arrange
            var id10DaysAgo = Guid.NewGuid();
            var id1DaysAgo = Guid.NewGuid();
            var id2DaysAgo = Guid.NewGuid();
            var id5DaysAgo = Guid.NewGuid();

            var articles = new List<Article>
            {
                new () {Id = id10DaysAgo, CreatedOn = DateTime.Now.AddDays(-10)},
                new () {Id = id1DaysAgo, CreatedOn = DateTime.Now.AddDays(-1)},
                new () {Id = id2DaysAgo, CreatedOn = DateTime.Now.AddDays(-2)},
                new () {Id = id5DaysAgo, CreatedOn = DateTime.Now.AddDays(-5)},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Article>>();
            mockSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(articles.Provider);
            mockSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(articles.Expression);
            mockSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(articles.ElementType);
            mockSet.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(articles.GetEnumerator());

            _mockContext
                .Setup(db => db.Articles)
                .Returns(mockSet.Object);

            var handler = new GetArticlesQueryHandler(_mockContext.Object);
            var request = new GetArticlesQuery(true, 2, 5);


            //Act
            var result = await handler.Handle(request, CancellationToken.None);


            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            var articlesResult = result.Value as IQueryable<Article>;

            Assert.That(articlesResult, Is.Not.Null);
            Assert.That(articlesResult, Is.Not.Empty);

            var articleList = articlesResult.ToList();
            Assert.That(articleList.Count, Is.EqualTo(2));
            Assert.That(articleList[0].Id, Is.EqualTo(id2DaysAgo));
            Assert.That(articleList[1].Id, Is.EqualTo(id1DaysAgo));
        }

        [Test]
        public async Task Handle_Success_DescOrder()
        {
            //Arrange
            var id10DaysAgo = Guid.NewGuid();
            var id1DaysAgo = Guid.NewGuid();
            var id2DaysAgo = Guid.NewGuid();
            var id5DaysAgo = Guid.NewGuid();

            var articles = new List<Article>
            {
                new () {Id = id10DaysAgo, CreatedOn = DateTime.Now.AddDays(-10)},
                new () {Id = id1DaysAgo, CreatedOn = DateTime.Now.AddDays(-1)},
                new () {Id = id2DaysAgo, CreatedOn = DateTime.Now.AddDays(-2)},
                new () {Id = id5DaysAgo, CreatedOn = DateTime.Now.AddDays(-5)},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Article>>();
            mockSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(articles.Provider);
            mockSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(articles.Expression);
            mockSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(articles.ElementType);
            mockSet.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(articles.GetEnumerator());

            _mockContext
                .Setup(db => db.Articles)
                .Returns(mockSet.Object);

            var handler = new GetArticlesQueryHandler(_mockContext.Object);
            var request = new GetArticlesQuery(false, 0, 3);


            //Act
            var result = await handler.Handle(request, CancellationToken.None);


            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);

            var articlesResult = result.Value as IQueryable<Article>;

            Assert.That(articlesResult, Is.Not.Null);
            Assert.That(articlesResult, Is.Not.Empty);

            var articleList = articlesResult.ToList();
            Assert.That(articleList.Count, Is.EqualTo(3));
            Assert.That(articleList[0].Id, Is.EqualTo(id1DaysAgo));
            Assert.That(articleList[1].Id, Is.EqualTo(id2DaysAgo));
            Assert.That(articleList[2].Id, Is.EqualTo(id5DaysAgo));
        }
    }
}