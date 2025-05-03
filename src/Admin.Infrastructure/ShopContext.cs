using Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options)
            : base(options) 
            => Database.Migrate();

        public DbSet<ShopConfig> Shops { get; set; }
    }
}
