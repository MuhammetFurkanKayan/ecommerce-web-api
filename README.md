# E-Commerce Web API

A RESTful E-Commerce backend API built with **ASP.NET Core (.NET 9)** and **Entity Framework Core**, featuring JWT authentication, role based authorization, catalog management, and controlled order lifecycle workflows.

---

## Features

- **Authentication & Authorization**: User registration and login with **JWT Bearer** tokens, secure PBKDF2 password hashing via `PasswordHasher<User>`, and role based access control (`Customer` & `Admin`).
- **User Account Management**: Self service profile endpoints (`/api/user/me`), partial profile updates (`PATCH`), email and password changes verified via current password, and admin user management.
- **Product & Category Catalog**: Public browsing endpoints for categories and products, with write operations (`POST`, `PUT`, `PATCH`, `DELETE`) restricted to `Admin` users.
- **Order Processing & Business Rules**:
  - Automatic total calculation, unit price snapshotting, and real time stock deduction upon order creation.
  - Shipping address snapshotting from the user profile at checkout, with updates allowed while the order is `Pending` or `Processing`.
  - Strict state machine transitions (`Pending` → `Processing` → `Shipped` → `Delivered`).
  - Dedicated order cancellation workflow (`Pending` → `Cancelled`) that automatically restores product stock and records `CancelledAt`.
  - Resource ownership enforcement ensuring customers can only view, update, or cancel their own orders.
- **Referential Data Integrity**: Configured with `DeleteBehavior.Restrict` and service level checks (`409 Conflict`) to preserve order history and prevent deleting categories with products, products used in orders, or users with existing orders.
- **Swagger / OpenAPI**: Interactive API documentation configured with JWT Bearer authorization support.

---

## Tech Stack

- **Framework**: .NET 9 (`ASP.NET Core Web API`)
- **ORM & Database**: Entity Framework Core 9, Microsoft SQL Server (Code First Migrations)
- **Security**: `Microsoft.AspNetCore.Authentication.JwtBearer`, `Microsoft.Extensions.Identity.Core`
- **Documentation**: `Swashbuckle.AspNetCore` (Swagger UI)
- **Architecture**: Layered Architecture (`Controllers` → `Services` → `Generic Repository` → `AppDbContext`)

---

## Project Structure

```text
EcommerceWebApi/
├── Context/        # EF Core AppDbContext, relationships, and indexes
├── Controllers/    # REST API controllers and HTTP status mapping
├── DTOs/           # Request and response Data Transfer Objects
├── Enums/          # Domain enums (OrderStatus, UserRole)
├── Interfaces/     # Repository and service abstractions
├── Migrations/     # EF Core database schema migrations
├── Models/         # Domain entities (User, Category, Product, Order, OrderItem)
├── Repository/     # Generic repository implementation
├── Services/       # Business logic, validation, and authorization rules
└── Program.cs      # DI registration, JWT setup, Swagger config, and middleware pipeline
```

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Microsoft SQL Server (or SQL Server Express)
- EF Core CLI tools (`dotnet tool install --global dotnet-ef`)

### 1. Clone the Repository

```bash
git clone https://github.com/MuhammetFurkanKayan/ecommerce-web-api.git
cd ecommerce-web-api/EcommerceWebApi
```

### 2. Configure Database & JWT Settings

Update the connection string and JWT configuration in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=EcommerceDb;Trusted_Connection=true;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "replace_with_a_strong_secret_key_at_least_32_chars!",
    "Issuer": "EcommerceWebApi",
    "Audience": "EcommerceWebUI",
    "ExpireMinutes": 60
  }
}
```

### 3. Apply Database Migrations

```bash
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

Once running in `Development` mode, open `/swagger` in your browser to explore and test the API. Click the **Authorize** button and paste the JWT token returned from `/api/auth/login`.

### Promoting a User to Admin

Newly registered accounts are assigned the `Customer` role by default. To test `Admin` only endpoints, promote a registered user in SQL Server and **log in again** to obtain a fresh JWT with the updated role claim:

```sql
UPDATE Users
SET Role = 'Admin'
WHERE Email = 'your-email@example.com';
```

---

## API Endpoints

### Auth (`/api/auth`)

| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/auth/register` | Public | Register a new customer account |
| `POST` | `/api/auth/login` | Public | Authenticate and receive a JWT token |

### Users (`/api/user`)

| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/user/me` | Authenticated | Get current user's profile |
| `PATCH` | `/api/user/me` | Authenticated | Partially update current user's profile (`UserName`, `Age`, `Address`) |
| `POST` | `/api/user/me/change-password` | Authenticated | Change password (requires current password) |
| `POST` | `/api/user/me/change-email` | Authenticated | Change email address (requires current password) |
| `DELETE` | `/api/user/me` | Authenticated | Delete current user's account (`409` if user has orders) |
| `GET` | `/api/user` | Admin | List all users |
| `GET` | `/api/user/{id}` | Admin | Get a specific user by ID |
| `DELETE` | `/api/user/{id}` | Admin | Delete a user by ID (`409` if user has orders) |

### Categories (`/api/category`)

| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/category` | Public | List all categories |
| `GET` | `/api/category/{id}` | Public | Get category by ID |
| `POST` | `/api/category` | Admin | Create a new category |
| `PUT` | `/api/category/{id}` | Admin | Replace a category |
| `PATCH` | `/api/category/{id}` | Admin | Partially update a category |
| `DELETE` | `/api/category/{id}` | Admin | Delete a category (`409` if category has products) |

### Products (`/api/product`)

| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/product` | Public | List all products |
| `GET` | `/api/product/{id}` | Public | Get product by ID |
| `POST` | `/api/product` | Admin | Create a new product |
| `PUT` | `/api/product/{id}` | Admin | Replace a product |
| `PATCH` | `/api/product/{id}` | Admin | Partially update a product |
| `DELETE` | `/api/product/{id}` | Admin | Delete a product (`409` if product is used in orders) |

### Orders (`/api/order`)

| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/order` | Authenticated | Create a new order for the authenticated user |
| `GET` | `/api/order/me` | Authenticated | List current user's orders (includes `OrderItems`) |
| `GET` | `/api/order` | Admin | List all orders in the system (includes `OrderItems`) |
| `GET` | `/api/order/{id}` | Owner / Admin | Get order details by ID |
| `PATCH` | `/api/order/{id}/status` | Admin | Advance order status (`Pending` → `Processing` → `Shipped` → `Delivered`) |
| `POST` | `/api/order/{id}/cancel` | Owner / Admin | Cancel a `Pending` order and restore product stock |
| `PATCH` | `/api/order/{id}/shipping-address` | Owner / Admin | Update shipping address (`Pending` or `Processing` orders only) |
