using Marketplace.API.Entities;
using Marketplace.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.API
{
    public class ShopDbContext : DbContext
    {
        private TenantContext? _tenantContext;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider? _serviceProvider;

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
            else
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("SardorDb"));
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
