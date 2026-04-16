# Bob-Sells-Corn API

A corn purchasing API with JWT authentication and per-client rate limiting.

## Overview

| Aspect | Detail |
|--------|--------|
| **Framework** | .NET 10.0 |
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
├── Bob-sells-corn/                    # API Project
│   ├── Controllers/
│   │   ├── AuthController.cs          # POST /api/auth/login
│   │   └── ClientController.cs       # POST /api/Client/buy-corn, GET /api/Client/get-corn-purchased
│   ├── Services/
│   │   ├── IClientService.cs          # Interface
│   │   ├── ClientService.cs           # Business logic
│   │   ├── IRateLimiterService.cs     # Interface
│   │   └── RateLimiterService.cs     # IMemoryCache rate limiting
│   ├── Models/
│   │   ├── Client.cs                  # Client entity
│   │   └── Purchase.cs                # Purchase entity
│   ├── Data/
│   │   ├── AppDbContext.cs            # EF Core InMemory context
│   │   └── DTOs/
│   │       └── LoginRequest.cs        # Login request model
│   ├── Extensions/
│   │   └── ClaimsPrincipalExtension.cs #JWT claim extraction
│   ├── Program.cs                      # DI, JWT config, CORS
│   ├── appsettings.json                # JWT settings
│   ├── README.md                      # API documentation
│   └── Bob-sells-corn.API.csproj
│
├── bob-sells-corn.client/             # Client Project (React 19 + Vite)
│   ├── src/
│   │   ├── context/
│   │   │   └── AuthContext.tsx        # Auth state (login, logout, client)
│   │   ├── pages/
│   │   │   ├── LoginPage.tsx         # Login form
│   │   │   └── DashboardPage.tsx     # Client dashboard/ buy corn
│   │   ├── services/
│   │   │   └── api.ts                # All API calls
│   │   ├── App.tsx                   # Routing
│   │   ├── main.tsx                  # Entry point
│   │   └── index.css                 # Tailwind CSS
│   ├── public/
│   │   └── favicon.svg
│   ├── .env                           # VITE_API_URL
│   ├── index.html
└── README.md                          # Documentation


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
│                     Controllers                             │
│  ┌─────────────────┐    ┌─────────────────────────────┐     │
│  │ AuthController  │    │   ClientController          │     │
│  │ POST /login     │    │   POST /buy-corn            │     │
│  └────────┬────────┘    └──────────────┬──────────────┘     │
└───────────┼────────────────────────────┼────────────────────┘
            │                            │
            ▼                            ▼
┌─────────────────────────────────────────────────────────────┐
│  ┌─────────────────┐    ┌─────────────────────────────┐     │
│  │ ClientService   │    │ RateLimiterService          │     │
│  │ - Login/Create  │    │ - IMemoryCache (1/min)      │     │
│  │ - Purchase      │    │                             │     │
│  └────────┬────────┘    └─────────────────────────────┘     │
└───────────┼─────────────────────────────────────────────────┘
            │
            ▼
┌─────────────────────────────────────────────────────────────┐
│  ┌─────────────────┐                                        │
│  │   AppDbContext  │                                        │
│  │ InMemoryDatabase│                                        │
│  └─────────────────┘                                        │
└─────────────────────────────────────────────────────────────┘
```

## Dataflows 
## login 
1. Enter Client Name  
2. Client Exists?   
2a.YES: Show Dashboard 
2b. NO: Create Client │ ShowDashboard
      
## buy corn 
1. Click "Buy Corn"                    
2. Get token from localStorage - corn_token  
3. POST /api/Client/buy-corn - Authorization: Bearer <token>         
4. Extract clientId from JWT claims
5. Check IMemoryCache for rate limit
6. RATE LIMITED?              
6a. YES: Show error429 Response  "Limit exceeded"
6B. NO: Create Purchase record│ Update client │ Record in   IMemoryCache │ 200 Response │ Update counter             

                    
