﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShopInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
