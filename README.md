###### AdminDashboard – Fullstack .NET + React + Docker







======== PRESENTATION



AdminDashboard est une application fullstack prête à l’emploi permettant de gérer produits et réservations, avec une API REST+GraphQL sécurisée et déployable via Docker.



Stack technique :

Backend : ASP.NET Core (.NET 9)

Frontend : React + TypeScript + Vite
API : REST + GraphQL

Base de données : PostgreSQL

Authentification : JWT (Access + Refresh tokens)

Infra : Docker + Docker Compose + NGINX (reverse proxy)







======== FONCTIONNALITES



Authentification JWT (access + refresh)

CRUD Produits \& Réservations

API REST + GraphQL

Pagination, filtrage, tri

Docker + NGINX reverse proxy

Seed automatique en développement







======== STRUCTURE



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







======== INSTALLATION / CONFIGURATION / LANCEMENT



Prérequis :

Docker Desktop installé



Configuration .env :
Créer les fichiers .env à partir des .env.example
AdminDashboard/.env (Docker)

AdminDashboard/frontend/.env (Vite)



Lancement :
docker compose up --build



Accès :
Frontend : http://localhost:5173

API REST : http://localhost:5000/api

GraphQL : http://localhost:5000/graphql

Swagger : http://localhost:5000/swagger







======== ENDPOINTS



REST :



Auth :
POST	/api/v1/Auth/register	Création d’un compte

POST	/api/v1/Auth/login	Connexion utilisateur

POST	/api/v1/Auth/refresh	Rafraîchissement du token



Products (REST) :
GET	/api/v1/Products?page=\&pageSize=	Liste paginée des produits

GET	/api/v1/Products/{id}	                Détail d’un produit

POST	/api/v1/Products	                Création d’un produit

PUT	/api/v1/Products/{id}	                Mise à jour d’un produit

DELETE	/api/v1/Products/{id}	                Suppression d’un produit



Reservations (REST) :

GET	/api/v1/Reservations?page=\&pageSize=	Liste paginée des réservations

GET	/api/v1/Reservations/{id}	        Détail d’une réservation

POST	/api/v1/Reservations	                Création d’une réservation

PUT	/api/v1/Reservations/{id}	        Mise à jour

DELETE	/api/v1/Reservations/{id}	        Suppression







GraphQL :



Endpoint :

/graphql



query {

&#x20; products(

&#x20;   first: 10

&#x20;   where: { category: { eq: "Electronics" } }

&#x20;   order: \[{ price: DESC }]

&#x20; ) {

&#x20;   nodes {

&#x20;     id

&#x20;     name

&#x20;     price

&#x20;   }

&#x20; }

}



query {

&#x20; paginatedProducts(input: { page: 1, pageSize: 10 }) {

&#x20;   items {

&#x20;     id

&#x20;     name

&#x20;     category

&#x20;     price

&#x20;     stock

&#x20;   }

&#x20;   totalItems

&#x20; }

}







======== CI-CD



CI : GitHub Actions



Trigger CI (push+pull)

Checkout

Setup (front+back)

Installation dépendances (front+back)

Lint/Code quality (front)

Build (front+back)

Tests (back+front)

Build Docker (front+back)

Test docker-compose (check API+GraphQL)

Clean up Docker







CD : Azure Container Apps (désactivé)



Trigger CD (push)

Checkout

Connexion Azure

Connexion Azure Container Registry (ACR)

Build Docker prod (back+front)

Push Docker (back+front) vers ACR

Déploiement (back+front) sur ACA

Attente que l'API REST soit prête

Attente que GraphQL soit prêt

Message de fin du pipeline

