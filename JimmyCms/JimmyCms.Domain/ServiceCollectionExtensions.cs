using System.Reflection;
using FluentValidation;
using JimmyCms.Domain.Pipelines;
using JimmyCms.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JimmyCms.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ValidationSettings>(config.GetSection("ValidationSettings"));
            services.Configure<SecuritySettings>(config.GetSection("Security"));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResponseBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}