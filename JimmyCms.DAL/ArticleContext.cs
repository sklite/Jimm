using Microsoft.EntityFrameworkCore;

namespace JimmyCms.Infrastructure
{
    class ArticleContext : DbContext, IArticleContext
    {
        public DbSet<Article> Articles { get; set; }

        public ArticleContext(DbContextOptions<ArticleContext> options)
            :base(options)
        { }
    }
}