using Marketplace.API.Entities;
using Marketplace.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Marketplace.API
{
    public class ShopDbContext : DbContext
    {
        private TenantContext? _tenantContext;
        private readonly string? _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider? _serviceProvider;

        public ShopDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ShopDbContext(IConfiguration configuration, IServiceProvider? serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;

            if (_serviceProvider != null)
            {
                _tenantContext = _serviceProvider.GetService<TenantContext>();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_tenantContext?.CurrentShop != null)
            {
                optionsBuilder.UseNpgsql(_tenantContext.CurrentShop.ConnectionString);
            }

            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        public DbSet<Product> Products { get; set; }
    }

    public class ShopDbContextFactory : IDesignTimeDbContextFactory<ShopDbContext>
    {
        public ShopDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("SardorDb"));

            // Design-time uchun serviceProvider va tenantContext kerak emas
            return new ShopDbContext(configuration, null);
        }
    }
}
