using Common.ServiceAttribute;
using Marketplace.API;
using Marketplace.API.Middlewares;
using Marketplace.API.Models;
using Microsoft.EntityFrameworkCore;
using Middlewares;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // Database create qilish

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ShopContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("MasterDb")));

        builder.Services.AddDbContext<ShopDbContext>();

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCustomServices();

        builder.Services.AddScoped<TenantContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandlingMiddleware();
        app.UseMiddleware<ShopMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}