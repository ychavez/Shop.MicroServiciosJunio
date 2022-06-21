using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;
using System.Reflection;

namespace Ordering.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            //Configuramos automaper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //Configuramos FluentValidator
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //Configuramos MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Configuramos los behaviours
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }

    }
}
