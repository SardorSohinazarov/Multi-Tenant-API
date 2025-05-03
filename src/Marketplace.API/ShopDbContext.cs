using Marketplace.API.Entities;
using Marketplace.API.Exceptions;
using Marketplace.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace Marketplace.API
{
    public class ShopDbContext : DbContext
    {
        private TenantContext? _tenantContext;
        private readonly string? _schema;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IServiceProvider? _serviceProvider;

        public ShopDbContext(string schema, string connectionString)
        {
            _schema = schema;
            _connectionString = connectionString;
        }

        public ShopDbContext(IConfiguration configuration, IServiceProvider? serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;

            if (_serviceProvider != null)
            {
                _tenantContext = _serviceProvider.GetService<TenantContext>();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("MasterDb") ?? _connectionString;
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schema = _tenantContext?.CurrentShop.Schema ?? _schema ?? throw new NotFoundException();
            modelBuilder.HasDefaultSchema(schema);

            base.OnModelCreating(modelBuilder);
        }

        public async Task AddSchemaAsync(string schema)
        {
            var connectionString = _configuration.GetConnectionString("MasterDb") ?? _connectionString;

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            var commandText = $"CREATE SCHEMA IF NOT EXISTS \"{schema}\";";

            await using var command = new NpgsqlCommand(commandText, connection);
            await command.ExecuteNonQueryAsync();
        }

        public DbSet<Product> Products { get; set; }
    }

    public class ShopDbContextFactory : IDesignTimeDbContextFactory<ShopDbContext>
    {
        public ShopDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("SardorDb"));

            // Design-time uchun serviceProvider va tenantContext kerak emas
            return new ShopDbContext(configuration, null);
        }
    }
}
