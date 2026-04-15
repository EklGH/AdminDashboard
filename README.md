# AdminDashboard - Fullstack (.NET/React/Docker)
![.NET](https://img.shields.io/badge/.NET-9-blue)
![React](https://img.shields.io/badge/React-TypeScript-blue)
![Docker](https://img.shields.io/badge/Docker-ready-blue)

AdminDashboard est une application fullstack permettant de gérer produits et réservations avec une API REST+GraphQL sécurisée déployable via Docker.

### Stack technique :

- Backend : ASP.NET Core (.NET 9)
- Frontend : React + TypeScript + Vite
- API : REST + GraphQL
- Base de données : PostgreSQL
- Authentification : JWT (Access + Refresh tokens)
- Infra : Docker + NGINX

---

## ⚙️ FONCTIONNALITES

- Authentification JWT (access + refresh)
- CRUD Produits \& Réservations
- API REST + GraphQL
- Pagination, filtrage, tri
- Docker + NGINX
- Seed automatique en développement

---

## 🏗️ STRUCTURE

```
AdminDashboard/
├── backend/                            # API .NET 9 (REST+GraphQL)
│   ├── AdminDashboard.Api/             # Controllers REST, GraphQL, Program.cs, Dockerfile
│   ├── AdminDashboard.Application/     # DTOs, Services, Interfaces
│   ├── AdminDashboard.Domain/          # Entities
│   ├── AdminDashboard.Infrastructure/  # DB, Auth, Repositories
│   └── AdminDashboard.Tests/           # Tests unitaires
│
├── frontend/                           # React + Vite + TypeScript
│   ├── src/                            # components, pages, hooks, services, context, types
│   ├── public/
│   ├── .env                            # .env front Vite
│   └── Dockerfile
│
├── nginx/                              # Reverse proxy (Docker)
├── .github/                            # ci, cd commenté
├── docker-compose.yml
├── .env
└── README.md
```

---

## 🚀 INSTALLATION / CONFIGURATION / LANCEMENT

### Prérequis :
Docker Desktop installé

### Configuration .env :
- Créer les fichiers .env à partir des .env.example
- AdminDashboard/.env (Docker)
- AdminDashboard/frontend/.env (Vite)

### Lancement :
docker compose up --build

### Accès :
- Frontend : http://localhost:5173
- API REST : http://localhost:5000/api
- GraphQL : http://localhost:5000/graphql
- Swagger : http://localhost:5000/swagger

---

## 🔌 ENDPOINTS

### REST :

|Auth|
|
|POST|`/api/v1/Auth/register`|Création d’un compte
|POST|`/api/v1/Auth/login`|Connexion utilisateur
|POST|`/api/v1/Auth/refresh`|Rafraîchissement du token

|Products|
|
|GET|`/api/v1/Products?page=\&pageSize=`|Liste paginée des produits
|GET|`/api/v1/Products/{id}`|Détail d’un produit
|POST|`/api/v1/Products`|Création d’un produit
|PUT|`/api/v1/Products/{id}`|Mise à jour d’un produit
|DELETE|`/api/v1/Products/{id}`|Suppression d’un produit

|Reservations|
|
|GET|`/api/v1/Reservations?page=\&pageSize=`|Liste paginée des réservations
|GET|`/api/v1/Reservations/{id}`|Détail d’une réservation
|POST|`/api/v1/Reservations`|Création d’une réservation
|PUT|`/api/v1/Reservations/{id}`|Mise à jour
|DELETE|`/api/v1/Reservations/{id}`|Suppression

---

### GraphQL :

Produits
```
query {
  products(
    first: 10
    where: { category: { eq: "Electronics" } }
    order: [{ price: DESC }]
  ) {
    nodes {
      id
      name
      price
    }
  }
}
```
Filtre paginé produits
```
query {
  paginatedProducts(input: { page: 1, pageSize: 10 }) {
    items {
      id
      name
      category
      price
      stock
    }
    totalItems
  }
}
```

---

## 🔄 CI-CD

#### CI : GitHub Actions
- Trigger CI (push+pull)
- Checkout
- Setup (front+back)
- Installation dépendances (front+back)
- Lint/Code quality (front)
- Build (front+back)
- Tests (back+front)
- Build Docker (front+back)
- Test docker-compose (check API+GraphQL)
- Clean up Docker

#### CD : Azure Container Apps (désactivé)
- Trigger CD (push)
- Checkout
- Connexion Azure
- Connexion Azure Container Registry (ACR)
- Build Docker prod (back+front)
- Push Docker (back+front) vers ACR
- Déploiement (back+front) sur ACA
- Attente que l'API REST soit prête
- Attente que GraphQL soit prêt
- Message de fin du pipeline

---

## 📄 LICENCE

Projet démonstratif

