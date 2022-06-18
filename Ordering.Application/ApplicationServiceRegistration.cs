using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            // Ijections goes here;

            return services;
        }

    }
}
