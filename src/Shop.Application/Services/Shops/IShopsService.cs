using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Common.Paginations.Models;
using Common.Paginations.Extensions;
using Marketplace.API.Entities;

namespace Services.Shops
{
    public interface IShopsService
    {
        Task<Shop> AddAsync(Shop entity);
        Task<List<Shop>> GetAllAsync();
        Task<List<Shop>> FilterAsync(PaginationOptions filter);
        Task<Shop> GetByIdAsync(int id);
        Task<Shop> UpdateAsync(int id, Shop entity);
        Task<Shop> DeleteAsync(int id);
    }
}