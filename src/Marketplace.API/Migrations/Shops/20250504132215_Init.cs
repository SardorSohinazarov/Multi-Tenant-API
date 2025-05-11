using Marketplace.API.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Marketplace.API.Migrations.Shops
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        private readonly IShopDbContext _shopDbContext;

        public Init(IShopDbContext shopDbContext) 
            => _shopDbContext = shopDbContext;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: _shopDbContext.Schema);

            migrationBuilder.CreateTable(
                name: "Products",
                schema: _shopDbContext.Schema,
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: _shopDbContext.Schema);
        }
    }
}
