using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Domain.Entities;

namespace Shop.Infrastructure
{
    public class ShopDbContext : DbContext
    {
        private TenantContext? _tenantContext;
        private readonly string _schema;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider? _serviceProvider;

        public ShopDbContext(string schema)
        {
            _schema = schema;
        }

        public ShopDbContext(IConfiguration configuration, IServiceProvider? serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;

            if (_serviceProvider != null)
            {
                _tenantContext = _serviceProvider.GetService<TenantContext>();
            }

            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ShopDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schema = _tenantContext.CurrentShop?.Schema ?? _schema;
            modelBuilder.HasDefaultSchema(schema);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
    }
}
