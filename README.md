# 🏪 Multi-Tenant API (Schema-Based)

Bu loyiha PostgreSQL ma'lumotlar bazasida **multi-tenant (ko‘p mijozli)** arxitekturani amalga oshirgan `ASP.NET Core Web API` dasturidir. Har bir tenant (do‘kon yoki mijoz) uchun **alohida schema** yaratiladi, bu esa ma’lumotlarni izolyatsiyalash va xavfsizlikni ta’minlaydi.

---

## 📦 Texnologiyalar

- [.NET 8](https://dotnet.microsoft.com/en-us/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL](https://www.postgresql.org/)
- [Npgsql EF Core Provider](https://www.npgsql.org/efcore/index.html)
- Dependency Injection, Middleware, Scoped Tenant Context
- Dynamic Schema Migrations

---

## 🧠 Schema based Multi-Tenancy yondashuvi

- Har bir tenant uchun alohida **PostgreSQL schema** yaratiladi: `shop1`, `shop2`, ...
- Migrationlarni har biri default holatda `public` schema bilan ishlaydi yani Entity framework orqali bu hal qilinmagan muammo. Shuning uchun EF Core migratsiyasi `IMigrationsAssembly` orqali override qilinib, `schema` parametrli konstruktor qo‘llab-quvvatlanadi.
- Admin databaseda Shops tableda Shoplarni malumotlari `ShopContext` orqali saqlaydi.
- Shop databaseda har bir tenant o‘z `Products` jadvaliga ega, umumiy `ShopDbContext` orqali ishlaydi.

---

## 🚀 Quick Start

### 🛒 [Marketplace.API](https://github.com/SardorSohinazarov/Multi-Tenant-API/tree/master/src/Marketplace.API)

- Sodda va tushunarli holda service, controller, dbcontext sozlamalari va bir nechta extensionlar bor.
- `ShopTenantMiddleware`, `IMigrationsAssembly` kabi interfeys va implementatsiyalar mavjud.
- Shops va Products controllerlari orqali hammasi boshqariladi.
- Shops controller - (admin o'rnida) shop yaratish orqali automatic database schema ham yaratadi.
- Marketplace controller - productlarni crud qilish uchun ishlaydi.

---

## 📂 Yanayam tushunarli holda

### 👤 [Admin.Api](https://github.com/SardorSohinazarov/Multi-Tenant-API/tree/master/src/Admin.Api)

- Tenantlarni yaratish, ro'yxatdan o'tkazish va ularga schema ajratib tablelarni yaratish imkonini beradi.

### 🛒 [Shop.Api](https://github.com/SardorSohinazarov/Multi-Tenant-API/tree/master/src/Shop.Api/Shop.Api)

- Har bir tenant (ya'ni do‘kon) uchun izolyatsiyalangan REST API.
- Domain orqali qaysi schema bilan ishlash aniqlanadi.
- CRUD operatsiyalar: mahsulotlar (`Products`) va boshqa resurslar.
- Har bir tenant faqat o‘z schema’sidagi ma’lumotlarga ega va boshqa tenantlar bilan ma’lumotlar almashilmaydi.

---

## 🛠 Ishga tushirish

1. **Repository klon qilish**  
```bash
git clone https://github.dev/SardorSohinazarov/Multi-Tenant-API
cd Multi-Tenant-API
