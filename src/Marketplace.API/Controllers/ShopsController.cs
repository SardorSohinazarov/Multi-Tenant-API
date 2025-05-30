using Microsoft.AspNetCore.Mvc;
using Services.Shops;
using Common.Paginations.Models;
using Common;
using Marketplace.API;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Marketplace.API.Entities;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopsService _shopsService;
        private readonly IConfiguration _configuration;
        private readonly ShopContext _shopContext;
        public ShopsController(IShopsService shopsService, IConfiguration configuration, ShopContext shopContext)
        {
            _shopsService = shopsService;
            _configuration = configuration;
            _shopContext = shopContext;
        }

        [HttpPost]
        public async Task<Result<Shop>> AddAsync(Shop shop)
        {
            var newShop = await _shopsService.AddAsync(shop);
            if(newShop == null)
                return Result<Shop>.Fail("Failed to create shop.");

            // Migrate
            #region Shop dbContext migration
            try
            {
                await _shopContext.ApplyDbMigrationsAsync(_configuration);
            }
            catch (Exception ex)
            {
                throw new Exception($"Shop yaratildi lekin baza yaratilmay qoldi. \n(shop: {JsonSerializer.Serialize(newShop)}).");
            }
            #endregion

            return Result<Shop>.Success(newShop);
        }

        [HttpGet]
        public async Task<Result<List<Shop>>> GetAllAsync()
        {
            return Result<List<Shop>>.Success(await _shopsService.GetAllAsync());
        }

        [HttpPost("filter")]
        public async Task<Result<List<Shop>>> FilterAsync(PaginationOptions filter)
        {
            return Result<List<Shop>>.Success(await _shopsService.FilterAsync(filter));
        }

        [HttpGet("{id}")]
        public async Task<Result<Shop>> GetByIdAsync(int id)
        {
            return Result<Shop>.Success(await _shopsService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<Result<Shop>> UpdateAsync(int id, Shop shop)
        {
            return Result<Shop>.Success(await _shopsService.UpdateAsync(id, shop));
        }

        [HttpDelete("{id}")]
        public async Task<Result<Shop>> DeleteAsync(int id)
        {
            return Result<Shop>.Success(await _shopsService.DeleteAsync(id));
        }
    }
}