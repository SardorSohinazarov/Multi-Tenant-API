using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Common.Paginations.Models;
using Common.Paginations.Extensions;
using Common.ServiceAttribute;
using Marketplace.API;
using Marketplace.API.Models;

namespace Services.Shops
{
    [ScopedService]
    public class ShopsService : IShopsService
    {
        private readonly ShopContext _shopContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public ShopsService(ShopContext shopContext, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _shopContext = shopContext;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<Shop> AddAsync(Shop shop)
        {
            var entity = _mapper.Map<Shop>(shop);
            var entry = await _shopContext.Set<Shop>().AddAsync(entity);
            await _shopContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<List<Shop>> GetAllAsync()
        {
            var entities = await _shopContext.Set<Shop>().ToListAsync();
            return entities;
        }

        public async Task<List<Shop>> FilterAsync(PaginationOptions filter)
        {
            var httpContext = _httpContext.HttpContext;
            var entities = await _shopContext.Set<Shop>().ApplyPagination(filter, httpContext).ToListAsync();
            return entities;
        }

        public async Task<Shop> GetByIdAsync(int id)
        {
            var entity = await _shopContext.Set<Shop>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Shop with Id {id} not found.");
            return entity;
        }

        public async Task<Shop> UpdateAsync(int id, Shop shop)
        {
            var entity = await _shopContext.Set<Shop>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Shop with {id} not found.");
            _mapper.Map(shop, entity);
            var entry = _shopContext.Set<Shop>().Update(entity);
            await _shopContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Shop> DeleteAsync(int id)
        {
            var entity = await _shopContext.Set<Shop>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Shop with {id} not found.");
            var entry = _shopContext.Set<Shop>().Remove(entity);
            await _shopContext.SaveChangesAsync();
            return entry.Entity;
        }
    }
}