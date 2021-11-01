using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JimmyCms.Infrastructure
{
    public interface IArticleContext
    {
        public DbSet<Article> Articles { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}