using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Common.Paginations.Models;
using Common.Paginations.Extensions;
using Marketplace.API.Entities;

namespace Services.Products
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