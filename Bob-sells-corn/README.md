# Bob-Sells-Corn API

A corn purchasing API with JWT authentication and per-client rate limiting.

## Overview

| Aspect | Detail |
|--------|--------|
| **Framework** | ASP.NET Core 10.0 |
| **Database** | Entity Framework Core InMemory |
| **Authentication** | JWT Bearer Tokens |
| **Base URL** | `https://localhost:5180` |
| **Documentation** | Scalar UI at `/scalar` |

## Endpoints

### 1. POST `/api/auth/login`

Login or create a new client. Returns JWT token.

**Request:**
```json
{
  "clientName": "Bob"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "clientId": "11111111-1111-1111-1111-111111111111",
  "clientName": "Bob",
  "expiresAt": "2026-04-15T11:30:00Z"
}
```

**Behavior:**
- If client exists → returns existing client
- If client doesn't exist → creates new client automatically
- Token expires in 30 minutes

---

### 2. POST `/api/clients/buy-corn`

Purchase corn (rate limited to 1 per minute per client).

**Headers:**
```
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "success": true,
  "totalCornPurchased": 5
}
```

**Response (429 Too Many Requests):**
```json
"You exceeded the limit. Please wait before purchasing again."
```

---

## Data Models

### Client

| Field | Type | Description |
|-------|------|-------------|
| `Id` | `Guid` | Primary key (auto-generated) |
| `Name` | `string` | Client name |
| `LastPurchaseDate` | `DateTime?` | Last corn purchase timestamp |
| `TotalCornPurchased` | `decimal` | Total corn purchased |

### Purchase

| Field | Type | Description |
|-------|------|-------------|
| `Id` | `int` | Primary key (auto-increment) |
| `ClientId` | `Guid` | Foreign key to Client |
| `Timestamp` | `DateTime` | Purchase timestamp (UTC) |
| `Quantity` | `decimal` | Amount purchased (always 1) |

---

## Seed Data

| Name | GUID |
|------|------|
| Bob | `11111111-1111-1111-1111-111111111111` |
| Alice | `22222222-2222-2222-2222-222222222222` |
| Charlie | `33333333-3333-3333-3333-333333333333` |

---

## Rate Limiting

| Rule | Value |
|------|-------|
| Limit | 1 purchase per client |
| Window | 1 minute |
| Scope | Per client (GUID) |
| Reset | On app restart |

---

## JWT Token Structure

**Claims:**

| Claim | Description |
|-------|-------------|
| `nameidentifier` | Client ID (Guid) |
| `name` | Client name |
| `jti` | Unique token ID |

---

## Project Structure

```
Bob-sells-corn/
├── Controllers/
│   ├── AuthController.cs      # Login endpoint
│   └── ClientController.cs    # Purchase endpoint
├── Services/
│   ├── ClientService.cs       # Business logic
│   └── RateLimiterService.cs  # Rate limiting (IMemoryCache)
├── Models/
│   ├── Client.cs              # Client entity
│   └── Purchase.cs            # Purchase entity
├── Data/
│   ├── AppDbContext.cs        # EF Core context + seed data
│   └── DTOs/LoginRequest.cs   # Login request model
├── Extensions/
│   └── ClaimsPrincipalExtension.cs  # JWT claim extraction
├── Program.cs                 # DI, JWT config
└── appsettings.json           # JWT settings
```

---

## Configuration

**appsettings.json:**

```json
{
  "Jwt": {
    "Key": "FSDEFGTLKJADWEI1524F1244G8567UY**!632555",
    "Issuer": "cesarcarcamo.com",
    "Audience": "cesarcarcamo.com"
  }
}
```

---

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     Controllers                              │
│  ┌─────────────────┐    ┌─────────────────────────────┐    │
│  │ AuthController  │    │   ClientController          │    │
│  │ POST /login     │    │   POST /buy-corn            │    │
│  └────────┬────────┘    └──────────────┬──────────────┘    │
└───────────┼─────────────────────────────┼───────────────────┘
            │                             │
            ▼                             ▼
┌─────────────────────────────────────────────────────────────┐
│  ┌─────────────────┐    ┌─────────────────────────────┐     │
│  │ ClientService   │    │ RateLimiterService          │     │
│  │ - Login/Create  │    │ - IMemoryCache (1/min)      │     │
│  │ - Purchase      │    │                             │     │
│  └────────┬────────┘    └─────────────────────────────┘     │
└────────────┼─────────────────────────────────────────────────┘
             │
             ▼
┌─────────────────────────────────────────────────────────────┐
│  ┌─────────────────┐                                        │
│  │   AppDbContext   │                                        │
│  │ InMemoryDatabase │                                        │
│  └─────────────────┘                                        │
└─────────────────────────────────────────────────────────────┘
```
