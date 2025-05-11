using Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Admin.Infrastructure
{
    public class ShopContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ShopContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.Migrate();
        }

        public DbSet<ShopConfig> Shops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("AdminDb");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
