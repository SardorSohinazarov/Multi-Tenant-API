using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Shop.Domain.Entities;

namespace Shop.Infrastructure;

public class ShopDbContext : DbContext, IShopDbContext
{
    private TenantContext? _tenantContext;
    public string Schema { get; set; }
    private readonly IConfiguration _configuration;

    public ShopDbContext(IConfiguration configuration, DbContextOptions<ShopDbContext> options)
    : base(options)
    {
        Schema = "public";
        _configuration = configuration;
    }

    public ShopDbContext(IConfiguration configuration, string schema)
    {
        Schema = schema;
        _configuration = configuration;
    }

    public ShopDbContext(IConfiguration configuration, TenantContext tenantContext)
    {
        _configuration = configuration;
        _tenantContext = tenantContext;
    }

    public DbSet<Product> Products { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("ShopDb");
        optionsBuilder.UseNpgsql(connectionString);

        optionsBuilder.ReplaceService<IMigrationsAssembly, ShopDbAssembly>()
            .ReplaceService<IModelCacheKeyFactory, ShopDbCacheKeyFactory>();

        optionsBuilder.UseNpgsql(
            connectionString,
            x => x.MigrationsHistoryTable("_migrations", Schema)
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var schema = _tenantContext?.CurrentShop.Schema ?? Schema ?? "public";

        modelBuilder.Entity<Product>(x =>
        {
            x.Metadata.SetSchema(schema);
        });

        base.OnModelCreating(modelBuilder);
    }
}
