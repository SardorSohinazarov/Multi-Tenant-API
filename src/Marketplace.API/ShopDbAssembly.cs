using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Marketplace.API
{
    public class ShopDbAssembly : MigrationsAssembly, IMigrationsAssembly
    {
        private readonly DbContext _context;

        public ShopDbAssembly(
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger
        )
            : base(currentContext, options, idGenerator, logger)
            => _context = currentContext.Context;

        public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
        {
            if (activeProvider == null || activeProvider == string.Empty)
            {
                throw new ArgumentNullException(nameof(activeProvider));
            }

            bool shopDbMigration =
                migrationClass.GetConstructor(new[] { typeof(IShopDbContext) }) != null;

            if (shopDbMigration && _context is IShopDbContext storeContext)
            {
                Migration? migration = (Migration?)
                    Activator.CreateInstance(migrationClass.AsType(), storeContext);

                if (migration != null)
                {
                    migration.ActiveProvider = activeProvider;

                    return migration;
                }
            }

            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
}
