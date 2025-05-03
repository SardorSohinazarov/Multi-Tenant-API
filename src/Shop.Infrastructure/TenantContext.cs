using Admin.Domain.Entities;

namespace Shop.Infrastructure
{
    public class TenantContext
    {
        public ShopConfig CurrentShop { get; set; }
    }
}
