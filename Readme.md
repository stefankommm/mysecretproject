# SP1 - Digital Signage

## O Projekte

Tento projekt sa zameriava na vytvorenie univerzálneho riešenia pre digitálne displeje, ktoré umožnuje zobraziť rôzne typy obsahu (ako sú denné menu, propagácie, informácie o počasí, videá a viac) do jednej centralizovanej platformy. Umožňuje užívateľom jednoducho konfigurovať a spravovať obsah zobrazený na rôznych displejoch prostredníctvom intuitívneho dashboardu.

## Hlavné Komponenty
### 1. Backend

- **Funkcie:**
  - REST komunikácia s Frontendami
  - Centrálny backend spravujúci aplikáciu
  
### 2. Frontend-Display

- **Funkcie:**
  - Zobrazovanie konfigurovateľného obsahu.
  - Reálna aktualizácia obsahu podľa plánu.

### 3. Frontend-Dashboard

- **Funkcie:**
  - Prihlásenie a správa užívateľov.
  - Správa zariadení a displejov.
  - Úprava a časové plánovanie obsahu.

## Use Cases

- **Reštaurácie:** Denné menu, propagačné videá.
- **Firmy:** Informácie pre zamestnancov, aktuálne udalosti.
- **Obchody:** Preddefinované videá na reklamných displejoch.

## Widgety (Plánované)

- Počasie, Video/Fotka, Zoznam, Čas, QR Kód, Custom text.

## Technológie

- **Databáza:** PostgreSQL
- **Backend:** .Net Core (REST API, MVC + Repository Service pattern), Middleware na auth.
- **Frontend:** Next.js (React), Tailwind.CSS, Shad/cn - Component Library.

## Začíname

### Požiadavky

- PostgreSQL
- .NET Core SDK
- Node.js

### Inštalácia

1. Spustenie DB pre Backend:
   ```docker
   docker run --name postgres -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -p 5432:5432 -v /var/lib/data -d postgres