# Auth — локальный запуск через Docker Compose

Стек поднимает Auth-сервис: API ([Auth.API](../src/Services/Auth/Auth.API)) + PostgreSQL.
Миграции EF Core применяются автоматически при старте API.

## Требования

- Docker + Docker Compose
- Свободные порты `5104` (API) и `5432` (PostgreSQL) — или переопредели их в `.env`

## Подготовка

Скопируй шаблон переменных и заполни `JWT_SECRET_KEY` (минимум 32 символа, обязателен):

```bash
cp .env.example .env
```

## Запуск

Все команды выполняются из папки `infrastructure/`.

```bash
# Собрать образ API и поднять стек в фоне
docker compose up --build -d
```

После старта:

- API — http://localhost:5104
- Swagger — http://localhost:5104/swagger

## Полезные команды

```bash
# Логи API в реальном времени
docker compose logs -f auth-api

# Статус контейнеров
docker compose ps

# Остановить (контейнеры удаляются, данные БД сохраняются в volume)
docker compose down

# Остановить и стереть данные БД
docker compose down -v

# Пересобрать только API после изменений в коде
docker compose up --build -d auth-api
```

## Переменные окружения

Полный список — в [.env.example](.env.example). Ключевые:

| Переменная | Назначение | По умолчанию |
|---|---|---|
| `JWT_SECRET_KEY` | Ключ для подписи JWT (HS256). **Обязателен** | — |
| `AUTH_API_PORT` | Порт API на хосте | `5104` |
| `AUTH_DB_PORT` | Порт PostgreSQL на хосте | `5432` |
| `AUTH_DB_NAME` / `AUTH_DB_USER` / `AUTH_DB_PASSWORD` | Параметры БД | `auth` / `auth` / — |
| `ASPNETCORE_ENVIRONMENT` | Окружение ASP.NET | `Development` |
| `GOOGLE_CLIENT_ID` / `GOOGLE_CLIENT_SECRET` | Google OAuth (опционально) | пусто |

> `.env` содержит секреты и в репозиторий не коммитится (см. `.gitignore`). В Git хранится только `.env.example`.
