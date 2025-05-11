using Admin.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure;

namespace Admin.Api
{
    public static class Extensions
    {
        public static WebApplication ApplyDbMigrations(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();

            ShopContext shopContext = scope.ServiceProvider.GetRequiredService<ShopContext>();

            shopContext.Database.Migrate();
            shopContext.ApplyDbMigrations(scope.ServiceProvider.GetRequiredService<IConfiguration>());

            return app;
        }

        public static void ApplyDbMigrations(this ShopContext shopContext, IConfiguration configuration)
        {
            foreach (string schema in shopContext.Shops.AsNoTracking().Select(x => x.Schema).ToList())
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
