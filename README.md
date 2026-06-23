# Banking System

A modern banking system built with .NET using Microservices Architecture, Clean Architecture, CQRS, MediatR and PostgreSQL.

> 🚧 Project is currently under active development.

## Technologies

- .NET 10
- ASP.NET Core Minimal API
- Entity Framework Core
- PostgreSQL
- Clean Architecture
- CQRS
- MediatR
- FluentValidation
- Domain Events
- Microservices Architecture

## Current Services

### Account Service

Implemented features:

- Create Account
- Get Account By Id
- Deposit Money
- Withdraw Money
- Transfer Money
- Account Transactions
- Account Status Management
- Validation Pipeline
- Global Exception Handling

## Architecture

```text
src
├── BuildingBlocks
│   └── Banking.Shared
│
└── Services
    └── Account
        ├── Domain
        ├── Application
        ├── Infrastructure
        └── Api
```

## Patterns Used

- Clean Architecture
- CQRS
- Vertical Slice Architecture
- Domain Driven Design (DDD)
- Domain Events

## Planned Features

- Customer Service
- Transaction Service
- Notification Service
- RabbitMQ Integration
- API Gateway
- Authentication & Authorization
- Distributed Caching (Redis)
- Docker Support
- Event Driven Communication
- Observability & Logging

## Learning Goals

This project is being developed to gain hands-on experience with:

- Microservices
- Distributed Systems
- Event-Driven Architecture
- Production-Ready Backend Development
- Banking Domain Modeling

## Status

Current Progress:
- Account Service ✅
- Customer Service ✅
- Transaction Service ⏳
- Notification Service ✅
- API Gateway ⏳
```
