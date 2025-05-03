using Common.ServiceAttribute;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddCustomServices();

            return services;
        }
    }
}
