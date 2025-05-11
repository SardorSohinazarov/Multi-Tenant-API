using Microsoft.EntityFrameworkCore;

namespace Marketplace.API
{
    public static class Extensions
    {
            public static IServiceCollection AddDataContexts(this IServiceCollection services)
            {
                services.AddHttpContextAccessor();
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

                    IHttpContextAccessor httpContextAccessor =
                        ctx.GetService<IHttpContextAccessor>()
                        ?? throw new Exception("HTTP context not accessible");

                    ShopContext shopContext =
                        ctx.GetService<ShopContext>() ?? throw new Exception("ShopContext database not set");

                    string schema =
                        httpContextAccessor.HttpContext?.GetSchemaFromDomain(shopContext)
                        ?? "public";

                    return new ShopDbContext(config, schema);
                });

                return services;
        }

        public static string? GetSchemaFromDomain(this HttpContext httpContext, ShopContext shopContext)
        {
            string? host = httpContext.Request.Host.Value;
            if (string.IsNullOrEmpty(host))
                return null;

            string? schema = shopContext.Shops
                .AsNoTracking()
                .Where(x => x.Domain == host)
                .Select(x => x.Schema)
                .FirstOrDefault();

            return schema;
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
                using ShopDbContext shopDbContext = new(configuration, schema);

                shopDbContext.Database.Migrate();
            }
        }

        public static Task ApplyDbMigrationsAsync(
            this ShopContext shopDbContext,
            IConfiguration configuration
        ) => Task.Run(() => shopDbContext.ApplyDbMigrations(configuration));
    }
}
