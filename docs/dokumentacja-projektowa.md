# Dokumentacja procesu powstawania projektu - ReLoop

## 1. Harmonogram

Projekt realizowany jednoosobowo w okresie 11-12 lutego 2026.

| Etap | Termin | Opis |
|---|---|---|
| Inicjalizacja projektu | 11.02 | Utworzenie repozytorium, struktura solucji, konfiguracja projektow |
| Konfiguracja bazy danych | 12.02 | PostgreSQL w Docker, EF Core, migracje, konfiguracja encji |
| Warstwa domenowa | 12.02 | Encje User i Item, value objects, enumy, logika biznesowa |
| Endpointy API | 12.02 | Rejestracja, logowanie, CRUD przedmiotow, zakup, obsluga obrazow |
| Integracja AI | 12.02 | Polaczenie z Google Gemini do automatycznej kategoryzacji przedmiotow |
| Seeding danych | 12.02 | Przykladowi uzytkownicy i przedmioty z obrazami |
| Frontend - layout | 12.02 | Nawigacja, layout strony, globalny CSS, motyw graficzny |
| Frontend - strony | 12.02 | Home, Login, Register, Sell Item, My Items |
| Frontend - autoryzacja | 12.02 | JWT w localStorage, AuthenticationStateProvider, stan zalogowania |
| Dockeryzacja | 12.02 | Dockerfiles dla API i klienta, docker-compose z 3 kontenerami |
| Dokumentacja | 12.02 | README, dokumentacja techniczna, dokumentacja uzytkownika |

## 2. Planowanie i estymacja zadan

### Podejscie

Projekt realizowany iteracyjnie - najpierw backend (API + baza danych), potem frontend (Blazor WASM), na koniec konteneryzacja i dokumentacja. Kazdy etap konczyl sie dzialajacym przyrostem funkcjonalnosci.

### Estymacja vs rzeczywistosc

| Zadanie | Estymacja | Rzeczywisty czas | Uwagi |
|---|---|---|---|
| Struktura projektu i konfiguracja | 1h | 1h | Zgodnie z planem |
| Baza danych + migracje | 2h | 3h | Kilka poprawek migracji (User, Item) |
| Endpointy API (auth + items) | 3h | 3h | Zgodnie z planem |
| Integracja Gemini AI | 1h | 1h | Semantic Kernel ulatwil integracje |
| Frontend - layout i strony | 4h | 5h | Wiecej pracy z CSS niz zakladano |
| Frontend - autoryzacja | 2h | 2h | Zgodnie z planem |
| Dockeryzacja | 1h | 2h | Problemy z seedowaniem w kontenerze |
| Dokumentacja | 1h | 1h | Zgodnie z planem |
| **Suma** | **15h** | **18h** | |

### Najwieksze wyzwania

- Konfiguracja CORS miedzy frontendem Blazor a API
- Przesylanie obrazow przez multipart form data (API binduje parametry z query string, obraz z body)
- Seedowanie danych w kontenerze Docker (sciezka do folderu seed rozni sie w kontenerze)
- Parsowanie JWT po stronie klienta do wyswietlania stanu autoryzacji

## 3. Podzial zadan

Projekt jednoosobowy - wszystkie zadania realizowane samodzielnie.

| Obszar | Zakres |
|---|---|
| Backend | Architektura, domena, endpointy API, integracja AI, baza danych |
| Frontend | Layout, strony, serwisy, autoryzacja, CSS |
| DevOps | Docker, docker-compose, konfiguracja srodowiska |
| Dokumentacja | README, dokumentacja techniczna, dokumentacja uzytkownika |

---

# Dokumentacja uzytkownika - ReLoop

## User Stories

### US-1: Przegladanie przedmiotow

**Jako** niezalogowany uzytkownik
**Chce** przegladac dostepne przedmioty na stronie glownej
**Aby** znalezc cos co chce kupic

**Scenariusz:**
1. Otworz strone http://localhost:3000
2. Na stronie glownej wyswietla sie siatka przedmiotow ze zdjeciami, nazwami, cenami i kategoriami
3. Mozna filtrowac przedmioty po kategorii klikajac przyciski filtrowania (All, Clothes, Electronics, Books itd.)
4. Klikniecie na przedmiot otwiera modal ze szczegolami

