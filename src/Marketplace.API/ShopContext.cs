using Marketplace.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.API
{
    public class ShopContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ShopContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.Migrate();
        }

        public DbSet<Shop> Shops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("MasterDb");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
