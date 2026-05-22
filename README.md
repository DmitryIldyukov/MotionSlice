# TaskFlow - Microservices Platform for Service Marketplace

## Overview
A distributed microservices platform for booking and managing services (freelance marketplace).

## Tech Stack
- .NET 10
- PostgreSQL
- RabbitMQ
- Docker & Kubernetes
- GitLab CI/CD
- ELK Stack

## Architecture
See `/docs/architecture.md` (добавишь позже)

## Getting Started
```bash
docker-compose up -d
```

## Project Structure
src/
├── TaskFlow.API/          # API Gateway
├── TaskFlow.UserService/
├── TaskFlow.ServiceCatalogService/
├── TaskFlow.OrderService/
├── TaskFlow.PaymentService/
└── TaskFlow.NotificationService/
tests/
infrastructure/
docs/

## Development Notes
- Branch strategy: main → develop → feature/*
- Commits: conventional commits (feat:, fix:, refactor:)