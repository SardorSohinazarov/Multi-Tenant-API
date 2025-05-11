using Admin.Domain.Exceptions;
using Admin.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure;

namespace Shop.Api.Middlewares
{
    public class ShopTenantMiddleware
    {
        private readonly RequestDelegate _next;

        public ShopTenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TenantContext tenantContext, ShopContext shopContext)
        {
            var host = context.Request.Host.ToString();
            var shop = await shopContext.Shops.FirstOrDefaultAsync(s => s.Domain == host);

            if (shop == null)
                throw new NotFoundException();

            tenantContext.CurrentShop = shop;
            await _next(context);
        }
    }
}
