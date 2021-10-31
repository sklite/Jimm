using System.Reflection;
using FluentValidation;
using JimmyCms.Domain.Pipelines;
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

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResponseBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}