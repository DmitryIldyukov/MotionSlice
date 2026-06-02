# Архитектура MotionSlice

## Структура репозитория

```
MotionSlice/
├── src/
│   ├── Services/
│   │   ├── Auth/
│   │   │   ├── Auth.API/
│   │   │   ├── Auth.Application/
│   │   │   ├── Auth.Domain/
│   │   │   └── Auth.Infrastructure/
│   │   ├── Users/
│   │   │   ├── Users.API/
│   │   │   ├── Users.Application/
│   │   │   ├── Users.Domain/
│   │   │   └── Users.Infrastructure/
│   │   └── ArticleSlice/
│   │       ├── ArticleSlice.API/
│   │       ├── ArticleSlice.Application/
│   │       ├── ArticleSlice.Domain/
│   │       └── ArticleSlice.Infrastructure/
│   ├── Gateway/
│   │   └── Gateway/
│   └── Shared/
│       └── Shared.Contracts/
├── tests/
│   ├── Auth/
│   │   ├── Auth.UnitTests/
│   │   └── Auth.IntegrationTests/
│   └── ArticleSlice/
│       ├── ArticleSlice.UnitTests/
│       └── ArticleSlice.IntegrationTests/
├── frontend/
├── infrastructure/
│   ├── docker-compose.yml
│   └── nginx/
└── docs/
```

## Слои каждого сервиса (Clean Architecture + DDD)

| Слой | Ответственность |
|---|---|
| **API** | Controllers, middleware, DI-регистрация, точка входа |
| **Application** | Use cases, команды/запросы (CQRS), DTO, интерфейсы сервисов |
| **Domain** | Агрегаты, сущности, value objects, доменные события, интерфейсы репозиториев |
| **Infrastructure** | EF Core, реализации репозиториев, брокер сообщений, внешние API |

Зависимости направлены строго внутрь: API → Application → Domain. Infrastructure реализует интерфейсы Domain.

## Сервисы

| Сервис | Ответственность |
|---|---|
| **Auth Service** | Регистрация, логин, выдача JWT, смена пароля |
| **User Service** | Профиль пользователя, настройки, баланс |
| **API Gateway** | Маршрутизация, валидация JWT, Feature Flags для фронтенда |
| **ArticleSlice** | Суммаризация статей → готовый пост для соцсетей |
| **VideoSlice** | Нарезка видео/стримов на короткие клипы *(идея)* |
| **VideoGeneration** | Генерация видео с нуля *(идея)* |

## Ключевые архитектурные решения

### Межсервисное взаимодействие
- Сервисы общаются через брокер сообщений (RabbitMQ / Kafka), не напрямую друг с другом
- **Outbox Pattern** — гарантированная доставка событий без потерь при сбоях
- **Saga Pattern** — управление распределёнными транзакциями

### Авторизация
- Auth Service выдаёт JWT-токены
- API Gateway валидирует токен один раз на входе
- Остальные сервисы только читают claims из токена — Auth Service при каждом запросе не вызывается

### Shared.Contracts
- Единственное общее между сервисами — интеграционные события для брокера (например `ArticleSummarizedIntegrationEvent`)
- Никакой бизнес-логики, никаких shared-сущностей

### API Gateway
- Реализуется на **YARP** (Yet Another Reverse Proxy, Microsoft)
- Отдаёт фронтенду конфиг доступных сервисов и их статус (Feature Flags)

### Frontend

Архитектура: **Feature-Sliced Design (FSD)** с прицелом на будущий переход к Micro-frontends (Module Federation).

```
frontend/src/
├── app/               — shell: роутер, провайдеры (в будущем — host для микрофронтендов)
├── pages/
│   ├── auth/
│   ├── profile/
│   └── article-slice/ — (в будущем: отдельный микрофронтенд)
├── widgets/
│   ├── sidebar/
│   └── header/
├── features/
│   ├── auth/
│   └── article-slice/ — изолирован, нет зависимостей на другие features
├── entities/
│   ├── user/
│   └── article/
└── shared/            — ui-кит, api-клиент, хуки, утилиты (в будущем: shared-библиотека между микрофронтендами)
```

**Ключевые правила:**
- Каждый слой импортирует только из слоёв ниже, никогда наоборот
- Сервисы не знают о существовании друг друга — нет импортов между слайсами сервисов
- Всё взаимодействие между сервисами только через `shared/` или `app/`
- Это та же граница, которая при миграции станет границей микрофронтенда без рефакторинга

**При миграции на Module Federation:**
- `app/` становится host-приложением
- Каждый сервис-слайс выносится в отдельный remote
- `shared/` превращается в shared scope Module Federation

**Feature Flags:**
- Список сервисов и их статус приходит с API Gateway, не хардкодится во фронтенде
- `enabled: false` → вкладка скрыта; `status: "error"` → вкладка видна, показывается ошибка
