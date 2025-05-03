using Marketplace.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly TenantContext _tenantContext;

        public ConfigController(TenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        [HttpGet]
        public IActionResult GetConfig()
        {
            var schema = _tenantContext?.CurrentShop?.Schema;

            if (string.IsNullOrEmpty(schema))
            {
                return BadRequest(new { message = "Schema not found." });
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "configs", $"{schema}.json");

            if (!System.IO.File.Exists(path))
            {
                return NotFound(new { message = "Config file not found." });
            }

            var configContent = System.IO.File.ReadAllText(path);
            return Ok(configContent);
        }
    }
}
