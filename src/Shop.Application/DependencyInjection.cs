using Common.ServiceAttribute;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shop.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddCustomServices();

            return services;
        }
    }
}
