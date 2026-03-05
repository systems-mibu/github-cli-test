# GithubCliTest API

Minimal ASP.NET Core Web API with in-memory CRUD endpoints for Products, Customers, and Orders.

## Run

```powershell
dotnet run --project .\GithubCliTest.Api.csproj
```

API base URL is the local URL printed by ASP.NET Core at startup (for example `http://localhost:5000`).

## Test

```powershell
dotnet test .\GithubCliTest.slnx
```

## API

All controllers are routed as `api/[controller]`.

### Products

Payload shape (`Product`):

```json
{
  "id": 1,
  "name": "Keyboard",
  "price": 49.99
}
```

- `GET /api/products`  
  Returns `200 OK` with `Product[]`.
- `GET /api/products/{id}`  
  Returns `200 OK` with `Product` when found, otherwise `404 Not Found`.
- `POST /api/products`  
  Request body: `{ "name": "Keyboard", "price": 49.99 }`  
  Returns `201 Created` with created `Product` (`id` assigned by server) and `Location` header.
- `PUT /api/products/{id}`  
  Request body: `{ "name": "Gaming Keyboard", "price": 79.99 }`  
  Returns `204 No Content` when updated, otherwise `404 Not Found`.
- `DELETE /api/products/{id}`  
  Returns `204 No Content` when deleted, otherwise `404 Not Found`.

### Customers

Payload shape (`Customer`):

```json
{
  "id": 1,
  "name": "Alice"
}
```

- `GET /api/customers`  
  Returns `200 OK` with `Customer[]`.
- `GET /api/customers/{id}`  
  Returns `200 OK` with `Customer` when found, otherwise `404 Not Found`.
- `POST /api/customers`  
  Request body: `{ "name": "Alice" }`  
  Returns `201 Created` with created `Customer` (`id` assigned by server) and `Location` header.
- `PUT /api/customers/{id}`  
  Request body: `{ "name": "Alice Updated" }`  
  Returns `204 No Content` when updated, otherwise `404 Not Found`.
- `DELETE /api/customers/{id}`  
  Returns `204 No Content` when deleted, otherwise `404 Not Found`.

### Orders

Payload shape (`Order`):

```json
{
  "id": 1,
  "customerName": "Alice",
  "total": 25.0
}
```

- `GET /api/orders`  
  Returns `200 OK` with `Order[]`.
- `GET /api/orders/{id}`  
  Returns `200 OK` with `Order` when found, otherwise `404 Not Found`.
- `POST /api/orders`  
  Request body: `{ "customerName": "Alice", "total": 25.0 }`  
  Returns `201 Created` with created `Order` (`id` assigned by server) and `Location` header.
- `PUT /api/orders/{id}`  
  Request body: `{ "customerName": "Alice Updated", "total": 30.0 }`  
  Returns `204 No Content` when updated, otherwise `404 Not Found`.
- `DELETE /api/orders/{id}`  
  Returns `204 No Content` when deleted, otherwise `404 Not Found`.
