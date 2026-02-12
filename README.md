# ReLoop

Aplikacja marketplace do kupowania i sprzedawania przedmiotow z drugiej reki. Projekt wykorzystuje AI do automatycznej kategoryzacji wystawianych przedmiotow.

## Wymagania

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Instalacja i uruchomienie

1. Sklonuj repozytorium:
```bash
git clone https://github.com/reuther03/ReLoop.git
cd ReLoop
```

2. Uruchom aplikacje:
```bash
docker compose up -d
```

3. Poczekaj ok. 30 sekund az wszystkie kontenery sie uruchomia, nastepnie otworz przegladarke:
   - **Frontend**: http://localhost:3000
   - **API (Swagger)**: http://localhost:5005/swagger

4. Aby zatrzymac:
```bash
docker compose down
```

## Konta testowe

Aplikacja automatycznie seeduje baze danych z przykladowymi uzytkownikami i przedmiotami.

| Email | Haslo | Rola |
|---|---|---|
| admin@reloop.com | Admin123! | Admin |
| jan.kowalski@email.com | User123! | User |
| anna.nowak@email.com | User123! | User |

## Uruchomienie lokalne (bez Dockera)

Jesli chcesz uruchomic projekt bez Dockera:

1. Zainstaluj [.NET 10 SDK](https://dotnet.microsoft.com/download)
2. Uruchom PostgreSQL na porcie 5434 (np. przez `docker compose up reloop-postgres -d`)
3. Ustaw user-secrets dla projektu API:
```bash
cd server/ReLoop.Api
dotnet user-secrets set "postgres:connectionString" "Host=localhost;Port=5434;Database=ReLoop;Username=postgres;Password="
dotnet user-secrets set "jwt:issuer" "ReLoop"
dotnet user-secrets set "jwt:audience" "ReLoop"
dotnet user-secrets set "jwt:SecretKey" "27D719E2-0077-4574-ACFC-063304D71D69"
dotnet user-secrets set "jwt:expiry" "12:00:00"
dotnet user-secrets set "llm:gemini:apiKey" "<twoj-klucz-gemini>"
dotnet user-secrets set "llm:gemini:model" "gemini-2.5-flash-lite"
```
4. Uruchom backend:
```bash
dotnet run --project server/ReLoop.Api
```
5. Uruchom frontend:
```bash
dotnet run --project client/ReLoop.Client
```
- Backend: http://localhost:5005
- Frontend: http://localhost:5151