---

### US-2: Rejestracja konta

**Jako** nowy uzytkownik
**Chce** zalozyc konto w serwisie
**Aby** moc kupowac i sprzedawac przedmioty

**Scenariusz:**
1. Kliknij "Register" w nawigacji
2. Wypelnij formularz: imie, nazwisko, email, haslo
3. Formularz waliduje dane (wymagane pola, format email, minimalna dlugosc hasla)
4. Po rejestracji nastepuje przekierowanie na strone logowania

---

### US-3: Logowanie

**Jako** zarejestrowany uzytkownik
**Chce** zalogowac sie na swoje konto
**Aby** uzyskac dostep do pelnej funkcjonalnosci

**Scenariusz:**
1. Kliknij "Login" w nawigacji
2. Podaj email i haslo
3. Po poprawnym logowaniu: nawigacja pokazuje saldo, email i przycisk Logout
4. Przy blednych danych wyswietla sie komunikat bledu

**Konta testowe:**
- jan.kowalski@email.com / User123!
- anna.nowak@email.com / User123!

---

### US-4: Wystawianie przedmiotu na sprzedaz

**Jako** zalogowany uzytkownik
**Chce** wystawic przedmiot na sprzedaz
**Aby** inni mogli go kupic

**Scenariusz:**
1. Kliknij "Sell" w nawigacji (widoczne tylko po zalogowaniu)
2. Wypelnij formularz: nazwa, opis, cena
3. Dodaj zdjecie przedmiotu (klikajac w pole uploadu)
4. Kliknij "List Item for Sale"
5. AI automatycznie przypisuje kategorie - wyswietla sie po utworzeniu
6. Nastepuje przekierowanie na strone glowna

---

### US-5: Zakup przedmiotu

**Jako** zalogowany uzytkownik
**Chce** kupic przedmiot od innego uzytkownika
**Aby** go posiadac

**Scenariusz:**
1. Na stronie glownej kliknij na przedmiot ktory chcesz kupic
2. W modalu ze szczegolami kliknij "Buy for $X"
3. Kwota zostaje odjeta z salda kupujacego i dodana do salda sprzedajacego
4. Przedmiot zmienia status na "Sold"
5. Saldo w nawigacji odswieza sie automatycznie

**Ograniczenia:**
- Nie mozna kupic wlasnego przedmiotu
- Trzeba miec wystarczajace saldo

---

### US-6: Przegladanie swoich przedmiotow

**Jako** zalogowany uzytkownik
**Chce** zobaczyc liste moich wystawionych przedmiotow
**Aby** sledzic ich status

**Scenariusz:**
1. Kliknij "My Items" w nawigacji
2. Wyswietla sie lista przedmiotow wystawionych przez uzytkownika
3. Kazdy przedmiot ma badge ze statusem (Active / Sold)

---

### US-7: Doladowanie salda

**Jako** zalogowany uzytkownik
**Chce** doladowac saldo konta
**Aby** moc kupowac przedmioty

**Scenariusz:**
1. W nawigacji obok salda kliknij przycisk "+"
2. Saldo zostaje zwiekszone o $100
3. Nowa kwota wyswietla sie od razu w nawigacji

**Uwaga:** Jest to uproszczone rozwiazanie demo w celach pokazowych. W prawdziwej aplikacji doladowanie salda byloby realizowane przez integracjie z systemem platnosci (np. Stripe, PayU).

---

### US-8: Automatyczna kategoryzacja AI

**Jako** sprzedajacy
**Chce** zeby AI automatycznie przypisalo kategorie mojemu przedmiotowi
**Aby** nie musiec recznie wybierac kategorii

**Scenariusz:**
1. Przy wystawianiu przedmiotu podaj nazwe i opis
2. Po kliknieciu "List Item for Sale" backend wysyla dane do Google Gemini
3. AI analizuje nazwe i opis i przypisuje jedna z kategorii (Clothes, Electronics, Books, Sports itd.)
4. Przypisana kategoria wyswietla sie na stronie po utworzeniu przedmiotu
5. Kategoria jest tez widoczna jako badge na karcie przedmiotu na stronie glownej
