using Common.ServiceAttribute;
using Marketplace.API;
using Marketplace.API.Middlewares;
using Marketplace.API.Models;
using Middlewares;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataContexts();

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

app.ApplyDbMigrations();

app.UseExceptionHandlingMiddleware();

app.UseMiddleware<ShopTenantMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
