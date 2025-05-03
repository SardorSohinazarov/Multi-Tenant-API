using Common.Paginations.Models;
using Shop.Domain.Entities;

namespace Shop.Application.Services.Products
{
    public interface IProductsService
    {
        Task<Product> AddAsync(Product entity);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> FilterAsync(PaginationOptions filter);
        Task<Product> GetByIdAsync(int id);
        Task<Product> UpdateAsync(int id, Product entity);
        Task<Product> DeleteAsync(int id);
    }
}