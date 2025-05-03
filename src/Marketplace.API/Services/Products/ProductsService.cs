using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Common.Paginations.Models;
using Common.Paginations.Extensions;
using Common.ServiceAttribute;
using Marketplace.API;
using Marketplace.API.Entities;

namespace Services.Products
{
    [ScopedService]
    public class ProductsService : IProductsService
    {
        private readonly ShopDbContext _shopDbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public ProductsService(ShopDbContext shopDbContext, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _shopDbContext = shopDbContext;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<Product> AddAsync(Product product)
        {
            var entity = _mapper.Map<Product>(product);
            var entry = await _shopDbContext.Set<Product>().AddAsync(entity);
            await _shopDbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var entities = await _shopDbContext.Set<Product>().ToListAsync();
            return entities;
        }

        public async Task<List<Product>> FilterAsync(PaginationOptions filter)
        {
            var httpContext = _httpContext.HttpContext;
            var entities = await _shopDbContext.Set<Product>().ApplyPagination(filter, httpContext).ToListAsync();
            return entities;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var entity = await _shopDbContext.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Product with Id {id} not found.");
            return entity;
        }

        public async Task<Product> UpdateAsync(int id, Product product)
        {
            var entity = await _shopDbContext.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Product with {id} not found.");
            _mapper.Map(product, entity);
            var entry = _shopDbContext.Set<Product>().Update(entity);
            await _shopDbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var entity = await _shopDbContext.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new InvalidOperationException($"Product with {id} not found.");
            var entry = _shopDbContext.Set<Product>().Remove(entity);
            await _shopDbContext.SaveChangesAsync();
            return entry.Entity;
        }
    }
}