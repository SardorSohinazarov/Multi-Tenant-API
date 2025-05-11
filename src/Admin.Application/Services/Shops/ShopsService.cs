using Microsoft.EntityFrameworkCore;
using Common.Paginations.Models;
using Common.Paginations.Extensions;
using Common.ServiceAttribute;
using Admin.Infrastructure;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Admin.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Admin.Application.Services.Shops
{
    [ScopedService]
    public class ShopsService : IShopsService
    {
        private readonly ShopContext _shopContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        public ShopsService(
            ShopContext shopContext,
            IMapper mapper,
            IHttpContextAccessor httpContext,
            IConfiguration configuration)
        {
            _shopContext = shopContext;
            _mapper = mapper;
            _httpContext = httpContext;
            _configuration = configuration;
        }

        public async Task<ShopConfig> AddAsync(ShopConfig shop)
        {
            var newConnectionString = string.IsNullOrWhiteSpace(shop.Schema) 
                    ? _configuration.GetConnectionString("ShopNewDb").Replace("{{NewDb}}", shop.Name) 
                    : shop.Schema;

            shop.Schema = newConnectionString;

            var entity = _mapper.Map<ShopConfig>(shop);
            var entry = await _shopContext.Set<ShopConfig>().AddAsync(entity);
            await _shopContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<List<ShopConfig>> GetAllAsync()
        {
            var entities = await _shopContext.Set<ShopConfig>().ToListAsync();
            return entities;
        }

        public async Task<List<ShopConfig>> FilterAsync(PaginationOptions filter)
        {
            var httpContext = _httpContext.HttpContext;
            var entities = await _shopContext.Set<ShopConfig>().ApplyPagination(filter, httpContext).ToListAsync();
            return entities;
        }

        public async Task<ShopConfig> GetByIdAsync(int id)
        {
            var entity = await _shopContext.Set<ShopConfig>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Shop with Id {id} not found.");
            return entity;
        }

        public async Task<ShopConfig> UpdateAsync(int id, ShopConfig shop)
        {
            var entity = await _shopContext.Set<ShopConfig>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Shop with {id} not found.");
            _mapper.Map(shop, entity);
            var entry = _shopContext.Set<ShopConfig>().Update(entity);
            await _shopContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<ShopConfig> DeleteAsync(int id)
        {
            var entity = await _shopContext.Set<ShopConfig>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Shop with {id} not found.");
            var entry = _shopContext.Set<ShopConfig>().Remove(entity);
            await _shopContext.SaveChangesAsync();
            return entry.Entity;
        }
    }
}