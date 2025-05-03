using Marketplace.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.API
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) 
        {
            Database.Migrate();
        }

        public DbSet<Shop> Shops { get; set; }
    }
}
