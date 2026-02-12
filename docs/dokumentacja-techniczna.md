# Dokumentacja techniczna - ReLoop

## 1. Wykorzystane technologie i biblioteki

### Backend (.NET 10)

| Technologia | Opis |
|---|---|
| ASP.NET Core 10 (Minimal API) | Framework webowy, endpointy API |
| Entity Framework Core 10 | ORM do komunikacji z baza danych |
| Npgsql (PostgreSQL) | Provider bazy danych PostgreSQL |
| Microsoft Semantic Kernel | Integracja z AI (Google Gemini) do kategoryzacji przedmiotow |
| FluentValidation | Walidacja requestow |
| Swashbuckle (Swagger) | Dokumentacja API |
| JWT Bearer Authentication | Uwierzytelnianie uzytkownikow tokenami JWT |
| Scrutor | Automatyczna rejestracja serwisow w DI |
| MediatR (przez Shared.Abstractions) | Wzorzec CQRS - oddzielenie komend i zapytan |

### Frontend (Blazor WebAssembly)

| Technologia | Opis |
|---|---|
| Blazor WebAssembly (.NET 10) | SPA framework dzialajacy w przegladarce |
| Blazored.LocalStorage | Przechowywanie tokenow JWT w localStorage |
| System.IdentityModel.Tokens.Jwt | Parsowanie claimow z tokena JWT po stronie klienta |
| Microsoft.AspNetCore.Components.Authorization | Obsluga stanu autoryzacji w Blazor |

### Infrastruktura

| Technologia | Opis |
|---|---|
| Docker | Konteneryzacja aplikacji |
| Docker Compose | Orkiestracja kontenerow (PostgreSQL + API + Client) |
| Nginx | Serwer HTTP dla frontendu (w kontenerze) |
| PostgreSQL | Relacyjna baza danych |

### Narzedzia AI

Przy tworzeniu warstwy frontendowej (Blazor WebAssembly) korzystalem z Claude Code jako wsparcia przy stylowaniu CSS oraz tworzeniu struktury stron i komponentow Razor. Backend zostal napisany samodzielnie.

## 2. Architektura aplikacji

Projekt sklada sie z trzech glownych warstw zgodnie z zasadami Clean Architecture:

```
ReLoop/
├── client/
│   └── ReLoop.Client          # Blazor WebAssembly (frontend)
├── server/
│   ├── ReLoop.Api              # Warstwa prezentacji (endpointy HTTP)
│   ├── ReLoop.Application      # Warstwa aplikacji (CQRS - komendy i zapytania)
│   ├── ReLoop.Domain           # Warstwa domenowa (encje, logika biznesowa)
│   └── ReLoop.Infrastructure   # Warstwa infrastruktury (EF Core, AI, repozytoria)
├── shared/
│   ├── ReLoop.Shared.Abstractions  # Interfejsy i abstrakcje wspoldzielone
│   ├── ReLoop.Shared.Contracts     # Kontrakty DTO
│   └── ReLoop.Shared.Infrastructure # Wspolna infrastruktura (JWT, Postgres, Swagger)
└── docker-compose.yml
```

### Opis warstw

**ReLoop.Api** - Warstwa prezentacji. Definiuje endpointy HTTP korzystajac z Minimal API. Kazdy endpoint dziedziczy po `EndpointBase` i jest automatycznie mapowany przy starcie aplikacji.

**ReLoop.Application** - Warstwa aplikacji. Implementuje wzorzec CQRS z uzyciem MediatR. Komendy (Commands) zmieniaja stan aplikacji, zapytania (Queries) odczytuja dane. Kazda operacja ma osobny handler.

**ReLoop.Domain** - Warstwa domenowa. Zawiera encje (`User`, `Item`), value objects (`UserId`, `Email`, `Password`, `Name`), enumy (`Role`, `ItemStatus`, `ItemCategory`) oraz logike biznesowa (np. walidacja zakupu).

**ReLoop.Infrastructure** - Warstwa infrastruktury. Konfiguracja Entity Framework Core, repozytoria, integracja z Google Gemini AI do kategoryzacji przedmiotow, inicjalizacja bazy danych z danymi testowymi.

**ReLoop.Client** - Frontend w Blazor WebAssembly. Komunikuje sie z API przez HttpClient. Obsluguje autoryzacje przez JWT przechowywany w localStorage. Strony: Home (marketplace), Login, Register, Sell Item, My Items.

### Wzorce projektowe

- **CQRS** - rozdzielenie operacji odczytu i zapisu
- **Repository Pattern** - abstrakcja dostepu do danych
- **Unit of Work** - transakcyjnosc operacji bazodanowych
- **Value Objects** - hermetyzacja walidacji (Email, Password, Name)
- **Aggregate Root** - encje domenowe z kontrolowana modyfikacja stanu

