# Banking System

A modern banking system built with .NET using Microservices Architecture, Clean Architecture, CQRS, MediatR, PostgreSQL, RabbitMQ, Keycloak and Docker Compose.

> 🚧 Project is currently under active development.

## Overview

Banking System is a backend-focused microservice project designed to simulate core banking operations such as customer management, account creation, deposit, withdrawal, money transfer and transaction history.

The project focuses on production-oriented backend concepts such as:

- Microservices Architecture
- Clean Architecture
- CQRS with MediatR
- JWT Authentication with Keycloak
- API Gateway with YARP
- Event-driven communication with RabbitMQ and MassTransit
- Outbox Pattern
- PostgreSQL database per service
- Secure local configuration with User Secrets
- Docker Compose infrastructure

---

## Technologies

- .NET 10
- ASP.NET Core Minimal API
- Entity Framework Core
- PostgreSQL
- RabbitMQ
- MassTransit
- Keycloak
- YARP API Gateway
- Docker Compose
- MediatR
- FluentValidation
- Polly
- CQRS
- Clean Architecture
- Domain Events
- Outbox Pattern

---

## Solution Structure

```text
src
├── BuildingBlocks
│   ├── Banking.Shared
│   ├── Banking.Bus
│   └── Banking.Authentication
│
├── Services
│   ├── Account
│   │   ├── Domain
│   │   ├── Application
│   │   ├── Infrastructure
│   │   └── Api
│   │
│   ├── Customer
│   │   ├── Domain
│   │   ├── Application
│   │   ├── Infrastructure
│   │   └── Api
│   │
│   ├── Notification
│   │   └── Api
│   │
│   └── Gateway
│       └── Api
│
├── docker
│   └── postgres
│       └── init.sql
│
├── docker-compose.yml
└── .env.example
