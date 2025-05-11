using Microsoft.AspNetCore.Mvc;
using Common.Paginations.Models;
using Common;
using System.Text.Json;
using Admin.Application.Services.Shops;
using Admin.Domain.Entities;
using Admin.Infrastructure;

namespace Admin.Api.Controllers
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
        public async Task<Result<ShopConfig>> AddAsync(ShopConfig shop)
        {
            var newShop = await _shopsService.AddAsync(shop);
            if (newShop is null)
                return Result<ShopConfig>.Fail("Failed to create shop.");

            #region Shop dbContext migration
            try
            {
                await _shopContext.ApplyDbMigrationsAsync(_configuration);
            }
            catch (Exception ex)
            {
                return Result<ShopConfig>.Fail($"Shop yaratildi lekin baza yaratilmay qoldi. \n(shop: {JsonSerializer.Serialize(newShop)}).");
            }
            #endregion

            return Result<ShopConfig>.Success(newShop);
        }

        [HttpGet]
        public async Task<Result<List<ShopConfig>>> GetAllAsync()
        {
            return Result<List<ShopConfig>>.Success(await _shopsService.GetAllAsync());
        }

        [HttpPost("filter")]
        public async Task<Result<List<ShopConfig>>> FilterAsync(PaginationOptions filter)
        {
            return Result<List<ShopConfig>>.Success(await _shopsService.FilterAsync(filter));
        }

        [HttpGet("{id}")]
        public async Task<Result<ShopConfig>> GetByIdAsync(int id)
        {
            return Result<ShopConfig>.Success(await _shopsService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<Result<ShopConfig>> UpdateAsync(int id, ShopConfig shop)
        {
            return Result<ShopConfig>.Success(await _shopsService.UpdateAsync(id, shop));
        }

        [HttpDelete("{id}")]
        public async Task<Result<ShopConfig>> DeleteAsync(int id)
        {
            return Result<ShopConfig>.Success(await _shopsService.DeleteAsync(id));
        }
    }
}