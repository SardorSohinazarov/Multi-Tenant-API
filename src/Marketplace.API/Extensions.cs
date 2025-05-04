using Marketplace.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.API
{
    public static class Extensions
    {
            public static IServiceCollection AddDataContexts(this IServiceCollection services)
            {
                services.AddDbContext<ShopContext>();

                services.AddScoped(ctx =>
                {
                    IConfiguration config =
                        ctx.GetService<IConfiguration>()
                        ?? throw new Exception("Could not find configurations");

                    if (EF.IsDesignTime)
                    {
                        return new ShopDbContext(config, new DbContextOptions<ShopDbContext>());
                    }

                    IHttpContextAccessor httpContext =
                        ctx.GetService<IHttpContextAccessor>()
                        ?? throw new Exception("HTTP context not accessible");

                    ShopDbContext hqContext =
                        ctx.GetService<ShopDbContext>() ?? throw new Exception("HQ database not set");

                    string schema = "sardor";

                    return new ShopDbContext(config, schema);
                });

                return services;
        }

        public static WebApplication ApplyDbMigrations(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();

            ShopContext shopContext = scope.ServiceProvider.GetRequiredService<ShopContext>();

            shopContext.Database.Migrate();
            shopContext.ApplyDbMigrations(scope.ServiceProvider.GetRequiredService<IConfiguration>());

            return app;
        }

        public static void ApplyDbMigrations(this ShopContext hqContext, IConfiguration configuration)
        {
            foreach (string schema in hqContext.Shops.AsNoTracking().Select(x => x.Schema).ToList())
            {
                using ShopDbContext worldContext = new(configuration, schema);

                worldContext.Database.Migrate();
            }
        }

        public static Task ApplyDbMigrationsAsync(
            this ShopContext shopDbContext,
            IConfiguration configuration
        ) => Task.Run(() => shopDbContext.ApplyDbMigrations(configuration));
    }
}
