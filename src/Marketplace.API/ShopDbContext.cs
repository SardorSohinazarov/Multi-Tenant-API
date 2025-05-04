using Marketplace.API.Entities;
using Marketplace.API.Exceptions;
using Marketplace.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Marketplace.API
{
    public class ShopDbContext : DbContext
    {
        private TenantContext? _tenantContext;
        private readonly string? _schema;
        private readonly IConfiguration _configuration;

        public ShopDbContext(IConfiguration configuration, DbContextOptions<ShopDbContext> options)
        : base(options)
        {
            _schema = "public";

            _configuration = configuration;
        }

        public ShopDbContext(IConfiguration configuration, string schema)
        {
            _schema = schema;
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

            optionsBuilder.UseNpgsql(
                connectionString,
                x => x.MigrationsHistoryTable("_migrations", _schema)
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schema = _tenantContext?.CurrentShop.Schema ?? _schema ?? "public";

            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.Entity<Product>(x =>
            {
                x.Metadata.SetSchema(schema);
            });

            // hamma entitylar uchun
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    var a = entityType.GetSchema();
            //    entityType.SetSchema(schema);
            //}

            base.OnModelCreating(modelBuilder);
        }
    }

    //public class ShopDbContextFactory : IDesignTimeDbContextFactory<ShopDbContext>
    //{
    //    public ShopDbContext CreateDbContext(string[] args)
    //    {
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build();

    //        // Dizayn vaqti uchun schema nomini qo‘lda belgilang
    //        var schema = "public";

    //        return new ShopDbContext(schema)
    //        {
    //            // _configuration qiymati konstruktor orqali emas, OnConfiguring orqali ishlatiladi
    //        };
    //    }
    //}
}
