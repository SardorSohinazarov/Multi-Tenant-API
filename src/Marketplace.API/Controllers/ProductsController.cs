using Microsoft.AspNetCore.Mvc;
using Services.Products;
using Common.Paginations.Models;
using Common;
using Marketplace.API.Entities;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpPost]
        public async Task<Result<Product>> AddAsync(Product product)
        {
            return Result<Product>.Success(await _productsService.AddAsync(product));
        }

        [HttpGet]
        public async Task<Result<List<Product>>> GetAllAsync()
        {
            return Result<List<Product>>.Success(await _productsService.GetAllAsync());
        }

        [HttpPost("filter")]
        public async Task<Result<List<Product>>> FilterAsync(PaginationOptions filter)
        {
            return Result<List<Product>>.Success(await _productsService.FilterAsync(filter));
        }

        [HttpGet("{id}")]
        public async Task<Result<Product>> GetByIdAsync(int id)
        {
            return Result<Product>.Success(await _productsService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<Result<Product>> UpdateAsync(int id, Product product)
        {
            return Result<Product>.Success(await _productsService.UpdateAsync(id, product));
        }

        [HttpDelete("{id}")]
        public async Task<Result<Product>> DeleteAsync(int id)
        {
            return Result<Product>.Success(await _productsService.DeleteAsync(id));
        }
    }
}