## 3. API Endpoints

### Autoryzacja

| Metoda | Sciezka | Opis | Auth |
|---|---|---|---|
| POST | `/sign-up` | Rejestracja nowego uzytkownika | Nie |
| POST | `/sign-in` | Logowanie, zwraca token JWT | Nie |
| GET | `/me` | Dane zalogowanego uzytkownika (id, email, imie, nazwisko, saldo) | Tak |
| POST | `/me/balance?amount={kwota}` | Doladowanie salda konta | Tak |

### Przedmioty

| Metoda | Sciezka | Opis | Auth |
|---|---|---|---|
| GET | `/items` | Lista wszystkich dostepnych przedmiotow | Nie |
| GET | `/items/my` | Przedmioty wystawione przez zalogowanego uzytkownika | Tak |
| POST | `/items?name={}&description={}&price={}` | Wystawienie nowego przedmiotu (obraz w body jako multipart) | Tak |
| POST | `/items/{itemId}/buy` | Zakup przedmiotu | Tak |
| GET | `/items/{itemId}/image` | Pobranie obrazu przedmiotu (zwraca JPEG) | Nie |

### Przykladowe odpowiedzi

**POST /sign-in** - Logowanie:
```json
{
  "isSuccess": true,
  "message": null,
  "value": "eyJhbGciOiJIUzI1NiIs..."
}
```

**GET /items** - Lista przedmiotow:
```json
{
  "isSuccess": true,
  "value": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "Nike Jordan 4 Retro Thunder",
      "description": "Classic Jordan 4 sneakers...",
      "price": 850.00,
      "category": "Clothes",
      "status": "Active",
      "seller": { "firstName": "Jan", "lastName": "Kowalski" },
      "createdAt": "2026-02-12T14:22:32Z"
    }
  ]
}
```

## 4. Struktura bazy danych

Baza danych: **PostgreSQL**

### Tabela: Users

| Kolumna | Typ | Ograniczenia |
|---|---|---|
| Id | UUID | PK |
| Email | VARCHAR(100) | NOT NULL, UNIQUE INDEX |
| FirstName | VARCHAR(50) | NOT NULL |
| LastName | VARCHAR(50) | NOT NULL |
| Password | VARCHAR(200) | NOT NULL (hashowane) |
| Role | TEXT | NOT NULL (User / Admin) |
| Balance | DECIMAL | NOT NULL, domyslnie 500 |
| IsDeleted | BOOLEAN | NOT NULL |

### Tabela: Items

| Kolumna | Typ | Ograniczenia |
|---|---|---|
| Id | UUID | PK |
| Name | VARCHAR(200) | NOT NULL |
| Description | VARCHAR(2000) | NOT NULL |
| ImageData | BYTEA | NOT NULL |
| Price | DECIMAL(18,2) | NOT NULL |
| Category | TEXT | NOT NULL, INDEX (Other, Clothes, Electronics, Books, Sports, ...) |
| Status | TEXT | NOT NULL, INDEX (Active / Sold) |
| SellerId | UUID | NOT NULL, FK -> Users(Id), INDEX, ON DELETE RESTRICT |
| BuyerId | UUID | NULL, FK -> Users(Id), ON DELETE RESTRICT |
| CreatedAt | TIMESTAMP | NOT NULL |
| SoldAt | TIMESTAMP | NULL |
| IsDeleted | BOOLEAN | NOT NULL |

### Relacje

```
Users (1) ──── (*) Items (SellerId)   - Uzytkownik moze wystawic wiele przedmiotow
Users (1) ──── (*) Items (BuyerId)    - Uzytkownik moze kupic wiele przedmiotow
```

Obie relacje maja `ON DELETE RESTRICT` - nie mozna usunac uzytkownika ktory ma powiazane przedmioty.

### Diagram

```
┌──────────────┐         ┌──────────────────┐
│    Users     │         │      Items       │
├──────────────┤         ├──────────────────┤
│ Id (PK)      │◄───┐    │ Id (PK)          │
│ Email        │    ├────│ SellerId (FK)    │
│ FirstName    │    │    │ BuyerId (FK)     │
│ LastName     │    └────│                  │
│ Password     │         │ Name             │
│ Role         │         │ Description      │
│ Balance      │         │ ImageData        │
│ IsDeleted    │         │ Price            │
└──────────────┘         │ Category         │
                         │ Status           │
                         │ CreatedAt        │
                         │ SoldAt           │
                         │ IsDeleted        │
                         └──────────────────┘
```
