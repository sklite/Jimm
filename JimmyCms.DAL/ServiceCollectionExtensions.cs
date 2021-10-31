using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JimmyCms.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseConnector(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<IArticleContext, ArticleContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }
    }
}