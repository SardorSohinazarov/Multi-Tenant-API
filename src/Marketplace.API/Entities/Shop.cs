using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.API.Entities
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Schema { get; set; }
    }

    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasIndex(s => s.Domain)
                .IsUnique();

            builder
                .HasIndex(s => s.Schema)
                .IsUnique();
        }
    }
}
