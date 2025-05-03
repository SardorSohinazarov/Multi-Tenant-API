using Common.Paginations.Models;
using Admin.Domain.Entities;

namespace Admin.Application.Services.Shops
{
    public interface IShopsService
    {
        Task<ShopConfig> AddAsync(ShopConfig entity);
        Task<List<ShopConfig>> GetAllAsync();
        Task<List<ShopConfig>> FilterAsync(PaginationOptions filter);
        Task<ShopConfig> GetByIdAsync(int id);
        Task<ShopConfig> UpdateAsync(int id, ShopConfig entity);
        Task<ShopConfig> DeleteAsync(int id);
    }
}