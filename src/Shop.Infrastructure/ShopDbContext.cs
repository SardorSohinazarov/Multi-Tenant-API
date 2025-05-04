using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Domain.Entities;

namespace Shop.Infrastructure
{
    public class ShopDbContext : DbContext
    {
        private TenantContext? _tenantContext;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider? _serviceProvider;

        public ShopDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ShopDbContext(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;

            if (_serviceProvider != null)
            {
                _tenantContext = _serviceProvider.GetService<TenantContext>();
            }

            Database.Migrate();
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _tenantContext?.CurrentShop.ConnectionString ?? _connectionString ?? _configuration.GetConnectionString("ShopDb");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
