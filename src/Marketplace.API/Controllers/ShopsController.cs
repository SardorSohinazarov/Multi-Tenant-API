using Microsoft.AspNetCore.Mvc;
using Services.Shops;
using Common.Paginations.Models;
using Common;
using Marketplace.API.Models;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopsService _shopsService;
        public ShopsController(IShopsService shopsService)
        {
            _shopsService = shopsService;
        }

        [HttpPost]
        public async Task<Result<Shop>> AddAsync(Shop shop)
        {
            return Result<Shop>.Success(await _shopsService.AddAsync(shop));
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